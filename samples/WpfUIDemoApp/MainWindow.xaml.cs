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

    private async void ButtonOk_OnClick(object sender, RoutedEventArgs e) {
        MessageBoxResult result = MessageBoxService.Show(
            LocalizationService.GetLocalizedString("MessageBox.Content"),
            LocalizationService.GetLocalizedString("MessageBox.Title"),
            MessageBoxButton.YesNo, MessageBoxImage.Information);

        try {
            if(result == MessageBoxResult.Yes) {
                using IProgressDialogService dialog = ProgressDialogFactory.CreateDialog();

                dialog.DisplayTitleFormat = LocalizationService.GetLocalizedString("ProgressDialog.Content");
                dialog.MaxValue = 10;

                dialog.Show();

                IProgress<int> progress = dialog.CreateProgress();
                CancellationToken cancellationToken = dialog.CreateCancellationToken();
                for(int i = 0; i < 10; i++) {
                    progress.Report(i);
                    cancellationToken.ThrowIfCancellationRequested();

                    Thread.Sleep(1000);
                }
            }
        } catch(OperationCanceledException) {
            //pass
        }
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