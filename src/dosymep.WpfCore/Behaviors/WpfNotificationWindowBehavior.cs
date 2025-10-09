using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
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
    /// Идентифицирует свойство зависимости BottomOffset,
    /// которое определяет расстояние от нижнего края экрана до нижнего края окна при анимации скольжения.
    /// </summary>
    public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
        nameof(Offset),
        typeof(double),
        typeof(WpfNotificationWindowBehavior),
        new PropertyMetadata(10.0));

    private Screen? _screen;

    /// <summary>
    /// Определяет расстояние от нижнего края экрана до нижнего края окна при анимации скольжения.
    /// </summary>
    public double Offset {
        get => (double) GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    private bool _isClosed;

    private Storyboard? _showing;
    private Storyboard? _closing;
    private Storyboard? _autoClosing;
    private Storyboard? _positioning;

    /// <inheritdoc />
    protected override void OnAttached() {
        AssociatedObject.Loaded += OnWindowLoaded;

        _screen = Screen.GetPrimaryScreen();

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

    /// <inheritdoc />
    protected override void OnDetaching() {
        AssociatedObject.Loaded -= OnWindowLoaded;
    }

    private void OnWindowLoaded(object sender, RoutedEventArgs e) {
        OnShowing();
    }

    private void ClosingOnCompleted(object sender, EventArgs e) {
        AssociatedObject.Close();

        if(_closing is not null) {
            _closing.Completed -= ClosingOnCompleted;
        }

        if(_autoClosing is not null) {
            _autoClosing.Completed -= ClosingOnCompleted;
        }
    }

    /// <summary>
    /// Запускает анимацию показа окна.
    /// </summary>
    public void OnShowing() {
        AssociatedObject.Top = _screen.Top + _screen.Height - AssociatedObject.ActualHeight;
        AssociatedObject.Left = _screen.Left + _screen.Width - AssociatedObject.ActualWidth;

        _showing?.Begin(AssociatedObject);
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

internal class Screen {
    public double Dpi { get; set; }

    public double Top { get; set; }
    public double Left { get; set; }

    public double Width { get; set; }
    public double Height { get; set; }

    public static Screen GetPrimaryScreen() {
        return new Screen() {
            Top = 0,
            Left = 0,
            Width = SystemParameters.FullPrimaryScreenWidth,
            Height = SystemParameters.FullPrimaryScreenHeight
        };
    }

    public static Screen GetScreen(Window window) {
        return new Screen();
    }
}