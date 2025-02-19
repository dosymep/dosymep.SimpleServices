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
using DevExpress.Xpf.Utils.Themes;

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

        public MainWindow(MainViewModel mainViewModel,
            ILocalizationService localizationService,
            IUIThemeUpdaterService themeUpdaterService) {
            DataContext = mainViewModel;

            LocalizationService = localizationService;
            ThemeUpdaterService = themeUpdaterService;

            InitializeComponent();

            Theme_Changed(this, new EditValueChangedEventArgs(UIThemes.Light, HostTheme));
        }

        public ILocalizationService LocalizationService { get; }
        public IUIThemeUpdaterService ThemeUpdaterService { get; }

        public UIThemes HostTheme => _themesComboBox?.EditValue == null
            ? UIThemes.Light
            : (UIThemes) _themesComboBox.EditValue;

        public CultureInfo HostLanguage => _langsComboBox?.EditValue == null
            ? CultureInfo.GetCultureInfo("ru-RU")
            : CultureInfo.GetCultureInfo((string) _langsComboBox.EditValue);

        private async void ButtonOk_OnClick(object sender, RoutedEventArgs e) {
            IsEnabled = false;
            await Task.Delay(2000);
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