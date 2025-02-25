using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Media.Imaging;

using dosymep.SimpleServices;
using dosymep.WpfCore.Ninject;
using dosymep.WpfUI.Core.Ninject;

using Ninject;

using WpfDemoLib.Input;
using WpfDemoLib.Input.Interfaces;
using WpfDemoLib.ViewModels;

namespace WpfUIDemoApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App {
    public const string LocalizationResourceName =
        "/WpfUIDemoApp;component/assets/localizations/language.xaml";

    public const string NotificationIconResourceName =
        "pack://application:,,,/WpfUIDemoApp;component/assets/images/icons8-notification-32.png";

    public const string NotificationWarningIconResourceName =
        "pack://application:,,,/WpfUIDemoApp;component/assets/images/icons8-notification-warning-32.png";

    private IKernel? _kernel;

    protected override void OnStartup(StartupEventArgs e) {
        base.OnStartup(e);

        _kernel = new StandardKernel();

        _kernel.Bind<ICommandFactory>().To<RelayCommandFactory>();

        _kernel.UseWpfDispatcher();

        _kernel.UseWpfWindowsLanguage();
        _kernel.UseWpfLocalization(LocalizationResourceName, CultureInfo.GetCultureInfo("ru-RU"));

        _kernel.UseWpfWindowsTheme();
        _kernel.UseWpfUIThemeUpdater();

        ILocalizationService localizationService = _kernel.Get<ILocalizationService>();

        _kernel.UseWpfUIMessageBox<MainViewModel>();

        _kernel.UseWpfUIProgressDialog<MainViewModel>(
            displayTitleFormat: localizationService.GetLocalizedString("ProgressDialog.Content"));

        _kernel.BindWindow<MainViewModel, MainWindow>();
        
        _kernel.Get<MainWindow>().Show();
    }

    protected override void OnExit(ExitEventArgs e) {
        base.OnExit(e);
        _kernel?.Dispose();
    }
}