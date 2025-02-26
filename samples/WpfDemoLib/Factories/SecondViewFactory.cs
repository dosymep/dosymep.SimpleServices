using System.Windows;

namespace WpfDemoLib.Factories;

public sealed class SecondViewFactory<TWindow> : ISecondViewFactory where TWindow : Window {
    private readonly Func<TWindow> _windowFactory;

    public SecondViewFactory(Func<TWindow> windowFactory) {
        _windowFactory = windowFactory;
    }

    public Window Create() {
        return _windowFactory();
    }
}