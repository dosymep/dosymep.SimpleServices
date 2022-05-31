using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;

using Microsoft.Win32;

namespace dosymep.SimpleServices.PlatformProfiles.ProfileStorages {
    internal enum ProfileStorage {
        Unknown,
        Git,
        Directory
    }

    /// <summary>
    /// Класс хранилища профиля в реестре.
    /// </summary>
    public class RegStorage : IProfileStorage {
        private const string ProfileNameValueName = "ProfileName";
        private const string ProfileProfileLocalPathName = "ProfileLocalPath";

        private const string ProfileUriValueName = "ProfileUri";
        private const string ProfileStorageValueName = "ProfileStorage";

        private const string ProfileCredentialsValueName = "Credentials";
        private const string ProfileCredentialsUsernameValueName = "Username";
        private const string ProfileCredentialsPasswordValueName = "Password";

        private const string GitProfileBranchValueName = "Branch";

        private readonly string _regPath;
        private readonly string _applicationVersion;
        private readonly ISerializationService _serializationService;

        /// <summary>
        /// Конструктор хранилища профиля в реестре.
        /// </summary>
        /// <param name="regPath">Относительный путь до хранилища в реестре.</param>
        /// <param name="applicationVersion">Версия приложения.</param>
        /// <param name="serializationService">Сервис сериализации.</param>
        public RegStorage(string regPath, string applicationVersion, ISerializationService serializationService) {
            _regPath = regPath;
            _applicationVersion = applicationVersion;
            _serializationService = serializationService;
        }

        /// <inheritdoc />
        public string ProfileName {
            get =>
                GetRegistryValue<string>(Registry.LocalMachine, ProfileNameValueName)
                    ?? GetRegistryValue<string>(Registry.CurrentUser, ProfileNameValueName);
            set {
                try {
                    SetRegistryValue(Registry.LocalMachine, ProfileNameValueName, value);
                } catch(SecurityException) {
                    SetRegistryValue(Registry.CurrentUser, ProfileNameValueName, value);
                }
            }
        }

        /// <inheritdoc />
        public string ProfileLocalPath {
            get =>
                GetRegistryValue<string>(Registry.LocalMachine, ProfileProfileLocalPathName)
                ?? GetRegistryValue<string>(Registry.CurrentUser, ProfileProfileLocalPathName);
            set {
                try {
                    SetRegistryValue(Registry.LocalMachine, ProfileProfileLocalPathName, value);
                } catch(SecurityException) {
                    SetRegistryValue(Registry.CurrentUser, ProfileProfileLocalPathName, value);
                }
            }
        }

        /// <inheritdoc />
        public ProfileInfo[] GetProfileInfos() {
            IEnumerable<ProfileInfo> currentUser = GetSubKeyNames(Registry.CurrentUser, false);
            IEnumerable<ProfileInfo> localMachine = GetSubKeyNames(Registry.LocalMachine, true);

            return localMachine.Union(currentUser).ToArray();
        }

        /// <inheritdoc />
        public ProfileInfo GetCurrentProfileInfo() {
            return GetProfileInfos()
                .FirstOrDefault(item => item.Name.Equals(ProfileName));
        }

        /// <inheritdoc />
        public IProfileInstance LoadProfileSpace(ProfileInfo profileInfo, ProfileSpace profileSpace) {
            ProfileInstance profileDefinition = CreateProfileInstance(profileInfo, profileSpace);
            profileDefinition.ApplicationVersion = _applicationVersion;
            profileDefinition.SerializationService = _serializationService;
            profileDefinition.LoadProfile();

            return profileDefinition;
        }

        private ProfileInstance CreateProfileInstance(ProfileInfo profileInfo, ProfileSpace profileSpace) {
            string keyFullName = GetKeyFullName(profileInfo, profileSpace);

            string profileUri =
                GetRegistryValue<string>(keyFullName, ProfileUriValueName);

            ProfileStorage profileStorage =
                GetRegistryValue(keyFullName, ProfileStorageValueName, ProfileStorage.Unknown);

            switch(profileStorage) {
                case ProfileStorage.Git:
                    return new GitProfileInstance(profileInfo, profileUri, profileSpace) {
                        ProfileLocalPath = ProfileLocalPath,
                        Credentials = GetCredentials(keyFullName),
                        Branch = GetRegistryValue<string>(keyFullName, GitProfileBranchValueName)
                    };
                case ProfileStorage.Directory:
                    return new DirectoryProfileInstance(profileInfo, profileUri, profileSpace) {
                        ProfileLocalPath = ProfileLocalPath,
                        Credentials = GetCredentials(keyFullName),
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerable<ProfileInfo> GetSubKeyNames(RegistryKey root, bool isReadOnly) {
            using(RegistryKey subKey = root.OpenSubKey(_regPath)) {
                return subKey?.GetSubKeyNames()
                           .Select(item => GetProfileName(root, item, isReadOnly))
                           .OrderBy(item => item.Name)
                       ?? Enumerable.Empty<ProfileInfo>();
            }
        }

        private ProfileInfo GetProfileName(RegistryKey root, string profileName, bool isReadOnly) {
            return new ProfileInfo {
                Name = profileName, FullName = Path.Combine(root.Name, _regPath, profileName), IsReadOnly = isReadOnly
            };
        }

        private string GetKeyFullName(ProfileInfo profileInfo, ProfileSpace profileSpace) {
            return Path.Combine(profileInfo.FullName, profileSpace.Name);
        }

        private Credentials GetCredentials(string keyFullName) {
            keyFullName = Path.Combine(keyFullName, ProfileCredentialsValueName);
            return new Credentials() {
                Username = GetRegistryValue<string>(keyFullName, ProfileCredentialsUsernameValueName),
                Password = GetRegistryValue<string>(keyFullName, ProfileCredentialsPasswordValueName),
            };
        }

        private T GetRegistryValue<T>(string keyFullName, string keyName, T defaultValue = default) {
            return (T) Registry.GetValue(keyFullName, keyName, defaultValue);
        }

        private void SetRegistryValue<T>(string keyFullName, string keyName, T value) {
            Registry.SetValue(keyFullName, keyName, value);
        }

        private T GetRegistryValue<T>(RegistryKey registryKey, string keyName, T defaultValue = default) {
            string keyFullName = Path.Combine(registryKey.Name, _regPath);
            return GetRegistryValue(keyFullName, keyName, defaultValue);
        }

        private void SetRegistryValue<T>(RegistryKey registryKey, string keyName, T value) {
            string keyFullName = Path.Combine(registryKey.Name, _regPath);
            SetRegistryValue(keyFullName, keyName, value);
        }
    }
}