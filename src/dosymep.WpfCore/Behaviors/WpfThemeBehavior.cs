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

        // Если окно было уже загружено
        // такое может быть, когда не используется behaviour в конструкторе
        if(AssociatedObject.IsLoaded) {
            Subscribe();
        }

        AssociatedObject.Loaded += AssociatedObjectOnLoaded;
        AssociatedObject.Unloaded += AssociatedObjectOnUnloaded;
    }

    /// <inheritdoc />
    protected override void OnDetaching() {
        Unsubscribe();
        AssociatedObject.Loaded -= AssociatedObjectOnLoaded;
        AssociatedObject.Unloaded -= AssociatedObjectOnUnloaded;
    }

    private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs e) {
        Subscribe();
    }

    private void AssociatedObjectOnUnloaded(object sender, RoutedEventArgs e) {
        Unsubscribe();
    }

    private void Subscribe() {
        if(_theme != null) {
            _themeUpdaterService = _theme.ThemeUpdaterService;
            _themeUpdaterService?.SetTheme(_theme.HostTheme, AssociatedObject);

            _theme.ThemeChanged += ThemeOnThemeChanged;
        }
    }

    private void Unsubscribe() {
        if(_theme != null) {
            _theme.ThemeChanged -= ThemeOnThemeChanged;
        }
    }

    private void ThemeOnThemeChanged(UIThemes theme) {
        _themeUpdaterService?.SetTheme(theme, AssociatedObject);
    }
}