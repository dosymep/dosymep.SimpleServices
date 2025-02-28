using WpfDemoLib.Input;
using WpfDemoLib.Input.Interfaces;

namespace WpfDemoLib.Factories;

public sealed class RelayCommandFactory : ICommandFactory {
    /// <inheritdoc />
    public IRelayCommand Create(Action action) {
        return new RelayCommand(action);
    }

    /// <inheritdoc />
    public IRelayCommand Create(Action action, Func<bool> canExecute) {
        return new RelayCommand(action, canExecute);
    }

    /// <inheritdoc />
    public IRelayCommand<TInput> Create<TInput>(Action<TInput> action) {
        return new RelayCommand<TInput>(action);
    }

    /// <inheritdoc />
    public IRelayCommand<TInput> Create<TInput>(Action<TInput> action, Func<TInput, bool> canExecute) {
        return new RelayCommand<TInput>(action, canExecute);
    }

    /// <inheritdoc />
    public IAsyncRelayCommand CreateAsync(Func<Task> action) {
        return new AsyncRelayCommand(action);
    }

    /// <inheritdoc />
    public IAsyncRelayCommand CreateAsync(Func<Task> action, Func<bool> canExecute) {
        return new AsyncRelayCommand(action, canExecute);
    }

    /// <inheritdoc />
    public IAsyncRelayCommand<TInput> CreateAsync<TInput>(Func<TInput, Task> action) {
        return new AsyncRelayCommand<TInput>(action);
    }

    /// <inheritdoc />
    public IAsyncRelayCommand<TInput> CreateAsync<TInput>(Func<TInput, Task> action, Func<TInput, bool> canExecute) {
        return new AsyncRelayCommand<TInput>(action, canExecute);
    }
}