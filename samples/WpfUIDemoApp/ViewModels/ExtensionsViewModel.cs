using System.Collections.ObjectModel;

namespace WpfUIDemoApp.ViewModels;

public sealed class ExtensionsViewModel {
    public ExtensionsViewModel() {
        Extensions = new() {
            new ExtensionViewModel() {Category = "1", Name = "1", Description = "1"},
            new ExtensionViewModel() {Category = "1", Name = "1", Description = "1"},
            new ExtensionViewModel() {Category = "2", Name = "2", Description = "2"},
            new ExtensionViewModel() {Category = "2", Name = "2", Description = "2"},
        };
    }
    public ObservableCollection<ExtensionViewModel> Extensions { get; }
}

public sealed class ExtensionViewModel {
    public string? Category { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}