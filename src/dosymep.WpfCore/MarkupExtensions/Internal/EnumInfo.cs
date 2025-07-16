using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

using dosymep.SimpleServices;

namespace dosymep.WpfCore.MarkupExtensions.Internal;

internal class EnumInfo : INotifyPropertyChanged, IEquatable<EnumInfo> {
    private object? _displayName;

    public EnumInfo(object id, FieldInfo fieldInfo) {
        Id = id;
        FieldInfo = fieldInfo;
        DisplayName = FieldInfo.Name;
    }

    public object Id { get; }
    public FieldInfo FieldInfo { get; }

    public object? DisplayName {
        get => _displayName;
        set => SetField(ref _displayName, value);
    }

    public void UpdateDisplayName(ILocalizationService? localizationService) {
        if(localizationService == null) {
            DisplayName = FieldInfo.Name;
            return;
        }

        DisplayName = localizationService.GetLocalizedString(
                          $"{FieldInfo.FieldType.Name}.{FieldInfo.Name}")
                      ?? FieldInfo.Name;
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
        if(EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    #endregion

    #region IEquatable

    public bool Equals(EnumInfo? other) {
        if(other is null) {
            return false;
        }

        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object? obj) {
        if(obj is null) {
            return false;
        }

        if(ReferenceEquals(this, obj)) {
            return true;
        }

        return obj.GetType() == GetType() && Equals((EnumInfo) obj);
    }

    public override int GetHashCode() {
        return Id.GetHashCode();
    }

    public static bool operator ==(EnumInfo? left, EnumInfo? right) {
        return Equals(left, right);
    }

    public static bool operator !=(EnumInfo? left, EnumInfo? right) {
        return !Equals(left, right);
    }

    #endregion
}