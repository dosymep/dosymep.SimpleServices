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

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public WpfUIThemeUpdaterService() {
        SetResource("/Expander.xaml", GetResource("/Expander.xaml"));
        SetResource("/CardExpander.xaml", GetResource("/CardExpander.xaml"));
    }

    /// <inheritdoc />
    public void SetTheme(Window window, UIThemes theme) {
        SetTheme(theme, window);
    }

    /// <inheritdoc />
    public void SetTheme(UIThemes theme, FrameworkElement frameworkElement) {
        _theme.Theme = GetAppTheme(theme);
        if(!frameworkElement.Resources.MergedDictionaries.Contains(_theme)) {
            frameworkElement.Resources.MergedDictionaries.Add(_theme);
            frameworkElement.Resources.MergedDictionaries.Add(_controlsDictionary);
        }

        if(frameworkElement is Window window) {
            WindowBackgroundManager.UpdateBackground(window, GetAppTheme(theme), WindowBackdropType.Mica);
        }
    }

    private ResourceDictionary? GetResource(string resourceName) {
        return _controlsDictionary.MergedDictionaries
            .FirstOrDefault(item => item.Source.AbsolutePath.EndsWith(resourceName));
    }

    private void SetResource(string resourceName, ResourceDictionary? resourceDictionary) {
        if(resourceDictionary is not null) {
            resourceDictionary.Source =
                new Uri("pack://application:,,,/dosymep.WpfUI.Core;component/Controls" + resourceName);
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