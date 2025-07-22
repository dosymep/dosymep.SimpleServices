using System.Windows;

using dosymep.SimpleServices;

using Microsoft.Xaml.Behaviors;

namespace dosymep.WpfCore.Behaviors;

/// <summary>
/// Поведение применения темы.
/// </summary>
public sealed class WpfThemeBehavior : Behavior<FrameworkElement> {
    private IHasTheme? _theme;
    private IUIThemeUpdaterService? _themeUpdaterService;

    /// <inheritdoc />
    protected override void OnAttached() {
        _theme = AssociatedObject as IHasTheme;
        _themeUpdaterService = _theme?.ThemeUpdaterService;
        AssociatedObject.Initialized += AssociatedObjectOnInitialized;
    }

    /// <inheritdoc />
    protected override void OnDetaching() {
        if(_theme is not null) {
            _theme.ThemeChanged -= ThemeOnThemeChanged;
        }
    }

    private void AssociatedObjectOnInitialized(object sender, EventArgs e) {
        AssociatedObject.Initialized -= AssociatedObjectOnInitialized;
        if(_theme is not null) {
            ThemeOnThemeChanged(_theme.HostTheme);
            _theme.ThemeChanged += ThemeOnThemeChanged;
        }
    }

    private void ThemeOnThemeChanged(UIThemes theme) {
        _themeUpdaterService?.SetTheme(theme, AssociatedObject);
    }
}