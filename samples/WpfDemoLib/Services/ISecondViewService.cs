using dosymep.SimpleServices;

namespace WpfDemoLib.Services;

public interface ISecondViewService: IAttachableService {
    string? Result { get; }
    
    Task ShowAsync(CancellationToken cancellationToken = default);
    Task<bool?> ShowDialogAsync(CancellationToken cancellationToken = default);
}