using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

using dosymep.SimpleServices;
using dosymep.WpfCore.Behaviors;

using Wpf.Ui.Controls;

namespace dosymep.WpfUI.Core.Windows;

/// <summary>
/// Предоставляет окно уведомления.
/// </summary>
public partial class WpfUINotificationWindow : INotification {
    private TaskCompletionSource<bool?>? _tcs;
    
    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty WindowStackProperty = DependencyProperty.Register(
        nameof(WindowStack),
        typeof(ObservableCollection<Window>),
        typeof(WpfUINotificationWindow),
        new PropertyMetadata(default(ObservableCollection<Window>)));

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

    private readonly IHasTheme _theme;
    private readonly IHasLocalization _localization;

    /// <inheritdoc cref="IHasTheme.ThemeChanged"/>
    public event Action<UIThemes>? ThemeChanged;

    /// <inheritdoc cref="IHasLocalization.LanguageChanged"/>
    public event Action<CultureInfo>? LanguageChanged;

    /// <summary>
    /// Предоставляет конструктор окна уведомления.
    /// </summary>
    public WpfUINotificationWindow(
        IHasTheme theme,
        IHasLocalization localization) {
        _theme = theme;
        _localization = localization;

        _theme.ThemeChanged += _ => ThemeChanged?.Invoke(_);
        _localization.LanguageChanged += _ => LanguageChanged?.Invoke(_);

        theme.ThemeUpdaterService.SetTheme(theme.HostTheme, this);

        InitializeComponent();
    }
    
    /// <summary>
    /// 
    /// </summary>
    public ObservableCollection<Window> WindowStack {
        get => (ObservableCollection<Window>) GetValue(WindowStackProperty);
        set => SetValue(WindowStackProperty, value);
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

    #region INotification

    Task<bool?> INotification.ShowAsync() {
        return ((INotification) this).ShowAsync(5000);
    }

    Task<bool?> INotification.ShowAsync(int millisecond) {
        return ((INotification) this).ShowAsync(TimeSpan.FromMilliseconds(millisecond));
    }

    async Task<bool?> INotification.ShowAsync(TimeSpan interval) {
        _tcs = new TaskCompletionSource<bool?>();

        DispatcherTimer timer = new() {Interval = interval};
        timer.Start();

        timer.Tick += (_, _) => {
            if(!_tcs.Task.IsCompleted) {
                _tcs?.SetResult(null);
                _notificationWindowBehavior.OnAutoClosing();
            }
        };

        try {
            Show();
            return await _tcs.Task;
        } finally {
            timer.Stop();
        }
    }

    #endregion

    /// <inheritdoc />
    protected override void OnExtendsContentIntoTitleBarChanged(bool oldValue, bool newValue) {
        // do nothing
    }

    /// <inheritdoc />
    protected override void OnBackdropTypeChanged(WindowBackdropType oldValue, WindowBackdropType newValue) {
        // do nothing
    }

    /// <inheritdoc />
    protected override void OnCornerPreferenceChanged(WindowCornerPreference oldValue, WindowCornerPreference newValue) {
        // do nothing
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
        if(_tcs?.Task.IsCompleted == false) {
            _tcs?.SetResult(false);
            _notificationWindowBehavior.OnClosing();
        }
    }
}