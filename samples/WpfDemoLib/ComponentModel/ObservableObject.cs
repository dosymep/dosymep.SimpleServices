using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfDemoLib.ComponentModel;

public abstract class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging {
    public event PropertyChangedEventHandler? PropertyChanged;
    public event PropertyChangingEventHandler? PropertyChanging;

    protected bool RaiseAndSetIfChanged<TRet>(
        ref TRet backingField, TRet newValue, [CallerMemberName] string? propertyName = default) {
        if(EqualityComparer<TRet>.Default.Equals(backingField, newValue)) {
            return false;
        }

        RaisePropertyChanging(propertyName);

        backingField = newValue;

        RaisePropertyChanged(propertyName);

        return true;
    }

    protected void RaisePropertyChanged(string? propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    protected void RaisePropertyChanging(string? propertyName) {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }
}