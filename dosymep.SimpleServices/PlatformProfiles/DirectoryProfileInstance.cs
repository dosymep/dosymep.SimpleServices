using System;
using System.IO;

namespace dosymep.SimpleServices.PlatformProfiles {
    internal class DirectoryProfileInstance : ProfileInstance {
        public DirectoryProfileInstance(ProfileInfo profileInfo, string profileUri, ProfileSpace profileSpace)
            : base(profileUri, profileInfo, profileSpace) {
        }

        public string ComputedProfileUri => GetDirectoryPath(ProfileUri);

        protected override void LoadProfileImpl(string directory) {
            RemoveProfile(directory);
            LoadProfile(ProfileUri, directory, true);
        }

        protected override void CommitProfileImpl(string pluginConfigPath) {
            throw new NotImplementedException();
        }

        protected override string GetPluginConfigPath(string pluginName, string settingsName) {
            return GetDirectoryPath(base.GetPluginConfigPath(pluginName, settingsName));
        }
    }
}