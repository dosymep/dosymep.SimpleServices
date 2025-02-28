using WpfDemoLib.Input.Interfaces;

namespace WpfDemoLib.Factories;

public interface ICommandFactory {
    IRelayCommand Create(Action action);
    IRelayCommand Create(Action action, Func<bool> canExecute);

    IRelayCommand<TInput> Create<TInput>(Action<TInput> action);
    IRelayCommand<TInput> Create<TInput>(Action<TInput> action, Func<TInput, bool> canExecute);

    IAsyncRelayCommand CreateAsync(Func<Task> action);
    IAsyncRelayCommand CreateAsync(Func<Task> action, Func<bool> canExecute);

    IAsyncRelayCommand<TInput> CreateAsync<TInput>(Func<TInput, Task> action);
    IAsyncRelayCommand<TInput> CreateAsync<TInput>(Func<TInput, Task> action, Func<TInput, bool> canExecute);
}