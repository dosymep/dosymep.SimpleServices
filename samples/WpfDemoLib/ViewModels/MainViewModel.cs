using System.Windows;
using System.Windows.Media.Imaging;

using dosymep.SimpleServices;

using WpfDemoLib.Input.Interfaces;

namespace WpfDemoLib.ViewModels;

public sealed class MainViewModel : ObservableObject {
    public const string NotificationWarningIconResourceName =
        "pack://application:,,,/WpfDemoLib;component/assets/images/icons8-notification-warning-32.png";

    public MainViewModel(
        ICommandFactory commandFactory,
        IMessageBoxService messageBoxService,
        ILocalizationService localizationService,
        IProgressDialogFactory progressDialogFactory,
        INotificationService? notificationService) {
        MessageBoxService = messageBoxService;
        LocalizationService = localizationService;
        ProgressDialogFactory = progressDialogFactory;
        NotificationService = notificationService;

        LoadViewCommand = commandFactory.CreateAsync(LoadAsync);
        AcceptViewCommand = commandFactory.CreateAsync(AcceptAsync);
    }

    public IMessageBoxService MessageBoxService { get; }
    public ILocalizationService LocalizationService { get; }
    public IProgressDialogFactory ProgressDialogFactory { get; }
    public INotificationService? NotificationService { get; }

    public IAsyncRelayCommand LoadViewCommand { get; set; }
    public IAsyncRelayCommand AcceptViewCommand { get; set; }

    private async Task LoadAsync() {
        using(IProgressDialogService progressDialogService = ProgressDialogFactory.CreateDialog()) {
            progressDialogService.Show();
            progressDialogService.MaxValue = 100;

            IProgress<int> progress = progressDialogService.CreateAsyncProgress();
            CancellationToken cancellationToken = progressDialogService.CreateCancellationToken();

            try {
                await AsyncOperation(progressDialogService.MaxValue, progress, cancellationToken);

                progressDialogService.Close();
                MessageBoxService.Show(
                    LocalizationService.GetLocalizedString("MainWindow.LoadedContent"),
                    LocalizationService.GetLocalizedString("MainWindow.LoadedTitle"),
                    MessageBoxButton.OK, MessageBoxImage.Information);

                if(NotificationService is not null) {
                    await NotificationService.CreateNotification(
                            LocalizationService.GetLocalizedString("MainWindow.LoadedTitle"),
                            LocalizationService.GetLocalizedString("MainWindow.LoadedContent"))
                        .ShowAsync();
                }
            } catch(OperationCanceledException) {
                progressDialogService.Close();
                MessageBoxService.Show(
                    LocalizationService.GetLocalizedString("MainWindow.NotLoadedContent"),
                    LocalizationService.GetLocalizedString("MainWindow.LoadedTitle"),
                    MessageBoxButton.OK, MessageBoxImage.Information);

                if(NotificationService is not null) {
                    await NotificationService.CreateWarningNotification(
                            LocalizationService.GetLocalizedString("MainWindow.LoadedTitle"),
                            LocalizationService.GetLocalizedString("MainWindow.NotLoadedContent"),
                            imageSource: BitmapFrame.Create(new Uri(NotificationWarningIconResourceName,
                                UriKind.Absolute)))
                        .ShowAsync();
                }
            }
        }
    }

    private async Task AcceptAsync() {
        if(NotificationService is not null) {
            await NotificationService.CreateNotification(
                    LocalizationService.GetLocalizedString("MainWindow.CloseTitle"),
                    LocalizationService.GetLocalizedString("MainWindow.CloseContent"))
                .ShowAsync();
        }
    }

    private async Task AsyncOperation(int maxValue, IProgress<int>? progress, CancellationToken cancellationToken) {
        for(int i = 0; i < maxValue; i++) {
            progress?.Report(i);
            cancellationToken.ThrowIfCancellationRequested();

            await Task.Delay(100, cancellationToken);
        }
    }
}