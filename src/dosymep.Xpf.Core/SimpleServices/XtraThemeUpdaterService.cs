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
            SetTheme(theme, window);
        }

        /// <inheritdoc />
        public void SetTheme(UIThemes theme, FrameworkElement frameworkElement) {
            if(theme == UIThemes.Dark) {
                ThemeManager.SetThemeName(frameworkElement, Theme.Win10DarkName);
            } else if(theme == UIThemes.Light) {
                ThemeManager.SetThemeName(frameworkElement, Theme.Win10LightName);
            }
        }
    }
}