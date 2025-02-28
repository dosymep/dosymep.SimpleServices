namespace WpfDemoLib.Input.Interfaces;

public interface IAsyncRelayCommand<T> : IAsyncRelayCommand, IRelayCommand<T> {
    Task ExecuteAsync(T parameter);
}