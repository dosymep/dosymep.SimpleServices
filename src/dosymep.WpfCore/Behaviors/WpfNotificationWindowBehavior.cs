using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

using dosymep.WpfCore.SimpleServices;

using Microsoft.Xaml.Behaviors;

namespace dosymep.WpfCore.Behaviors;

/// <summary>
/// Поведение, которое применяет анимацию скольжения появления окна.
/// </summary>
public sealed class WpfNotificationWindowBehavior : Behavior<Window> {
    /// <summary>
    /// Определяет, на каком мониторе будет отображаться уведомление.
    /// </summary>
    public static readonly DependencyProperty NotificationScreenProperty = DependencyProperty.Register(
        nameof(NotificationScreen),
        typeof(NotificationScreen),
        typeof(WpfNotificationWindowBehavior),
        new PropertyMetadata(default(NotificationScreen)));

    /// <summary>
    /// Позиция уведомления на экране.
    /// </summary>
    public static readonly DependencyProperty NotificationPositionProperty = DependencyProperty.Register(
        nameof(NotificationPosition),
        typeof(NotificationPosition),
        typeof(WpfNotificationWindowBehavior),
        new PropertyMetadata(default(NotificationPosition)));

    /// <summary>
    /// Список открытых окон уведомлений.
    /// </summary>
    public static readonly DependencyProperty WindowStackProperty = DependencyProperty.Register(
        nameof(WindowStack),
        typeof(ObservableCollection<Window>),
        typeof(WpfNotificationWindowBehavior),
        new PropertyMetadata(default(ObservableCollection<Window>)));

    /// <summary>
    /// Идентифицирует свойство зависимости BottomOffset,
    /// которое определяет расстояние от нижнего края экрана до нижнего края окна при анимации скольжения.
    /// </summary>
    public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
        nameof(Offset),
        typeof(double),
        typeof(WpfNotificationWindowBehavior),
        new PropertyMetadata(10.0));

    private bool _isClosed;

    private Storyboard? _showing;
    private Storyboard? _closing;
    private Storyboard? _autoClosing;
    private Storyboard? _positioning;

    /// <inheritdoc />
    protected override void OnAttached() {
        AssociatedObject.Loaded += OnWindowLoaded;
        AssociatedObject.RenderTransform = new TranslateTransform();

        ResourceDictionary resources = new() {
            Source = new Uri("pack://application:,,,/dosymep.WpfCore;component/Animations/NotificationAnimations.xaml")
        };

        _showing = (Storyboard) resources["Showing"];
        Storyboard.SetTarget(_showing, AssociatedObject);

        _closing = (Storyboard) resources["Closing"];
        Storyboard.SetTarget(_closing, AssociatedObject);

        _autoClosing = (Storyboard) resources["AutoClosing"];
        Storyboard.SetTarget(_autoClosing, AssociatedObject);

        _positioning = (Storyboard) resources["Positioning"];
        Storyboard.SetTarget(_positioning, AssociatedObject);

        _closing.Completed += ClosingOnCompleted;
        _autoClosing.Completed += ClosingOnCompleted;
    }

    /// <summary>
    /// Список открытых окон уведомлений.
    /// </summary>
    public ObservableCollection<Window> WindowStack {
        get => (ObservableCollection<Window>) GetValue(WindowStackProperty);
        set => SetValue(WindowStackProperty, value);
    }

    /// <summary>
    /// Определяет, на каком мониторе будет отображаться уведомление.
    /// </summary>
    public NotificationScreen NotificationScreen {
        get => (NotificationScreen) GetValue(NotificationScreenProperty);
        set => SetValue(NotificationScreenProperty, value);
    }

    /// <summary>
    /// Позиция уведомления на экране.
    /// </summary>
    public NotificationPosition NotificationPosition {
        get => (NotificationPosition) GetValue(NotificationPositionProperty);
        set => SetValue(NotificationPositionProperty, value);
    }

    /// <summary>
    /// Определяет расстояние от нижнего края экрана до нижнего края окна при анимации скольжения.
    /// </summary>
    public double Offset {
        get => (double) GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    /// <inheritdoc />
    protected override void OnDetaching() {
        AssociatedObject.Loaded -= OnWindowLoaded;
    }

    private void OnWindowLoaded(object sender, RoutedEventArgs e) {
        OnShowing();
    }

    private void ClosingOnCompleted(object sender, EventArgs e) {
        AssociatedObject.Close();

        int windowIndex = WindowStack.IndexOf(AssociatedObject);
        WindowStack.RemoveAt(windowIndex);

        if(_closing is not null) {
            _closing.Completed -= ClosingOnCompleted;
        }

        if(_autoClosing is not null) {
            _autoClosing.Completed -= ClosingOnCompleted;
        }


        int sign = GetSign();
        for(int index = 0; index < WindowStack.Count; index++) {
            Window window = WindowStack[index];
            if(index > (windowIndex - 1)) {
                window.Top += sign * AssociatedObject.ActualHeight + sign * Offset;
            }
        }
    }

    /// <summary>
    /// Запускает анимацию показа окна.
    /// </summary>
    public void OnShowing() {
        WindowStack.Add(AssociatedObject);

        if(NotificationPosition == NotificationPosition.TopRight) {
            SetTopPosition(AssociatedObject);
        } else if(NotificationPosition == NotificationPosition.BottomRight) {
            SetBottomPosition(AssociatedObject);
        } else {
            throw new NotSupportedException($"Position {NotificationPosition} is not supported.");
        }

        _showing?.Begin(AssociatedObject);
    }

    private void SetBottomPosition(Window window) {
        var screen = GetMainScreen();
        var dpiScale = VisualTreeHelper.GetDpi(window);

        window.Top = screen.WorkingArea.Top / dpiScale.DpiScaleY +
                     screen.WorkingArea.Height / dpiScale.DpiScaleY -
                     WindowStack.Count * window.ActualHeight - WindowStack.Count * Offset;

        window.Left = screen.WorkingArea.Left / dpiScale.DpiScaleX +
            screen.WorkingArea.Width / dpiScale.DpiScaleX - window.ActualWidth;
    }

    /// <summary>
    /// Запускает анимацию закрытия окна.
    /// </summary>
    public void OnClosing() {
        if(_closing is not null && !_isClosed) {
            _closing.Begin(AssociatedObject);
        }

        _isClosed = true;
    }

    /// <summary>
    /// Запускает анимацию закрытия окна по времени.
    /// </summary>
    public void OnAutoClosing() {
        if(_autoClosing is not null && !_isClosed) {
            _autoClosing.Begin(AssociatedObject);
        }

        _isClosed = true;
    }

    private int GetSign() {
        if(NotificationPosition == NotificationPosition.TopRight) {
            return -1;
        }

        if(NotificationPosition == NotificationPosition.BottomRight) {
            return 1;
        }

        throw new NotSupportedException($"Position {NotificationPosition} is not supported.");
    }

    private Screen GetMainScreen() {
        if(NotificationScreen == NotificationScreen.Primary) {
            return Screen.PrimaryScreen;
        }

        if(NotificationScreen == NotificationScreen.ApplicationWindow) {
            return Screen.FromHandle(new WindowInteropHelper(AssociatedObject).Handle);
        }
       
        throw new NotSupportedException($"Screen {NotificationScreen} is not supported.");
    }

    private void SetTopPosition(Window window) {
        var screen = GetMainScreen();
        var dpiScale = VisualTreeHelper.GetDpi(window);

        int startBarHeight = screen.Bounds.Height - screen.WorkingArea.Height;

        window.Top = -startBarHeight + WindowStack.Count * window.ActualHeight + WindowStack.Count * Offset;

        window.Left = screen.WorkingArea.Left / dpiScale.DpiScaleX +
            screen.WorkingArea.Width / dpiScale.DpiScaleX - window.ActualWidth;
    }
}