namespace WpfDemoLib.Input.Interfaces;

public interface IRelayCommand<T> : IRelayCommand {
    bool CanExecute(T parameter);
    void Execute(T parameter);
}