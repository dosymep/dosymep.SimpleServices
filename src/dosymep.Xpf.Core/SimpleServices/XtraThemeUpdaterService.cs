using System.Windows;

using DevExpress.Xpf.Core;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Класс по обновлению тем окон.
    /// </summary>
    public class XtraThemeUpdaterService : IUIThemeUpdaterService {
        /// <inheritdoc />
        public void SetTheme(Window window, UIThemes theme) {
            SetTheme((FrameworkElement) window, theme);
        }

        /// <inheritdoc />
        public void SetTheme(FrameworkElement frameworkElement, UIThemes theme) {
            if(theme == UIThemes.Dark) {
                ThemeManager.SetThemeName(frameworkElement, Theme.Win10DarkName);
            } else if(theme == UIThemes.Light) {
                ThemeManager.SetThemeName(frameworkElement, Theme.Win10LightName);
            }
        }
    }
}