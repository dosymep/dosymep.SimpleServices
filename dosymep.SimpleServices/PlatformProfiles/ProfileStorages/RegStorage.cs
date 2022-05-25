using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        private const string ProfileAllowCopyName = "ProfileAllowCopy";
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
            get => GetRegistryValue<string>(_regPath, ProfileNameValueName);
            set => SetRegistryValue(_regPath, ProfileNameValueName, value);
        }

        /// <inheritdoc />
        public string ProfileLocalPath {
            get => GetRegistryValue<string>(_regPath, ProfileProfileLocalPathName);
            set => SetRegistryValue(_regPath, ProfileProfileLocalPathName, value);
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
            ProfileInstance profileDefinition = CreateProfileDefinition(profileInfo, profileSpace);
            profileDefinition.ApplicationVersion = _applicationVersion;
            profileDefinition.SerializationService = _serializationService;
            profileDefinition.CopyProfile(ProfileLocalPath);

            return profileDefinition;
        }

        private ProfileInstance CreateProfileDefinition(ProfileInfo profileInfo, ProfileSpace profileSpace) {
            string keyFullName = GetKeyFullName(profileInfo, profileSpace);

            string profileUri =
                GetRegistryValue<string>(keyFullName, ProfileUriValueName);

            ProfileStorage profileStorage =
                (ProfileStorage) Enum.Parse(typeof(ProfileStorage),
                    (string) Registry.GetValue(keyFullName, ProfileStorageValueName, nameof(ProfileStorage.Unknown)));

            switch(profileStorage) {
                case ProfileStorage.Git:
                    return new GitProfileInstance(profileInfo, profileUri, profileSpace) {
                        Credentials = GetCredentials(keyFullName),
                        AllowCopy = GetRegistryValue<bool>(keyFullName, ProfileAllowCopyName),
                        Branch = GetRegistryValue<string>(keyFullName, GitProfileBranchValueName),
                    };
                case ProfileStorage.Directory:
                    return new DirectoryProfileInstance(profileInfo, profileUri, profileSpace) {
                        Credentials = GetCredentials(keyFullName),
                        AllowCopy = GetRegistryValue<bool>(keyFullName, ProfileAllowCopyName)
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
            return Path.Combine(profileInfo.FullName, profileSpace.ToString());
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
    }
}