using System.Collections;
using System.Globalization;
using System.IO.Packaging;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using dosymep.SimpleServices;
using dosymep.WpfCore.SimpleServices;
using dosymep.WpfCore.MarkupExtensions;
using dosymep.WpfUI.Core.SimpleServices;
using dosymep.WpfUI.Core.Windows;

using Wpf.Ui.Abstractions;

using WpfDemoLib.Input;
using WpfDemoLib.ViewModels;

using WpfUIDemoApp.Views.Pages;

namespace WpfUIDemoApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IHasTheme, IHasLocalization {
    public event Action<UIThemes>? ThemeChanged;
    public event Action<CultureInfo>? LanguageChanged;

    public MainWindow(
        INavigationViewPageProvider pageProvider,
        ILocalizationService localizationService,
        IUIThemeUpdaterService themeUpdaterService) {
        LocalizationService = localizationService;
        ThemeUpdaterService = themeUpdaterService;

        ThemeUpdaterService.SetTheme(HostTheme, this);
        
        InitializeComponent();
        
        _navigationView.SetPageProviderService(pageProvider);
        Loaded += (_, _) => _navigationView.Navigate(typeof(GridViewPage));
    }

    public ILocalizationService LocalizationService { get; set; }
    public IUIThemeUpdaterService ThemeUpdaterService { get; set; }

    public UIThemes HostTheme => _themesComboBox?.SelectedValue == null
        ? UIThemes.Light
        : (UIThemes) _themesComboBox.SelectedValue;

    public CultureInfo HostLanguage => _langsComboBox?.SelectedValue == null
        ? CultureInfo.GetCultureInfo("ru-RU")
        : CultureInfo.GetCultureInfo((string) _langsComboBox.SelectedValue);

    private void ButtonOk_OnClick(object sender, RoutedEventArgs e) {
        Close();
    }

    private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) {
        Close();
    }

    private void Theme_Changed(object sender, SelectionChangedEventArgs e) {
        ThemeChanged?.Invoke(HostTheme);
    }

    private void Language_Changed(object sender, SelectionChangedEventArgs e) {
        LanguageChanged?.Invoke(HostLanguage);
    }
}