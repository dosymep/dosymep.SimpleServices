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

    internal class RegStorage : IProfileStorage {
        private const string ProfileNameValueName = "ProfileName";
        
        private const string ProfileUriValueName = "ProfileUri";
        private const string ProfileStorageValueName = "ProfileStorage";

        private const string GitProfileGitBranchValueName = "GitBranch";
        private const string GitProfileGitUsernameValueName = "GitUsername";
        private const string GitProfileGitPasswordValueName = "GitPassword";

        private readonly string _regPath;
        private readonly string _applicationVersion;
        private readonly ISerializationService _serializationService;

        public RegStorage(string regPath, string applicationVersion, ISerializationService serializationService) {
            _regPath = regPath;
            _applicationVersion = applicationVersion;
            _serializationService = serializationService;
        }

        public string ProfileName {
            get => (string) Registry.GetValue(_regPath, ProfileNameValueName, null);
            set => Registry.SetValue(_regPath, ProfileNameValueName, value);
        }

        public ProfileInfo[] GetProfileInfos() {
            IEnumerable<ProfileInfo> currentUser = GetSubKeyNames(Registry.CurrentUser, false);
            IEnumerable<ProfileInfo> localMachine = GetSubKeyNames(Registry.LocalMachine, true);

            return localMachine.Union(currentUser).ToArray();
        }

        public IProfileInstance LoadProfileSpace(ProfileInfo profileInfo, ProfileSpace profileSpace) {
            return CreateProfileDefinition(profileInfo, profileSpace);
        }

        private IProfileInstance CreateProfileDefinition(ProfileInfo profileInfo, ProfileSpace profileSpace) {
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
                        Password = (string) Registry.GetValue(keyFullName, GitProfileGitPasswordValueName, null),
                        ApplicationVersion = _applicationVersion,
                        SerializationService = _serializationService,
                    };
                case ProfileStorage.Directory:
                    return new DirectoryProfileInstance(profileInfo, profileUri, profileSpace) {
                        ApplicationVersion = _applicationVersion,
                        SerializationService = _serializationService,
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
    }
}