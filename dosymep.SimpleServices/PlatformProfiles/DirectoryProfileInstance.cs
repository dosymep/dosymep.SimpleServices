using System;
using System.IO;

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

        protected override string GetPluginConfigPath(string pluginName, string settingsName) {
            return GetDirectoryPath(base.GetPluginConfigPath(pluginName, settingsName));
        }
    }
}