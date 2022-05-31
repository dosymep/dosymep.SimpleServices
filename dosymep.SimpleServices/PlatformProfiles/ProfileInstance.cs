using System;
using System.IO;

namespace dosymep.SimpleServices.PlatformProfiles {
    internal class Credentials {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    
    internal abstract class ProfileInstance : IProfileInstance {
        public ProfileInstance(string profileUri, ProfileInfo profileInfo, ProfileSpace profileSpace) {
            ProfileUri = profileUri;
            ProfileInfo = profileInfo;
            ProfileSpace = profileSpace;
        }

        public string Name => ProfileInfo.Name;
        public bool IsReadOnly => ProfileInfo.IsReadOnly;

        public string ProfileUri { get; }
        public ProfileInfo ProfileInfo { get; }
        public ProfileSpace ProfileSpace { get; }
       
        public string ProfileLocalPath { get; set; }
        public string ApplicationVersion { get; set; }

        public Credentials Credentials { get; set; }
        public ISerializationService SerializationService { get; set; }

        public void LoadProfile() {
            string directory = GetProfileName(ProfileLocalPath);
            try {
                LoadProfileImpl(directory);
            } catch {
                RemoveProfile(directory);
                LoadProfileImpl(directory);
            }
        }
        
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

        protected abstract void LoadProfileImpl(string directory);
        protected abstract void CommitProfileImpl(string pluginConfigPath);
        
        protected string GetProfileName(string directory) {
            return GetDirectoryPath(Path.Combine(directory, ProfileSpace.Name, ProfileInfo.Name));
        }

        protected virtual string GetPluginConfigPath(string pluginName, string settingsName) {
            return Path.Combine(ProfileLocalPath, ProfileSpace.Name, ProfileInfo.Name,
                ApplicationVersion, pluginName, settingsName) + SerializationService.FileExtension;
        }

        protected void RemoveProfile(string directory) {
            if(Directory.Exists(directory)) {
                File.SetAttributes(directory, FileAttributes.Normal);
                foreach(string fileSystemEntry
                        in Directory.GetFileSystemEntries(directory, "*", SearchOption.AllDirectories)) {
                    File.SetAttributes(fileSystemEntry, FileAttributes.Normal);
                }

                try {
                    Directory.Delete(directory, true);
                } catch(IOException) {
                    Directory.Delete(directory, true);
                }
            }
        }

        protected void LoadProfile(string source, string destination, bool recursive) {
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
                    LoadProfile(subDirectoryInfo.FullName, newDestinationDir, true);
                }
            }
        }
        
        protected string GetDirectoryPath(string directory) {
            return Environment.ExpandEnvironmentVariables(directory)
                .Replace($"%{Environment.SpecialFolder.MyDocuments}%",
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        }

        private T GetProfileSettingsImp<T>(string pluginName, string settingsName) {
            string pluginConfigPath = GetPluginConfigPath(pluginName, settingsName);
            return SerializationService.Deserialize<T>(File.ReadAllText(pluginConfigPath));
        }

        private void SaveProfileSettingsImpl<T>(T settings, string pluginName, string settingsName) {
            string pluginConfigPath = GetPluginConfigPath(pluginName, settingsName);
            Directory.CreateDirectory(Path.GetDirectoryName(pluginConfigPath));
            File.WriteAllText(pluginConfigPath, SerializationService.Serialize(settings));
        }
    }
}