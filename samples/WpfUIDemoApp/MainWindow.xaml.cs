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

using WpfDemoLib.Input;
using WpfDemoLib.ViewModels;

namespace WpfUIDemoApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IHasTheme, IHasLocalization {
    public event Action<UIThemes>? ThemeChanged;
    public event Action<CultureInfo>? LanguageChanged;

    public MainWindow() {
        string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

        ThemeUpdaterService = new WpfUIThemeUpdaterService();
        LocalizationService = new WpfLocalizationService(
            $"/{assemblyName};component/assets/localizations/language.xaml", CultureInfo.GetCultureInfo("ru-RU"));

        MessageBoxService = new WpfUIMessageBoxService(this, this);
        ProgressDialogFactory = new WpfUIProgressDialogFactory(this, this);

        DataContext = new MainViewModel(
            new RelayCommandFactory(),
            MessageBoxService, LocalizationService, ProgressDialogFactory);

        InitializeComponent();
    }


    public ILocalizationService LocalizationService { get; }
    public IUIThemeUpdaterService ThemeUpdaterService { get; }
    public IMessageBoxService MessageBoxService { get; set; }
    public IProgressDialogFactory ProgressDialogFactory { get; set; }

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