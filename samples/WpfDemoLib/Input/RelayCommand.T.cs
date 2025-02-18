using System.Windows.Input;

using WpfDemoLib.Input.Interfaces;

namespace WpfDemoLib.Input;

internal sealed class RelayCommand<T> : IRelayCommand<T> {
    private readonly Action<T> _action;
    private readonly Func<T, bool>? _canExecute;

    public event EventHandler CanExecuteChanged {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public RelayCommand(Action<T> action) {
        _action = action;
    }

    public RelayCommand(Action<T> action, Func<T, bool> canExecute) {
        _action = action;
        _canExecute = canExecute;
    }

    bool ICommand.CanExecute(object parameter) {
        return CanExecute((T) parameter);
    }

    void ICommand.Execute(object parameter) {
        Execute((T) parameter);
    }

    public bool CanExecute(T parameter) {
        return _canExecute == null || _canExecute(parameter);
    }

    public void Execute(T parameter) {
        _action(parameter);
    }
}