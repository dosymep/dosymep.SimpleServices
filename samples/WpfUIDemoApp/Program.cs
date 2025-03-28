using System.Globalization;
using System.Windows;

using dosymep.SimpleServices;
using dosymep.WpfCore.Ninject;
using dosymep.WpfUI.Core.Ninject;

using Ninject;

using Wpf.Ui.Abstractions;

using WpfDemoLib.Factories;
using WpfDemoLib.Services;
using WpfDemoLib.ViewModels;

using WpfUIDemoApp.Factories;
using WpfUIDemoApp.Views;

namespace WpfUIDemoApp;

public class Program : Application {
    public const string LocalizationResourceName =
        "/WpfUIDemoApp;component/assets/localizations/language.xaml";
    public const string NotificationIconResourceName =
        "pack://application:,,,/WpfUIDemoApp;component/assets/images/icons8-notification-32.png";
    public const string NotificationWarningIconResourceName =
        "pack://application:,,,/WpfUIDemoApp;component/assets/images/icons8-notification-warning-32.png";
    private static IKernel? _kernel;
    
    [STAThread]
    public static void Main() {
        Program app = new();
        
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
        
         _kernel.Bind<ISecondViewService>().To<SecondViewService>();
         _kernel.Bind<ISecondViewFactory>()
             .ToMethod(c => new SecondViewFactory<SecondWindow>(() => c.Kernel.Get<SecondWindow>()));
        
         _kernel.Bind<INavigationViewPageProvider>()
             .To<NavigationViewPageProvider>();
        
         _kernel.Bind<SecondWindow>().ToSelf();
         _kernel.Bind<SecondViewModel>().ToSelf();
        
         _kernel.BindMainWindow<MainViewModel, MainWindow>();
        
         
         app.Run(_kernel.Get<MainWindow>());
    }
}