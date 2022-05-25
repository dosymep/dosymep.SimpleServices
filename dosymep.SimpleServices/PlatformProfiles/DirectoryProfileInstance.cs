using System;

namespace dosymep.SimpleServices.PlatformProfiles {
    internal class DirectoryProfileInstance : ProfileInstance {
        public DirectoryProfileInstance(ProfileInfo profileInfo, string profileUri, ProfileSpace profileSpace)
            : base(profileUri, profileInfo, profileSpace) {
        }

        public string ComputedProfileUri => GetDirectoryPath(ProfileUri);

        protected override void CopyProfileImp(string directory) {
            RemoveProfile(directory);
            CopyProfile(ProfileUri, directory, true);
        }

        protected override T GetProfileSettingsImp<T>(string pluginName, string settingsName) {
            throw new NotImplementedException();
        }

        protected override void SaveProfileSettingsImpl<T>(T settings, string pluginName, string settingsName) {
            throw new NotImplementedException();
        }

        private string GetDirectoryPath(string profileUri) {
            return Environment.ExpandEnvironmentVariables(profileUri)
                .Replace($"%{Environment.SpecialFolder.MyDocuments}%",
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        }
    }
}