using System.ComponentModel;
using System.Windows.Input;

namespace WpfDemoLib.Input.Interfaces;

public interface IAsyncRelayCommand : IRelayCommand, INotifyPropertyChanged, INotifyPropertyChanging {
    bool IsRunning { get; }
}