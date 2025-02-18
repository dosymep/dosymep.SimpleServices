using System.Windows.Input;

using WpfDemoLib.Input.Interfaces;
using WpfDemoLib.ViewModels;

namespace WpfDemoLib.Input;

internal sealed class AsyncRelayCommand<T> : ObservableObject, IAsyncRelayCommand<T> {
    private readonly Func<T, Task> _action;
    private readonly Func<T, bool>? _canExecute;

    private bool _isRunning;

    public event EventHandler CanExecuteChanged {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public AsyncRelayCommand(Func<T, Task> action) {
        _action = action;
    }

    public AsyncRelayCommand(Func<T, Task> action, Func<T, bool> canExecute) {
        _action = action;
        _canExecute = canExecute;
    }

    public bool IsRunning {
        get => _isRunning;
        private set => this.RaiseAndSetIfChanged(ref _isRunning, value);
    }

    bool ICommand.CanExecute(object parameter) {
        return CanExecute((T) parameter);
    }

    async void ICommand.Execute(object parameter) {
        await ExecuteAsync((T) parameter);
    }

    public bool CanExecute(T parameter) {
        return _canExecute == null || _canExecute(parameter);
    }

    public async void Execute(T parameter) {
        await ExecuteAsync(parameter);
    }

    public Task ExecuteAsync(T parameter) {
        IsRunning = true;
        try {
            return _action(parameter);
        } finally {
            IsRunning = false;
        }
    }
}