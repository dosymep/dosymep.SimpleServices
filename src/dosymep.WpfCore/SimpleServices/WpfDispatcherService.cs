using System.Windows.Threading;

using dosymep.SimpleServices;

namespace dosymep.WpfCore.SimpleServices;

/// <summary>
/// Класс сервиса диспетчера окна.
/// </summary>
public sealed class WpfDispatcherService : WpfBaseService, IDispatcherService {
    private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

    /// <inheritdoc />
    public void Invoke(Action action) {
        _dispatcher.Invoke(action);
    }

    /// <inheritdoc />
    public Task BeginInvoke(Action action) {
        return _dispatcher.BeginInvoke(action).Task;
    }
}