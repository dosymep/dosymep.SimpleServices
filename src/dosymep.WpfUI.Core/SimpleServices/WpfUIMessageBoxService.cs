using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

using dosymep.SimpleServices;
using dosymep.WpfCore.SimpleServices;

namespace dosymep.WpfUI.Core.SimpleServices;

/// <summary>
/// Класс сервиса окна сообщений. 
/// </summary>
public sealed class WpfUIMessageBoxService : WpfUIBaseService, IMessageBoxService {
    private static readonly string _messageBoxLanguage =
        "pack://application:,,,/dosymep.WpfUI.Core;component/assets/localizations/language.xaml";

    private static readonly string _messageBoxContentTemplate =
        "pack://application:,,,/dosymep.WpfUI.Core;component/Views/MessageBoxContentTemplate.xaml";

    private readonly IHasTheme _theme;
    private readonly IHasLocalization _localization;
    private readonly ILocalizationService _internalLocalization;

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public WpfUIMessageBoxService(
        IHasTheme theme,
        IHasLocalization localization) {
        _theme = theme;
        _localization = localization;
        _internalLocalization = new WpfLocalizationService(_messageBoxLanguage, _localization.HostLanguage);
    }

    /// <inheritdoc />
    public MessageBoxResult Show(string messageBoxText,
        string caption,
        MessageBoxButton button,
        MessageBoxImage icon,
        MessageBoxResult defaultResult) {
        
        _internalLocalization.SetLocalization(_localization.HostLanguage);

        Wpf.Ui.Controls.MessageBox messageBox = new() {
            MinWidth = 350,
            Title = caption,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        Window? window = GetWindow();
        if(window?.IsVisible == true) {
            messageBox.Owner = window;
        } else {
            WindowInteropHelper helper = new(messageBox);
            helper.Owner = Process.GetCurrentProcess().MainWindowHandle;
        }
        
        messageBox.Content = new {ImageSource = GetImageSource(icon), MessageBoxText = messageBoxText};

        messageBox.Resources.MergedDictionaries.Add(
            new ResourceDictionary() {Source = new Uri(_messageBoxContentTemplate, UriKind.Absolute)});

        messageBox.ContentTemplate = messageBox.FindResource("MessageBoxContentTemplate") as DataTemplate;
        
        void UpdateTheme(UIThemes uiTheme) {
            _theme.ThemeUpdaterService.SetTheme(messageBox, uiTheme);
        }

        try {
            _theme.ThemeChanged += UpdateTheme;
            _theme.ThemeUpdaterService.SetTheme(messageBox, _theme.HostTheme);

            (MessageBoxResult closeResult,
                MessageBoxResult primaryResult,
                MessageBoxResult secondaryResult) = ShowMessageBox(button, messageBox);

            // magic hack :D
            return messageBox.ShowDialogAsync()
                .ConfigureAwait(false).GetAwaiter().GetResult() switch {
                Wpf.Ui.Controls.MessageBoxResult.None => closeResult,
                Wpf.Ui.Controls.MessageBoxResult.Primary => primaryResult,
                Wpf.Ui.Controls.MessageBoxResult.Secondary => secondaryResult,
                _ => MessageBoxResult.None
            };
        } finally {
            _theme.ThemeChanged -= UpdateTheme;
        }
    }

    private BitmapFrame GetImageSource(MessageBoxImage icon) {
        string symbol = icon switch {
            MessageBoxImage.None =>
                "pack://application:,,,/dosymep.WpfUI.Core;component/assets/images/icons8-empty-26.png",
            MessageBoxImage.Hand =>
                "pack://application:,,,/dosymep.WpfUI.Core;component/assets/images/icons8-cross-26.png",
            MessageBoxImage.Question =>
                "pack://application:,,,/dosymep.WpfUI.Core;component/assets/images/icons8-question-26.png",
            MessageBoxImage.Exclamation =>
                "pack://application:,,,/dosymep.WpfUI.Core;component/assets/images/icons8-error-26.png",
            MessageBoxImage.Asterisk =>
                "pack://application:,,,/dosymep.WpfUI.Core;component/assets/images/icons8-exclamation-26.png",
            _ => throw new ArgumentOutOfRangeException(nameof(icon), icon, default)
        };

        return BitmapFrame.Create(new Uri(symbol, UriKind.Absolute));
    }

    private (MessageBoxResult closeResult,
        MessageBoxResult primaryResult,
        MessageBoxResult secondaryResult)
        ShowMessageBox(MessageBoxButton button, Wpf.Ui.Controls.MessageBox messageBox) {
        if(button == MessageBoxButton.OK) {
            messageBox.IsPrimaryButtonEnabled = false;
            messageBox.IsSecondaryButtonEnabled = false;
            messageBox.CloseButtonText = GetLocalization("MessageBox.Ok");
            messageBox.CloseButtonAppearance = Wpf.Ui.Controls.ControlAppearance.Info;

            return (MessageBoxResult.OK, MessageBoxResult.None, MessageBoxResult.None);
        } else if(button == MessageBoxButton.OKCancel) {
            messageBox.IsPrimaryButtonEnabled = true;
            messageBox.IsSecondaryButtonEnabled = false;
            messageBox.CloseButtonText = GetLocalization("MessageBox.Cancel");
            messageBox.PrimaryButtonText = GetLocalization("MessageBox.Ok");
            messageBox.PrimaryButtonAppearance = Wpf.Ui.Controls.ControlAppearance.Info;

            return (MessageBoxResult.Cancel, MessageBoxResult.OK, MessageBoxResult.None);
        } else if(button == MessageBoxButton.YesNo) {
            messageBox.IsPrimaryButtonEnabled = true;
            messageBox.IsSecondaryButtonEnabled = false;
            messageBox.CloseButtonText = GetLocalization("MessageBox.No");
            messageBox.PrimaryButtonText = GetLocalization("MessageBox.Yes");
            messageBox.PrimaryButtonAppearance = Wpf.Ui.Controls.ControlAppearance.Info;

            return (MessageBoxResult.No, MessageBoxResult.Yes, MessageBoxResult.None);
        } else if(button == MessageBoxButton.YesNoCancel) {
            messageBox.IsPrimaryButtonEnabled = true;
            messageBox.IsSecondaryButtonEnabled = true;
            messageBox.CloseButtonText = GetLocalization("MessageBox.Cancel");
            messageBox.PrimaryButtonText = GetLocalization("MessageBox.Yes");
            messageBox.SecondaryButtonText = GetLocalization("MessageBox.No");
            messageBox.SecondaryButtonAppearance = Wpf.Ui.Controls.ControlAppearance.Info;

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

    private string GetLocalization(string localizationName) {
        return _localization.LocalizationService.GetLocalizedString(localizationName)
               ?? _internalLocalization.GetLocalizedString(localizationName);
    }
}