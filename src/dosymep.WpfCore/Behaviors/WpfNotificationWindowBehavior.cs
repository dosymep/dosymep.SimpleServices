using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

using Microsoft.Xaml.Behaviors;

namespace dosymep.WpfCore.Behaviors;

/// <summary>
/// Поведение, которое применяет анимацию скольжения появления окна.
/// </summary>
public sealed class WpfNotificationWindowBehavior : Behavior<Window> {
    /// <summary>
    /// 
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
    /// 
    /// </summary>
    public ObservableCollection<Window> WindowStack {
        get => (ObservableCollection<Window>) GetValue(WindowStackProperty);
        set => SetValue(WindowStackProperty, value);
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
        WindowStack.Remove(AssociatedObject);

        if(_closing is not null) {
            _closing.Completed -= ClosingOnCompleted;
        }

        if(_autoClosing is not null) {
            _autoClosing.Completed -= ClosingOnCompleted;
        }

        foreach(Window window in WindowStack) {
            window.Top-= AssociatedObject.ActualHeight;
        }
    }

    /// <summary>
    /// Запускает анимацию показа окна.
    /// </summary>
    public void OnShowing() {
        WindowStack.Add(AssociatedObject);

        SetTopPosition(AssociatedObject);
        // SetBottomPosition(AssociatedObject);

        _showing?.Begin(AssociatedObject);
    }

    private void SetTopPosition(Window window) {
        var dpiScale = VisualTreeHelper.GetDpi(window);
        var mainScreen = Screen.FromHandle(new WindowInteropHelper(AssociatedObject).Handle);

        window.Top = WindowStack.Count * window.ActualHeight + WindowStack.Count * Offset;

        window.Left = mainScreen.WorkingArea.Left / dpiScale.DpiScaleX +
            mainScreen.WorkingArea.Width / dpiScale.DpiScaleX - window.ActualWidth;
    }

    private void SetBottomPosition(Window window) {
        var dpiScale = VisualTreeHelper.GetDpi(window);
        var screen = Screen.FromHandle(new WindowInteropHelper(AssociatedObject).Handle);

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
}