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
        _localizationService = _localization?.LocalizationService;
        AssociatedObject.Initialized += AssociatedObjectOnInitialized;
    }

    /// <inheritdoc />
    protected override void OnDetaching() {
        if(_localization is not null) {
            _localization.LanguageChanged -= LanguageOnThemeChanged;
        }
    }

    private void AssociatedObjectOnInitialized(object sender, EventArgs e) {
        AssociatedObject.Initialized -= AssociatedObjectOnInitialized;
        
        if(_localization is not null) {
            LanguageOnThemeChanged(_localization.HostLanguage);
            _localization.LanguageChanged += LanguageOnThemeChanged;
        }
    }

    private void LanguageOnThemeChanged(CultureInfo cultureInfo) {
        _localizationService?.SetLocalization(cultureInfo, AssociatedObject);
    }
}