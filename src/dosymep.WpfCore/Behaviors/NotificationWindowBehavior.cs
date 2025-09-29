using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

using Microsoft.Xaml.Behaviors;

namespace dosymep.WpfCore.Behaviors;

/// <summary>
/// Поведение, которое применяет анимацию скольжения появления окна.
/// </summary>
public class NotificationWindowBehavior : Behavior<Window> {
    public static readonly DependencyProperty WindowsProperty = DependencyProperty.Register(
        "Windows",
        typeof(ObservableCollection<Window>),
        typeof(NotificationWindowBehavior),
        new PropertyMetadata(default(ObservableCollection<Window>)));

    public static ObservableCollection<Window> GetWindows(DependencyObject d) {
        ObservableCollection<Window> windows = (ObservableCollection<Window>) d.GetValue(WindowsProperty);
        if(windows is null) {
            windows = [];
            d.SetValue(WindowsProperty, windows);
        }

        return windows;
    }

    /// <summary>
    /// Идентифицирует свойство зависимости Duration, которое определяет продолжительность анимации скольжения.
    /// </summary>
    public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
        nameof(Duration),
        typeof(TimeSpan),
        typeof(NotificationWindowBehavior),
        new PropertyMetadata(TimeSpan.FromSeconds(0.2)));

    /// <summary>
    /// Идентифицирует свойство зависимости BottomOffset, которое определяет расстояние от нижнего края экрана до нижнего края окна при анимации скольжения.
    /// </summary>
    public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
        nameof(Offset),
        typeof(double),
        typeof(NotificationWindowBehavior),
        new PropertyMetadata(10.0));

    /// <summary>
    /// Идентифицирует свойство зависимости StackWindows,
    /// где содержит в себе DependencyProperty, в котором хранится стек всех открыты уведомлений.
    /// </summary>
    public static readonly DependencyProperty StackWindowsCacheProperty = DependencyProperty.Register(
        nameof(StackWindowsCache),
        typeof(DependencyObject),
        typeof(NotificationWindowBehavior),
        new PropertyMetadata(default(DependencyObject)));

    private Screen? _screen;
    private Storyboard? _slideIn;

    /// <summary>
    /// Определяет продолжительность анимации в поведении скольжения.
    /// </summary>
    public TimeSpan Duration {
        get => (TimeSpan) GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    /// <summary>
    /// Определяет расстояние от нижнего края экрана до нижнего края окна при анимации скольжения.
    /// </summary>
    public double Offset {
        get => (double) GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    /// <summary>
    /// Содержит в себе DependencyProperty, в котором хранится стек всех открыты уведомлений.
    /// </summary>
    public DependencyObject StackWindowsCache {
        get => (DependencyObject) GetValue(StackWindowsCacheProperty);
        set => SetValue(StackWindowsCacheProperty, value);
    }

    /// <inheritdoc />
    protected override void OnAttached() {
        AssociatedObject.Loaded += OnWindowLoaded;
        AssociatedObject.Closing += OnWindowClosing;
    }

    /// <inheritdoc />
    protected override void OnDetaching() {
        AssociatedObject.Loaded -= OnWindowLoaded;
        AssociatedObject.Closing -= OnWindowClosing;
    }

    private void OnWindowLoaded(object sender, RoutedEventArgs e) {
        Window window = AssociatedObject;

        double height = GetWindows(StackWindowsCache)
            .Sum(item => item.ActualHeight + Offset);

        GetWindows(StackWindowsCache).Add(window);

        _screen = Screen.GetPrimaryScreen();

        window.Top = _screen.Top + _screen.Height - window.ActualHeight - height;

        double to = _screen.Left + _screen.Width - window.ActualWidth;
        double from = _screen.Left + _screen.Width + window.ActualWidth;

        DoubleAnimation animation =
            new() {To = to, From = from, Duration = Duration};

        _slideIn = new Storyboard {Children = [animation]};

        Storyboard.SetTarget(animation, window);
        Storyboard.SetTargetProperty(animation, new PropertyPath("(Window.Left)"));

        _slideIn.Begin(window);
    }

    private void OnWindowClosing(object sender, CancelEventArgs e) {
        AssociatedObject.Closing -= OnWindowClosing;

        e.Cancel = true;
        RemoveWindow(AssociatedObject);

        DispatcherTimer timer = new() {Interval = Duration};
        timer.Start();
        timer.Tick += delegate {
            timer.Stop();
            AssociatedObject.Close();
        };
    }

    private void RemoveWindow(Window removedWindow) {
        ObservableCollection<Window> stackWindows = GetWindows(StackWindowsCache);

        int index = stackWindows.IndexOf(removedWindow);
        if(index == -1) {
            return;
        }

        index++;
        if(index >= stackWindows.Count) {
            return;
        }

        for(int i = index; i < stackWindows.Count; i++) {
            Window stackWindow = stackWindows[i];

            double to = stackWindow.Top + removedWindow.Height + Offset;

            DoubleAnimation animation =
                new() {To = to, From = stackWindow.Top, Duration = Duration};

            Storyboard storyboard = new() {Children = [animation]};

            Storyboard.SetTarget(animation, stackWindow);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Window.Top)"));

            storyboard.Begin(stackWindow);
        }

        stackWindows.RemoveAt(index);
    }

    /// <summary>
    /// Запускает анимацию закрытия для связанного окна и закрывает его после завершения.
    /// </summary>
    public void CloseClicked() {
        Window window = AssociatedObject;

        double to = window.Left + window.ActualWidth + Offset;

        DoubleAnimation animation =
            new() {To = to, From = window.Left, Duration = Duration};

        Storyboard storyboard = new() {Children = [animation]};

        Storyboard.SetTarget(animation, window);
        Storyboard.SetTargetProperty(animation, new PropertyPath("(Window.Left)"));

        storyboard.Begin(window);
        storyboard.Completed += (s, e) => {
            window.Close();
            RemoveWindow(window);
        };
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