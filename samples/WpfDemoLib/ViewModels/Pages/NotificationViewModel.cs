using System.Windows.Input;
using System.Windows.Media.Imaging;

using dosymep.SimpleServices;

using WpfDemoLib.ComponentModel;
using WpfDemoLib.Factories;

namespace WpfDemoLib.ViewModels.Pages;

public sealed class NotificationViewModel : ObservableObject {
    public const string NotificationWarningIconResourceName =
        "pack://application:,,,/WpfDemoLib;component/assets/images/icons8-notification-warning-32.png";

    public INotificationService NotificationService { get; }

    public NotificationViewModel(ICommandFactory commandFactory, INotificationService notificationService) {
        NotificationService = notificationService;

        ShowNotificationCommand = commandFactory.CreateAsync(ShowNotificationAsync);
        ShowFatalNotificationCommand = commandFactory.CreateAsync(ShowFatalNotificationAsync);
        ShowWarningNotificationCommand = commandFactory.CreateAsync(ShowWarningNotificationAsync);
    }

    public ICommand ShowNotificationCommand { get; }
    public ICommand ShowFatalNotificationCommand { get; }
    public ICommand ShowWarningNotificationCommand { get; }

    private int _count = 0;

    private async Task ShowNotificationAsync() {
        INotification notification = NotificationService.CreateNotification(
            title: "Title " + _count++,
            body: "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            footer: "IronPython",
            author: "dosymep",
            imageSource: new BitmapImage(new Uri(NotificationWarningIconResourceName, UriKind.Absolute)));

        await notification.ShowAsync();
    }

    private async Task ShowFatalNotificationAsync() {
        INotification notification = NotificationService.CreateFatalNotification(
            title: "IronPython " + _count++,
            body: "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            author: "dosymep",
            imageSource: new BitmapImage(new Uri(NotificationWarningIconResourceName, UriKind.Absolute)));

        await notification.ShowAsync();
    }

    private async Task ShowWarningNotificationAsync() {
        INotification notification = NotificationService.CreateWarningNotification(
            title: "IronPython " + _count++,
            body: "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            author: "dosymep",
            imageSource: new BitmapImage(new Uri(NotificationWarningIconResourceName, UriKind.Absolute)));

        await notification.ShowAsync();
    }
}