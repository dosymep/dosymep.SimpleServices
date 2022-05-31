using System;
using System.IO;

namespace dosymep.SimpleServices.PlatformProfiles {
    internal class Credentials {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    internal abstract class ProfileInstance : IProfileInstance {
        public ProfileInstance(string profileLocalPath, string profileOriginalPath, ProfileInfo profileInfo,
            ProfileSpace profileSpace) {
            ProfileLocalPath = profileLocalPath;
            ProfileOriginalPath = profileOriginalPath;

            ProfileInfo = profileInfo;
            ProfileSpace = profileSpace;
        }

        public string Name => ProfileInfo.Name;
        public bool IsReadOnly => ProfileInfo.IsReadOnly;


        public string ProfileLocalPath { get; }
        public string ProfileOriginalPath { get; }


        public ProfileInfo ProfileInfo { get; }
        public ProfileSpace ProfileSpace { get; }


        public Credentials Credentials { get; set; }
        public string ApplicationVersion { get; set; }
        public ISerializationService SerializationService { get; set; }


        public string LocalPath => GetLocalPath(ProfileLocalPath);
        public string OriginalPath => ExpandEnvironmentVariables(ProfileOriginalPath);


        public void LoadProfile() {
            LoadProfileImpl();
        }

        protected abstract void LoadProfileImpl();
        protected abstract void CommitProfileImpl(string pluginConfigPath);

        protected string GetLocalPath(string profileLocalPath) {
            return ExpandEnvironmentVariables(Path.Combine(profileLocalPath, ProfileSpace.Name, ProfileInfo.Name));
        }

        protected string GetProfileConfigPath(string pluginName, string settingsName) {
            return Path.Combine(LocalPath, ApplicationVersion, pluginName, settingsName)
                   + SerializationService.FileExtension;
        }

        protected void RemoveProfile() {
            if(Directory.Exists(LocalPath)) {
                File.SetAttributes(LocalPath, FileAttributes.Normal);
                foreach(string fileSystemEntry
                        in Directory.GetFileSystemEntries(LocalPath, "*", SearchOption.AllDirectories)) {
                    File.SetAttributes(fileSystemEntry, FileAttributes.Normal);
                }

                try {
                    Directory.Delete(LocalPath, true);
                } catch(IOException) {
                    Directory.Delete(LocalPath, true);
                }
            }
        }

        protected void CopyDirectory(string source, string destination, bool recursive) {
            // Get information about the source directory
            DirectoryInfo directoryInfo = new DirectoryInfo(source);

            // Check if the source directory exists
            if(!directoryInfo.Exists) {
                throw new DirectoryNotFoundException($"Source directory not found: {directoryInfo.FullName}");
            }

            // Cache directories before we start copying
            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destination);

            // Get the files in the source directory and copy to the destination directory
            foreach(FileInfo fileInfo in directoryInfo.GetFiles()) {
                string targetFilePath = Path.Combine(destination, fileInfo.Name);
                fileInfo.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if(recursive) {
                foreach(DirectoryInfo subDirectoryInfo in directoryInfos) {
                    string newDestinationDir = Path.Combine(destination, subDirectoryInfo.Name);
                    CopyDirectory(subDirectoryInfo.FullName, newDestinationDir, true);
                }
            }
        }

        protected string ExpandEnvironmentVariables(string directory) {
            return Environment.ExpandEnvironmentVariables(directory)
                .Replace($"%{Environment.SpecialFolder.MyDocuments}%",
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        }

        private T GetProfileSettingsImp<T>(string pluginName, string settingsName) {
            string pluginConfigPath = GetProfileConfigPath(pluginName, settingsName);
            return SerializationService.Deserialize<T>(File.ReadAllText(pluginConfigPath));
        }

        private void SaveProfileSettingsImpl<T>(T settings, string pluginName, string settingsName) {
            string pluginConfigPath = GetProfileConfigPath(pluginName, settingsName);
            Directory.CreateDirectory(Path.GetDirectoryName(pluginConfigPath));
            File.WriteAllText(pluginConfigPath, SerializationService.Serialize(settings));
        }

        #region IProfileInstance

        public T GetProfileSettings<T>(string pluginName) {
            if(string.IsNullOrEmpty(pluginName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pluginName));
            }

            return GetProfileSettingsImp<T>(pluginName, typeof(T).Name);
        }

        public T GetProfileSettings<T>(string pluginName, string settingsName) {
            if(string.IsNullOrEmpty(settingsName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(settingsName));
            }

            if(string.IsNullOrEmpty(pluginName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pluginName));
            }

            return GetProfileSettingsImp<T>(pluginName, settingsName);
        }

        public void SaveProfileSettings<T>(T settings, string pluginName) {
            if(settings == null) {
                throw new ArgumentNullException(nameof(settings));
            }

            if(string.IsNullOrEmpty(pluginName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pluginName));
            }

            SaveProfileSettingsImpl(settings, pluginName, typeof(T).Name);
        }

        public void SaveProfileSettings<T>(T settings, string pluginName, string settingsName) {
            if(settings == null) {
                throw new ArgumentNullException(nameof(settings));
            }

            if(string.IsNullOrEmpty(settingsName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(settingsName));
            }

            if(string.IsNullOrEmpty(pluginName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pluginName));
            }

            SaveProfileSettingsImpl(settings, pluginName, settingsName);
        }

        #endregion
    }
}