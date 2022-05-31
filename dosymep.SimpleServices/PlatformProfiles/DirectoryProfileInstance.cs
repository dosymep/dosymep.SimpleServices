using System;
using System.IO;

namespace dosymep.SimpleServices.PlatformProfiles {
    internal class DirectoryProfileInstance : ProfileInstance {
        public DirectoryProfileInstance(string profileLocalPath, string profileOriginalPath, 
            ProfileInfo profileInfo, ProfileSpace profileSpace)
            : base(profileLocalPath, profileOriginalPath, profileInfo, profileSpace) {
        }

        protected override void LoadProfileImpl() {
            if(Directory.Exists(ProfileOriginalPath)) {
                RemoveProfile();
                CopyDirectory(ProfileOriginalPath, LocalPath, true);
            }
        }

        protected override void CommitProfileImpl(string pluginConfigPath) {
            // Директория не сохраняет лог изменений (возможно в будущем можно будет сохранять лог изменений)
        }
    }
}