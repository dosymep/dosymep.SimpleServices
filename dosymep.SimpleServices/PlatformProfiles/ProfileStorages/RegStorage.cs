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
        
        private const string ProfileUriValueName = "ProfileUri";
        private const string ProfileStorageValueName = "ProfileStorage";

        private const string GitProfileGitBranchValueName = "GitBranch";
        private const string GitProfileGitUsernameValueName = "GitUsername";
        private const string GitProfileGitPasswordValueName = "GitPassword";

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
            get => (string) Registry.GetValue(_regPath, ProfileNameValueName, null);
            set => Registry.SetValue(_regPath, ProfileNameValueName, value);
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

            return profileDefinition;
        }

        private ProfileInstance CreateProfileDefinition(ProfileInfo profileInfo, ProfileSpace profileSpace) {
            string keyFullName = GetKeyFullName(profileInfo, profileSpace);

            string profileUri =
                (string) Registry.GetValue(keyFullName, ProfileUriValueName, null);

            ProfileStorage profileStorage =
                (ProfileStorage) Enum.Parse(typeof(ProfileStorage),
                    (string) Registry.GetValue(keyFullName, ProfileStorageValueName, nameof(ProfileStorage.Unknown)));

            switch(profileStorage) {
                case ProfileStorage.Git:
                    return new GitProfileInstance(profileInfo, profileUri, profileSpace) {
                        Branch = (string) Registry.GetValue(keyFullName, GitProfileGitBranchValueName, null),
                        Username = (string) Registry.GetValue(keyFullName, GitProfileGitUsernameValueName, null),
                        Password = (string) Registry.GetValue(keyFullName, GitProfileGitPasswordValueName, null)
                    };
                case ProfileStorage.Directory:
                    return new DirectoryProfileInstance(profileInfo, profileUri, profileSpace);
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
    }
}