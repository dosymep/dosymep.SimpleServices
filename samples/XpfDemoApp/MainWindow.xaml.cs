using DevExpress.Xpf.Core;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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

using DevExpress.Xpf.Editors;

using dosymep.SimpleServices;
using dosymep.WpfCore.SimpleServices;
using dosymep.Xpf.Core.SimpleServices;

using WpfDemoLib.Input;
using WpfDemoLib.ViewModels;

namespace XpfDemoApp {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IHasTheme, IHasLocalization {
        public event Action<UIThemes>? ThemeChanged;
        public event Action<CultureInfo>? LanguageChanged;

        public MainWindow() {
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            ThemeUpdaterService = new XtraThemeUpdaterService();
            LocalizationService = new WpfLocalizationService(
                $"/{assemblyName};component/assets/localizations/language.xaml", CultureInfo.GetCultureInfo("ru-RU"));

            var uiThemeService = new XtraWindowsThemeService();
            
            MessageBoxService = new XtraMessageBoxService() {
                UIThemeService = uiThemeService, UIThemeUpdaterService = ThemeUpdaterService
            };
            
            ProgressDialogFactory = new ProgressDialogFactory() {
                DisplayTitleFormat = LocalizationService.GetLocalizedString("ProgressDialog.Content"),
                UIThemeService = uiThemeService, UIThemeUpdaterService = ThemeUpdaterService
            };

            DataContext = new MainViewModel(
                new RelayCommandFactory(),
                MessageBoxService, LocalizationService, ProgressDialogFactory);

            InitializeComponent();

            Theme_Changed(this, new EditValueChangedEventArgs(UIThemes.Light, HostTheme));
        }

        public ILocalizationService LocalizationService { get; }
        public IUIThemeUpdaterService ThemeUpdaterService { get; }
        public IMessageBoxService MessageBoxService { get; set; }
        public IProgressDialogFactory ProgressDialogFactory { get; set; }

        public UIThemes HostTheme => _themesComboBox?.EditValue == null
            ? UIThemes.Light
            : (UIThemes) _themesComboBox.EditValue;

        public CultureInfo HostLanguage => _langsComboBox?.EditValue == null
            ? CultureInfo.GetCultureInfo("ru-RU")
            : CultureInfo.GetCultureInfo((string) _langsComboBox.EditValue);

        private void ButtonOk_OnClick(object sender, RoutedEventArgs e) {
            Close();
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) {
            Close();
        }

        private void Theme_Changed(object sender, EditValueChangedEventArgs editValueChangedEventArgs) {
            ThemeChanged?.Invoke(HostTheme);
        }

        private void Language_Changed(object sender, EditValueChangedEventArgs editValueChangedEventArgs) {
            LanguageChanged?.Invoke(HostLanguage);
        }
    }
}