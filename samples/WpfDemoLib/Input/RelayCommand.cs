using System.Windows.Input;

using WpfDemoLib.Input.Interfaces;

namespace WpfDemoLib.Input;

internal sealed class RelayCommand : IRelayCommand {
    private readonly Action _action;
    private readonly Func<bool>? _canExecute;

    public event EventHandler CanExecuteChanged {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public RelayCommand(Action action) {
        _action = action;
    }

    public RelayCommand(Action action, Func<bool> canExecute) {
        _action = action;
        _canExecute = canExecute;
    }

    bool ICommand.CanExecute(object parameter) {
        return CanExecute();
    }

    void ICommand.Execute(object parameter) {
        Execute();
    }
    
    public bool CanExecute() {
        return _canExecute == null || _canExecute();
    }

    public void Execute() {
        _action();
    }
}