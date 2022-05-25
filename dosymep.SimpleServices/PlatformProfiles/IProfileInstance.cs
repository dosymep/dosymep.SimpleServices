namespace dosymep.SimpleServices.PlatformProfiles {
    public interface IProfileInstance {
        T GetProfileSettings<T>(string pluginName);
        T GetProfileSettings<T>(string pluginName, string settingsName);

        void SaveProfileSettings<T>(T settings, string pluginName);
        void SaveProfileSettings<T>(T settings, string pluginName, string settingsName);
    }
}