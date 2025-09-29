using System.Windows.Controls;

using dosymep.SimpleServices;

using WpfDemoLib.ViewModels;
using WpfDemoLib.ViewModels.Pages;

namespace WpfUIDemoApp.Views.Pages;

public partial class NotificationPage {
    public NotificationPage(
        IHasTheme hasTheme,
        IHasLocalization hasLocalization,
        NotificationViewModel notificationViewModel)
        : base(hasTheme, hasLocalization) {
        InitializeComponent();
        DataContext = notificationViewModel;
    }
}