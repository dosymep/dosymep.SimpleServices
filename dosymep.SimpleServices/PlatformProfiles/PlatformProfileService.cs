using System;

using dosymep.SimpleServices.PlatformProfiles.ProfileStorages;

namespace dosymep.SimpleServices.PlatformProfiles {
    /// <summary>
    /// Класс сервиса профилей платформы.
    /// </summary>
    public class PlatformProfileService : 
        IPlatformProfileService,
        IPlatformProfile<UserProfileSpace>,
        IPlatformProfile<SystemProfileSpace>,
        IPlatformProfile<OrganizationProfileSpace> {
        /// <summary>
        /// Хранилище профилей.
        /// </summary>
        private readonly IProfileStorage _profileStorage;

        /// <summary>
        /// Сохраняет экземпляр профиля.
        /// </summary>
        /// <param name="profileStorage">Хранилище профилей.</param>
        /// <param name="profileInfo">Информация о профиле.</param>
        public PlatformProfileService(IProfileStorage profileStorage, ProfileInfo profileInfo) {
            if(profileStorage == null) {
                throw new ArgumentNullException(nameof(profileStorage));
            }

            if(profileInfo == null) {
                throw new ArgumentNullException(nameof(profileInfo));
            }

            _profileStorage = profileStorage;

            User = _profileStorage.LoadProfileSpace(profileInfo, ProfileSpace.UserProfileSpace);
            System = _profileStorage.LoadProfileSpace(profileInfo, ProfileSpace.SystemProfileSpace);
            Organization = _profileStorage.LoadProfileSpace(profileInfo, ProfileSpace.OrganizationProfileSpace);
        }

        /// <inheritdoc />
        public IProfileInstance User { get; }
        
        /// <inheritdoc />
        public IProfileInstance System { get; }
        
        /// <inheritdoc />
        public IProfileInstance Organization { get; }

        #region IPlatformProfile<UserProfileSpace>

        /// <inheritdoc />
        T IPlatformProfile<UserProfileSpace>.GetPluginSettings<T>(string pluginName) {
            if(string.IsNullOrEmpty(pluginName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pluginName));
            }

            return User.GetProfileSettings<T>(pluginName);
        }

        /// <inheritdoc />
        T IPlatformProfile<UserProfileSpace>.GetPluginSettings<T>(string pluginName, string settingsName) {
            if(string.IsNullOrEmpty(pluginName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pluginName));
            }

            if(string.IsNullOrEmpty(settingsName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(settingsName));
            }

            return User.GetProfileSettings<T>(pluginName, settingsName);
        }

        /// <inheritdoc />
        void IPlatformProfile<UserProfileSpace>.SavePluginSettings<T>(T settings, string pluginName) {
            if(settings == null) {
                throw new ArgumentNullException(nameof(settings));
            }

            if(string.IsNullOrEmpty(pluginName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pluginName));
            }

            User.SaveProfileSettings(settings, pluginName);
        }

        /// <inheritdoc />
        void IPlatformProfile<UserProfileSpace>.SavePluginSettings<T>(T settings, string pluginName, string settingsName) {
            if(settings == null) {
                throw new ArgumentNullException(nameof(settings));
            }

            if(string.IsNullOrEmpty(pluginName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pluginName));
            }

            if(string.IsNullOrEmpty(settingsName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(settingsName));
            }

            User.SaveProfileSettings(settings, pluginName, settingsName);
        }

        #endregion

        #region IPlatformProfile<SystemProfileSpace>

        /// <inheritdoc />
        T IPlatformProfile<SystemProfileSpace>.GetPluginSettings<T>(string pluginName) {
            if(string.IsNullOrEmpty(pluginName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pluginName));
            }

            return System.GetProfileSettings<T>(pluginName);
        }

        /// <inheritdoc />
        T IPlatformProfile<SystemProfileSpace>.GetPluginSettings<T>(string pluginName, string settingsName) {
            if(string.IsNullOrEmpty(pluginName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pluginName));
            }

            if(string.IsNullOrEmpty(settingsName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(settingsName));
            }

            return System.GetProfileSettings<T>(pluginName, settingsName);
        }

        /// <inheritdoc />
        void IPlatformProfile<SystemProfileSpace>.SavePluginSettings<T>(T settings, string pluginName) {
            if(settings == null) {
                throw new ArgumentNullException(nameof(settings));
            }

            if(string.IsNullOrEmpty(pluginName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pluginName));
            }

            System.SaveProfileSettings(settings, pluginName);
        }

        /// <inheritdoc />
        void IPlatformProfile<SystemProfileSpace>.SavePluginSettings<T>(T settings, string pluginName, string settingsName) {
            if(settings == null) {
                throw new ArgumentNullException(nameof(settings));
            }

            if(string.IsNullOrEmpty(pluginName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pluginName));
            }

            if(string.IsNullOrEmpty(settingsName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(settingsName));
            }

            System.SaveProfileSettings(settings, pluginName, settingsName);
        }

        #endregion

        #region IPlatformProfile<OrganizationProfileSpace>

        /// <inheritdoc />
        T IPlatformProfile<OrganizationProfileSpace>.GetPluginSettings<T>(string pluginName) {
            if(string.IsNullOrEmpty(pluginName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pluginName));
            }

            return Organization.GetProfileSettings<T>(pluginName);
        }

        /// <inheritdoc />
        T IPlatformProfile<OrganizationProfileSpace>.GetPluginSettings<T>(string pluginName, string settingsName) {
            if(string.IsNullOrEmpty(pluginName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pluginName));
            }

            if(string.IsNullOrEmpty(settingsName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(settingsName));
            }

            return Organization.GetProfileSettings<T>(pluginName, settingsName);
        }

        /// <inheritdoc />
        void IPlatformProfile<OrganizationProfileSpace>.SavePluginSettings<T>(T settings, string pluginName) {
            if(settings == null) {
                throw new ArgumentNullException(nameof(settings));
            }

            if(string.IsNullOrEmpty(pluginName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pluginName));
            }

            Organization.SaveProfileSettings(settings, pluginName);
        }

        /// <inheritdoc />
        void IPlatformProfile<OrganizationProfileSpace>.SavePluginSettings<T>(T settings, string pluginName,
            string settingsName) {
            if(settings == null) {
                throw new ArgumentNullException(nameof(settings));
            }

            if(string.IsNullOrEmpty(pluginName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pluginName));
            }

            if(string.IsNullOrEmpty(settingsName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(settingsName));
            }

            Organization.SaveProfileSettings(settings, pluginName, settingsName);
        }

        #endregion
    }
}