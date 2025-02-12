using System.Windows;

using dosymep.SimpleServices;

namespace dosymep.WpfUI.Core.SimpleServices;

/// <summary>
/// Класс сервиса окна сообщений. 
/// </summary>
public sealed class WpfUIMessageBoxService : WpfUIBaseService, IMessageBoxService {
    private readonly ILocalizationService _localizationService;
    private readonly IUIThemeService _uiThemeService;
    private readonly IUIThemeUpdaterService _uiThemeUpdaterService;

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="localizationService">Сервис локализации.</param>
    /// <param name="uiThemeService">Сервис тем.</param>
    /// <param name="uiThemeUpdaterService">Сервис обновления тем.</param>
    public WpfUIMessageBoxService(
        ILocalizationService localizationService,
        IUIThemeService uiThemeService,
        IUIThemeUpdaterService uiThemeUpdaterService) {
        _localizationService = localizationService;
        _uiThemeService = uiThemeService;
        _uiThemeUpdaterService = uiThemeUpdaterService;
    }

    /// <inheritdoc />
    public MessageBoxResult Show(string messageBoxText,
        string caption,
        MessageBoxButton button,
        MessageBoxImage icon,
        MessageBoxResult defaultResult) {
        global::Wpf.Ui.Controls.MessageBox messageBox = new() {
            Owner = GetWindow(),
            MinWidth = 300,
            ShowTitle = true,
            Title = caption,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        messageBox.Content = new {ImageSource = GetImageSource(icon), MessageBoxText = messageBoxText};
        messageBox.Resources.MergedDictionaries.Add(
            new ResourceDictionary() {Source = new Uri("../Views/MessageBoxContentTemplate.xaml")});

        void UpdateTheme(UIThemes uiTheme) {
            _uiThemeUpdaterService.SetTheme(messageBox, uiTheme);
        }

        try {
            _uiThemeService.UIThemeChanged += UpdateTheme;
            _uiThemeUpdaterService.SetTheme(messageBox, _uiThemeService.HostTheme);

            (MessageBoxResult closeResult,
                MessageBoxResult primaryResult,
                MessageBoxResult secondaryResult) = ShowMessageBox(button, messageBox);

            return messageBox.ShowDialogAsync().Result switch {
                global::Wpf.Ui.Controls.MessageBoxResult.None => closeResult,
                global::Wpf.Ui.Controls.MessageBoxResult.Primary => primaryResult,
                global::Wpf.Ui.Controls.MessageBoxResult.Secondary => secondaryResult,
                _ => MessageBoxResult.None
            };
        } finally {
            _uiThemeService.UIThemeChanged -= UpdateTheme;
        }
    }

    private Uri GetImageSource(MessageBoxImage icon) {
        string symbol = icon switch {
            MessageBoxImage.None => "assets/icons8-empty-26.png",
            MessageBoxImage.Hand => "assets/icons8-cross-26.png",
            MessageBoxImage.Question => "assets/icons8-question-26.png",
            MessageBoxImage.Exclamation => "assets/icons8-error-26.png",
            MessageBoxImage.Asterisk => "assets/icons8-exclamation-26.png",
            _ => throw new ArgumentOutOfRangeException(nameof(icon), icon, default)
        };

        return new Uri(symbol);
    }

    private (MessageBoxResult closeResult,
        MessageBoxResult primaryResult,
        MessageBoxResult secondaryResult)
        ShowMessageBox(MessageBoxButton button, global::Wpf.Ui.Controls.MessageBox messageBox) {
        if(button == MessageBoxButton.OK) {
            messageBox.IsPrimaryButtonEnabled = false;
            messageBox.IsSecondaryButtonEnabled = false;
            messageBox.CloseButtonText = _localizationService.GetLocalizedString("MessageBox.Ok");
            messageBox.CloseButtonAppearance = global::Wpf.Ui.Controls.ControlAppearance.Info;

            return (MessageBoxResult.OK, MessageBoxResult.None, MessageBoxResult.None);
        } else if(button == MessageBoxButton.OKCancel) {
            messageBox.IsPrimaryButtonEnabled = true;
            messageBox.IsSecondaryButtonEnabled = true;
            messageBox.CloseButtonText = _localizationService.GetLocalizedString("MessageBox.Cancel");
            messageBox.PrimaryButtonText = _localizationService.GetLocalizedString("MessageBox.Ok");
            messageBox.PrimaryButtonAppearance = global::Wpf.Ui.Controls.ControlAppearance.Info;

            return (MessageBoxResult.Cancel, MessageBoxResult.OK, MessageBoxResult.None);
        } else if(button == MessageBoxButton.YesNo) {
            messageBox.IsPrimaryButtonEnabled = true;
            messageBox.IsSecondaryButtonEnabled = false;
            messageBox.CloseButtonText = _localizationService.GetLocalizedString("MessageBox.No");
            messageBox.PrimaryButtonText = _localizationService.GetLocalizedString("MessageBox.Yes");
            messageBox.PrimaryButtonAppearance = global::Wpf.Ui.Controls.ControlAppearance.Info;

            return (MessageBoxResult.No, MessageBoxResult.Yes, MessageBoxResult.None);
        } else if(button == MessageBoxButton.YesNoCancel) {
            messageBox.IsPrimaryButtonEnabled = true;
            messageBox.IsSecondaryButtonEnabled = true;
            messageBox.CloseButtonText = _localizationService.GetLocalizedString("MessageBox.Cancel");
            messageBox.PrimaryButtonText = _localizationService.GetLocalizedString("MessageBox.Yes");
            messageBox.SecondaryButtonText = _localizationService.GetLocalizedString("MessageBox.No");
            messageBox.SecondaryButtonAppearance = global::Wpf.Ui.Controls.ControlAppearance.Info;

            return (MessageBoxResult.Cancel, MessageBoxResult.Yes, MessageBoxResult.No);
        }

        return (MessageBoxResult.None, MessageBoxResult.None, MessageBoxResult.None);
    }

    private Window? GetWindow() {
        if(AssociatedObject == null) {
            return default;
        }

        return AssociatedObject is Window window
            ? window
            : Window.GetWindow(AssociatedObject);
    }
}