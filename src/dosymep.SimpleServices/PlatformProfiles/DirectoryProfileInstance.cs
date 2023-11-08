using System;
using System.IO;

namespace dosymep.SimpleServices.PlatformProfiles {
    internal class DirectoryProfileInstance : ProfileInstance {
        public DirectoryProfileInstance(string profileLocalPath, string profileOriginalPath,
            ProfileInfo profileInfo, ProfileSpace profileSpace)
            : base(profileLocalPath, profileOriginalPath, profileInfo, profileSpace) {
        }

        public override string LocalPath => GetLocalPath();
        public override string OriginalPath => GetOriginalPath();

        protected override void LoadProfileImpl() {
            if(IsNotLocalPath() && Directory.Exists(OriginalPath)) {
                RemoveProfile();
                CopyDirectory(OriginalPath, LocalPath, true);
            }
        }

        protected override void CommitProfileImpl(string pluginConfigPath) {
            // Директория не сохраняет лог изменений (возможно в будущем можно будет сохранять лог изменений)
        }

        private string GetLocalPath() {
            return IsNotLocalPath() ? base.LocalPath : OriginalPath;
        }

        private string GetOriginalPath() {
            return IsNotLocalPath()
                ? base.OriginalPath
                : ExpandEnvironmentVariables(ProfileOriginalPath);
        }

        private bool IsNotLocalPath() {
            var rootPath = Path.GetPathRoot(ExpandEnvironmentVariables(ProfileOriginalPath));
            var driveInfo = new DriveInfo(rootPath);
            return driveInfo.DriveType == DriveType.Network;
        }
    }
}