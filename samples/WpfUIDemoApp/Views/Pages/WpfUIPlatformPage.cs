using System.Globalization;
using System.Windows.Controls;

using dosymep.SimpleServices;
using dosymep.WpfCore.Behaviors;

using Microsoft.Xaml.Behaviors;

namespace WpfUIDemoApp.Views.Pages {
    public class WpfUIPlatformPage : Page, IHasTheme, IHasLocalization {
        public event Action<UIThemes>? ThemeChanged;
        public event Action<CultureInfo>? LanguageChanged;
        
        private readonly IHasTheme? _hasTheme;
        private readonly IHasLocalization? _hasLocalization;

        public WpfUIPlatformPage() { }

        public WpfUIPlatformPage(
            IHasTheme hasTheme,
            IHasLocalization hasLocalization) {
            _hasTheme = hasTheme;
            _hasLocalization = hasLocalization;

            _hasTheme.ThemeChanged += _ => ThemeChanged?.Invoke(_);
            _hasLocalization.LanguageChanged += _ => LanguageChanged?.Invoke(_);
            
            Interaction.GetBehaviors(this).Add(new WpfThemeBehavior());
            Interaction.GetBehaviors(this).Add(new WpfLocalizationBehavior());
        }

        public IUIThemeUpdaterService ThemeUpdaterService => _hasTheme.ThemeUpdaterService;
        public ILocalizationService LocalizationService => _hasLocalization.LocalizationService;

        public UIThemes HostTheme => _hasTheme.HostTheme;
        public CultureInfo HostLanguage => _hasLocalization.HostLanguage;
    }
}
