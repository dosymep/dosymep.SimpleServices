using System.Windows;
using System.Windows.Media;

namespace dosymep.WpfUI.Core.Windows;

/// <summary>
/// Предоставляет окно уведомления.
/// </summary>
public partial class WpfUINotificationWindow {
    /// <summary>
    /// Идентифицирует свойство зависимости Body,
    /// которое представляет основное содержимое или сообщение уведомления, отображаемого в уведомлении.
    /// </summary>
    public static readonly DependencyProperty BodyProperty = DependencyProperty.Register(
        nameof(Body),
        typeof(string),
        typeof(WpfUINotificationWindow),
        new PropertyMetadata(default(string)));

    /// <summary>
    /// Идентифицирует свойство зависимости Footer,
    /// которое представляет нижний текст или дополнительную информацию, отображаемую в уведомлении.
    /// </summary>
    public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(
        nameof(Footer),
        typeof(string),
        typeof(WpfUINotificationWindow),
        new PropertyMetadata(default(string)));

    /// <summary>
    /// Идентифицирует свойство зависимости Author,
    /// которое представляет автора уведомления, отображаемого в уведомлении.
    /// </summary>
    public static readonly DependencyProperty AuthorProperty = DependencyProperty.Register(
        nameof(Author),
        typeof(string),
        typeof(WpfUINotificationWindow),
        new PropertyMetadata(default(string)));

    /// <summary>
    /// Идентифицирует свойство зависимости ImageSource,
    /// которое определяет источник изображения, отображаемого в уведомлении.
    /// </summary>
    public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
        nameof(ImageSource),
        typeof(ImageSource),
        typeof(WpfUINotificationWindow),
        new PropertyMetadata(default(ImageSource)));

    /// <summary>
    /// Предоставляет конструктор окна уведомления.
    /// </summary>
    public WpfUINotificationWindow() {
        InitializeComponent();
    }

    /// <summary>
    /// Представляет основное содержимое или сообщение, отображаемое в окне уведомления.
    /// </summary>
    public string? Body {
        get => (string?) GetValue(BodyProperty);
        set => SetValue(BodyProperty, value);
    }

    /// <summary>
    /// Идентифицирует свойство зависимости Footer,
    /// которое представляет текст нижнего колонтитула, отображаемого в уведомлении.
    /// </summary>
    public string? Footer {
        get => (string) GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    /// <summary>
    /// Идентифицирует свойство зависимости Author,
    /// которое представляет автора уведомления, отображаемого в окне уведомления.
    /// </summary>
    public string? Author {
        get => (string?) GetValue(AuthorProperty);
        set => SetValue(AuthorProperty, value);
    }

    /// <summary>
    /// Идентифицирует свойство зависимости ImageSource,
    /// которое определяет источник изображения, отображаемого в уведомлении.
    /// </summary>
    public ImageSource? ImageSource {
        get => (ImageSource?) GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }
}