using System;
using DevExpress.Xpf.Core;

using dosymep.SimpleServices;

using Microsoft.Win32;

namespace dosymep.Xpf.Core.SimpleServices {
    public class XtraWindowsThemeService : IUIThemeService, IDisposable {
        public event Action<UIThemes> UIThemeChanged;
        
        public XtraWindowsThemeService() {
            ApplicationThemeHelper.ApplicationThemeName = Theme.NoneName;
            SystemEvents.UserPreferenceChanged += OnSystemEventsOnUserPreferenceChanged;
        }
        
        public UIThemes HostTheme { get; private set; } = UIThemes.Light;

        private void OnSystemEventsOnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e) {
            if(e.Category == UserPreferenceCategory.General) {
                HostTheme = ThemeIsLight() ? UIThemes.Light : UIThemes.Dark;
                UIThemeChanged?.Invoke(HostTheme);
            }
        }

        private static bool ThemeIsLight() {
            RegistryKey registry =
                Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            
            return (int?) registry?.GetValue("AppsUseLightTheme") == 1;
        }

        protected virtual void Dispose(bool disposing) {
            if(disposing) {
                SystemEvents.UserPreferenceChanged -= OnSystemEventsOnUserPreferenceChanged;
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}