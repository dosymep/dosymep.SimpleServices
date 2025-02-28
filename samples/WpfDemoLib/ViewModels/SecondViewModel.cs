using WpfDemoLib.ComponentModel;

namespace WpfDemoLib.ViewModels;

public sealed class SecondViewModel : ObservableObject {
    private string? _result;

    public string? Result {
        get => _result;
        set => this.RaiseAndSetIfChanged(ref _result, value);
    }
}