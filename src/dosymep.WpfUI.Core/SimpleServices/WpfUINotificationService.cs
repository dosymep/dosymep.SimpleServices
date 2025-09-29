using System.Windows;
using System.Windows.Media;

using dosymep.SimpleServices;
using dosymep.WpfCore.SimpleServices;
using dosymep.WpfUI.Core.Windows;

namespace dosymep.WpfUI.Core.SimpleServices;

public sealed class WpfUINotificationService : WpfBaseService, INotificationService {
    private readonly DependencyObject _stackWindowsCache = new();

    private readonly IHasTheme _theme;
    private readonly IHasLocalization _localization;

    public WpfUINotificationService(
        IHasTheme theme,
        IHasLocalization localization) {
        _theme = theme;
        _localization = localization;
    }

    /// <summary>
    /// Идентификатор приложения.
    /// </summary>
    public string ApplicationId { get; set; }

    /// <summary>
    /// Значение автора по умолчанию.
    /// </summary>
    public string DefaultAuthor { get; set; }

    /// <summary>
    /// Значение футера по умолчанию.
    /// </summary>
    public string DefaultFooter { get; set; }

    /// <summary>
    /// Значение изображения по умолчанию.
    /// </summary>
    public ImageSource DefaultImage { get; set; }

    /// <summary>
    /// Определяет на каком мониторе будет отображаться уведомление.
    /// </summary>
    public NotificationScreen NotificationScreen { get; set; }

    /// <summary>
    /// Позиция уведомления на экране.
    /// </summary>
    public NotificationPosition NotificationPosition { get; set; }

    /// <summary>
    /// Максимальное количество отображаемых уведомлений на экране.
    /// </summary>
    public int NotificationVisibleMaxCount { get; set; }

    public INotification CreateNotification(
        string title, string body, string? footer = null, string? author = null, ImageSource? imageSource = null) {
        return new WpfUINotificationWindow(_theme, _localization) {
            Title = title,
            Body = body,
            Footer = footer ?? DefaultFooter,
            Author = author ?? DefaultAuthor,
            ImageSource = imageSource ?? DefaultImage,
            StackWindowsCache = _stackWindowsCache
        };
    }

    public INotification CreateFatalNotification(
        string title, string body, string? author = null, ImageSource? imageSource = null) {
        return CreateNotification("Ошибка", body, title, author, imageSource);
    }

    public INotification CreateWarningNotification(
        string title, string body, string? author = null, ImageSource? imageSource = null) {
        return CreateNotification("Предупреждение", body, title, author, imageSource);
    }
}

public enum NotificationScreen {
    Primary,
    ApplicationWindow,
}

public enum NotificationPosition {
    TopRight,
    BottomRight,
}