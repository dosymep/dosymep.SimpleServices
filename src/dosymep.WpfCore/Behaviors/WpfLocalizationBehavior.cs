using System.Globalization;
using System.Windows;

using dosymep.SimpleServices;

using Microsoft.Xaml.Behaviors;

namespace dosymep.WpfCore.Behaviors;

/// <summary>
/// Поведение применения языка.
/// </summary>
public sealed class WpfLocalizationBehavior : Behavior<FrameworkElement> {
    private IHasLocalization? _localization;
    private ILocalizationService? _localizationService;

    /// <inheritdoc />
    protected override void OnAttached() {
        _localization = AssociatedObject as IHasLocalization;

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
        if(_localization is not null) {
            _localizationService = _localization.LocalizationService;
            _localizationService?.SetLocalization(_localization.HostLanguage, AssociatedObject);

            _localization.LanguageChanged += LanguageOnThemeChanged;
        }
    }

    private void Unsubscribe() {
        if(_localization is not null) {
            _localization.LanguageChanged -= LanguageOnThemeChanged;
        }
    }

    private void LanguageOnThemeChanged(CultureInfo cultureInfo) {
        _localizationService?.SetLocalization(cultureInfo, AssociatedObject);
    }
}