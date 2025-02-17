using System.Windows;

using dosymep.SimpleServices;

using Microsoft.Xaml.Behaviors;

namespace dosymep.Wpf.Core.Behaviors;

/// <summary>
/// Поведение применения темы.
/// </summary>
public sealed class WpfThemeBehavior : Behavior<Window> {
    private IUIThemeUpdaterService? _themeUpdaterService;

    /// <inheritdoc />
    protected override void OnAttached() {
        Subscribe(AssociatedObject as IHasTheme);
    }

    /// <inheritdoc />
    protected override void OnDetaching() {
        Unsubscribe(AssociatedObject as IHasTheme);
    }

    private void Subscribe(IHasTheme? theme) {
        if(theme is not null) {
            _themeUpdaterService = theme.ThemeUpdaterService;
            _themeUpdaterService?.SetTheme(AssociatedObject, theme.HostTheme);

            theme.ThemeChanged += ThemeOnThemeChanged;
        }
    }

    private void Unsubscribe(IHasTheme? theme) {
        if(theme is not null) {
            theme.ThemeChanged -= ThemeOnThemeChanged;
        }
    }

    private void ThemeOnThemeChanged(UIThemes theme) {
        _themeUpdaterService?.SetTheme(AssociatedObject, theme);
    }
}