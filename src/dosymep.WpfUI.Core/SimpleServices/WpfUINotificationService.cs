using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

using dosymep.SimpleServices;
using dosymep.WpfCore.SimpleServices;
using dosymep.WpfUI.Core.Windows;

namespace dosymep.WpfUI.Core.SimpleServices;

/// <summary>
/// Класс сервиса уведомлений
/// </summary>
public sealed class WpfUINotificationService : WpfBaseService, INotificationService {
    private readonly IHasTheme _theme;
    private readonly IHasLocalization _localization;

    /// <summary>
    /// Создает экземпляр сервиса уведомлений.
    /// </summary>
    public WpfUINotificationService(
        IHasTheme theme,
        IHasLocalization localization) {
        _theme = theme;
        _localization = localization;
    }

    /// <summary>
    /// Идентификатор приложения.
    /// </summary>
    public string? ApplicationId { get; set; }

    /// <summary>
    /// Значение автора по умолчанию.
    /// </summary>
    public string? DefaultAuthor { get; set; }

    /// <summary>
    /// Значение футера по умолчанию.
    /// </summary>
    public string? DefaultFooter { get; set; }

    /// <summary>
    /// Значение изображения по умолчанию.
    /// </summary>
    public ImageSource? DefaultImage { get; set; }

    /// <summary>
    /// Определяет, на каком мониторе будет отображаться уведомление.
    /// </summary>
    public NotificationScreen NotificationScreen { get; set; } = NotificationScreen.Primary;

    /// <summary>
    /// Позиция уведомления на экране.
    /// </summary>
    public NotificationPosition NotificationPosition { get; set; } = NotificationPosition.BottomRight;

    /// <summary>
    /// Максимальное количество отображаемых уведомлений на экране.
    /// </summary>
    public int NotificationVisibleMaxCount { get; set; } = 1;

    private ObservableCollection<Window> _windowStack = [];

    /// <inheritdoc />
    public INotification CreateNotification(
        string title, string body, string? footer = null, string? author = null, ImageSource? imageSource = null) {
        WpfUINotificationWindow window = new(_theme, _localization) {
            Title = title,
            Body = body,
            Footer = footer ?? DefaultFooter,
            Author = author ?? DefaultAuthor,
            ImageSource = imageSource ?? DefaultImage,
            WindowStack = _windowStack
        };
        
        SetAssociatedOwner(window);
        return window;
    }

    /// <inheritdoc />
    public INotification CreateFatalNotification(
        string title, string body, string? author = null, ImageSource? imageSource = null) {
        return CreateNotification("Ошибка", body, title, author, imageSource);
    }

    /// <inheritdoc />
    public INotification CreateWarningNotification(
        string title, string body, string? author = null, ImageSource? imageSource = null) {
        return CreateNotification("Предупреждение", body, title, author, imageSource);
    }
}

/// <summary>
/// Перечисление, представляющее различные экраны для отображения уведомлений.
/// </summary>
public enum NotificationScreen {
    /// <summary>
    /// Основной экран.
    /// </summary>
    Primary,

    /// <summary>
    /// Экран, где расположено окно.
    /// </summary>
    ApplicationWindow,
}

/// <summary>
/// Определяет возможные позиции отображения уведомлений.
/// </summary>
public enum NotificationPosition {
    /// <summary>
    /// Сверху справа.
    /// </summary>
    TopRight,
    
    /// <summary>
    /// Снизу справа.
    /// </summary>
    BottomRight,
}