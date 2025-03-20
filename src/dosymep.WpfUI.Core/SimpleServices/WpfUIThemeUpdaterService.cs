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
        SetTheme((FrameworkElement) window, theme);
    }

    /// <inheritdoc />
    public void SetTheme(FrameworkElement frameworkElement, UIThemes theme) {
        _theme.Theme = GetAppTheme(theme);
        if(!frameworkElement.Resources.MergedDictionaries.Contains(_theme)) {
            frameworkElement.Resources.MergedDictionaries.Add(_theme);
            frameworkElement.Resources.MergedDictionaries.Add(_controlsDictionary);
        }

        if(frameworkElement is Window window) {
            WindowBackgroundManager.UpdateBackground(window, GetAppTheme(theme), WindowBackdropType.Mica);
        }
    }

    private static ApplicationTheme GetAppTheme(UIThemes theme) {
        return theme switch {
            UIThemes.Dark => ApplicationTheme.Dark,
            UIThemes.Light => ApplicationTheme.Light,
            _ => ApplicationTheme.Unknown
        };
    }
}