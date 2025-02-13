using System.Windows;

namespace dosymep.Wpf.Core.MarkupExtensions.Internal;

internal class MarkupValueObject : DependencyObject {
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value), typeof(object), typeof(MarkupValueObject), new PropertyMetadata(default(object)));

    public object? Value {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
}