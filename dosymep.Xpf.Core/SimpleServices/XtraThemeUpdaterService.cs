using System.Windows;

using DevExpress.Xpf.Core;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Класс по обновлению тем окон.
    /// </summary>
    public class XtraThemeUpdaterService : IThemeUpdaterService {
        /// <inheritdoc />
        public void SetTheme(Window window, UIThemes theme) {
            if(theme == UIThemes.Dark) {
                ThemeManager.SetThemeName(window, Theme.Win10DarkName);
            } else if(theme == UIThemes.Light) {
                ThemeManager.SetThemeName(window, Theme.Win10LightName);
            }
        }
    }
}