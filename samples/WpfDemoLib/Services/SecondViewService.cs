using System.Windows;

using dosymep.WpfCore.SimpleServices;

using WpfDemoLib.Factories;
using WpfDemoLib.ViewModels;

namespace WpfDemoLib.Services;

public sealed class SecondViewService : WpfBaseService, ISecondViewService {
    private readonly SecondViewModel _viewModel;
    private readonly ISecondViewFactory _viewFactory;

    public SecondViewService(SecondViewModel viewModel, ISecondViewFactory viewFactory) {
        _viewModel = viewModel;
        _viewFactory = viewFactory;
    }

    public string? Result => _viewModel.Result;

    public Task ShowAsync(CancellationToken cancellationToken = default) {
        Window window = _viewFactory.Create();
        window.DataContext = _viewModel;

        SetAssociatedOwner(window);

        TaskCompletionSource<bool?> tcs = new();
        window.Closed += (_, _) => tcs.TrySetResult(true);

        window.Show();

        return tcs.Task;
    }

    public Task<bool?> ShowDialogAsync(CancellationToken cancellationToken = default) {
        Window window = _viewFactory.Create();
        window.DataContext = _viewModel;

        SetAssociatedOwner(window);

        TaskCompletionSource<bool?> tcs = new();
        window.Closed += (_, _) => tcs.TrySetResult(window.DialogResult);

        window.ShowDialog();

        return tcs.Task;
    }
}