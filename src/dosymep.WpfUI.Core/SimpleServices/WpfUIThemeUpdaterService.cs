using System.Windows;

using dosymep.SimpleServices;

using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Markup;

namespace dosymep.WpfUI.Core.SimpleServices;

/// <summary>
/// Обновляет тему для библиотеки WIN-UI.
/// </summary>
public sealed class WpfUIThemeUpdaterService : IUIThemeUpdaterService {
    private ThemesDictionary _theme = new();
    private ControlsDictionary _controlsDictionary = new();

    /// <inheritdoc />
    public void SetTheme(Window window, UIThemes theme) {
        window.Resources.MergedDictionaries.Remove(_theme);
        window.Resources.MergedDictionaries.Remove(_controlsDictionary);

        ApplicationTheme appTheme = ApplicationTheme.Unknown;
        if(theme == UIThemes.Dark) {
            appTheme = ApplicationTheme.Dark;
        } else if(theme == UIThemes.Light) {
            appTheme = ApplicationTheme.Light;
        }

        _theme.Theme = appTheme;
        window.Resources.MergedDictionaries.Add(_theme);
        window.Resources.MergedDictionaries.Add(_controlsDictionary);

        WindowBackgroundManager.UpdateBackground(window, appTheme, WindowBackdropType.Mica);
    }
}