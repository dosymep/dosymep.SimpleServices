using System.Windows;

namespace dosymep.Wpf.Core.MarkupExtensions.Internal;

internal class MarkupDisplayObject : DependencyObject {
    private readonly Func<string> _updateDisplayName;

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value), typeof(object), typeof(MarkupDisplayObject), new PropertyMetadata(default(object)));

    public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(
        nameof(DisplayName), typeof(object), typeof(MarkupDisplayObject), new PropertyMetadata(default(object)));

    public MarkupDisplayObject(Func<string> updateDisplayName) {
        _updateDisplayName = updateDisplayName;
    }

    public object Value {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public object DisplayName {
        get => GetValue(DisplayNameProperty);
        set => SetValue(DisplayNameProperty, value);
    }

    public void UpdateDisplayName() {
        DisplayName = _updateDisplayName();
    }
}