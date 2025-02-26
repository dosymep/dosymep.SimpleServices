using System.ComponentModel;
using System.Windows.Input;

using WpfDemoLib.ComponentModel;
using WpfDemoLib.Input.Interfaces;
using WpfDemoLib.ViewModels;

namespace WpfDemoLib.Input;

internal sealed class AsyncRelayCommand : ObservableObject, IAsyncRelayCommand {
    private readonly Func<Task> _action;
    private readonly Func<bool>? _canExecute;
   
    private bool _isRunning;

    public event EventHandler CanExecuteChanged {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public AsyncRelayCommand(Func<Task> action) {
        _action = action;
    }

    public AsyncRelayCommand(Func<Task> action, Func<bool> canExecute) {
        _action = action;
        _canExecute = canExecute;
    }
    
    public bool IsRunning {
        get => _isRunning;
        private set => this.RaiseAndSetIfChanged(ref _isRunning, value);
    }

    bool ICommand.CanExecute(object parameter) {
        return CanExecute();
    }

    async void ICommand.Execute(object parameter) {
        await Execute();
    }
    
    public bool CanExecute() {
        return _canExecute == null || _canExecute();
    }

    public Task Execute() {
        IsRunning = true;
        try {
            return _action();
        } finally {
            IsRunning = false;
        }
    }
}