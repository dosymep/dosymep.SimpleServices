using System.Globalization;
using System.Windows;

using dosymep.SimpleServices;

using Microsoft.Xaml.Behaviors;

namespace dosymep.Wpf.Core.Behaviors;

/// <summary>
/// Поведение применения языка.
/// </summary>
public sealed class WpfLocalizationBehavior : Behavior<Window> {
    private ILocalizationService? _localizationService;

    /// <inheritdoc />
    protected override void OnAttached() {
        Subscribe(AssociatedObject as IHasLocalization);
    }

    /// <inheritdoc />
    protected override void OnDetaching() {
        Unsubscribe(AssociatedObject as IHasLocalization);
    }

    private void Subscribe(IHasLocalization? localization) {
        if(localization is not null) {
            _localizationService = localization.LocalizationService;
            _localizationService?.SetLocalization(localization.HostLanguage, AssociatedObject);
            
            localization.LanguageChanged += LanguageOnThemeChanged;
        }
    }

    private void Unsubscribe(IHasLocalization? localization) {
        if(localization is not null) {
            localization.LanguageChanged -= LanguageOnThemeChanged;
        }
    }

    private void LanguageOnThemeChanged(CultureInfo cultureInfo) {
        _localizationService?.SetLocalization(cultureInfo, AssociatedObject);
    }
}