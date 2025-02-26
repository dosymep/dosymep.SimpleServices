using System.Globalization;
using System.Reflection;
using System.Windows;

using dosymep.SimpleServices;

namespace WpfUIDemoApp.Views;

public partial class SecondWindow : IHasTheme, IHasLocalization {
    private readonly IHasTheme _hasTheme;
    private readonly IHasLocalization _hasLocalization;

    public event Action<UIThemes>? ThemeChanged;
    public event Action<CultureInfo>? LanguageChanged;

    public SecondWindow(
        IHasTheme hasTheme,
        IHasLocalization hasLocalization) {
        _hasTheme = hasTheme;
        _hasLocalization = hasLocalization;

        _hasTheme.ThemeChanged += _ => ThemeChanged?.Invoke(_);
        _hasLocalization.LanguageChanged += _ => LanguageChanged?.Invoke(_);

        InitializeComponent();
    }

    public IUIThemeUpdaterService ThemeUpdaterService => _hasTheme.ThemeUpdaterService;
    public ILocalizationService LocalizationService => _hasLocalization.LocalizationService;

    public UIThemes HostTheme => _hasTheme.HostTheme;
    public CultureInfo HostLanguage => _hasLocalization.HostLanguage;

    private void ButtonOk_OnClick(object sender, RoutedEventArgs e) {
        if(IsDialog()) {
            DialogResult = true;
        }

        Close();
    }

    private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) {
        if(IsDialog()) {
            DialogResult = false;
        }

        Close();
    }

    private bool IsDialog() {
        BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
        return (bool?) typeof(Window)
            .GetField("_showingAsDialog", bindingFlags)
            ?.GetValue(this) == true;
    }
}