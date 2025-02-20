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
        IProgressDialogFactory progressDialogFactory) {
        MessageBoxService = messageBoxService;
        LocalizationService = localizationService;
        ProgressDialogFactory = progressDialogFactory;

        LoadViewCommand = commandFactory.CreateAsync(LoadAsync);
        AcceptViewCommand = commandFactory.CreateAsync(AcceptAsync);
    }

    public IMessageBoxService MessageBoxService { get; }
    public ILocalizationService LocalizationService { get; }
    public IProgressDialogFactory ProgressDialogFactory { get; }

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
            } catch(OperationCanceledException) {
                progressDialogService.Close();
                MessageBoxService.Show(
                    LocalizationService.GetLocalizedString("MainWindow.NotLoadedContent"),
                    LocalizationService.GetLocalizedString("MainWindow.LoadedTitle"),
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

    private async Task AcceptAsync() {
        await Task.Delay(1000);
    }

    private async Task AsyncOperation(int maxValue, IProgress<int>? progress, CancellationToken cancellationToken) {
        for(int i = 0; i < maxValue; i++) {
            progress?.Report(i);
            cancellationToken.ThrowIfCancellationRequested();

            await Task.Delay(100, cancellationToken);
        }
    }
}