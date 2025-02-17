using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace dosymep.WpfCore.Converters;

/// <summary>
/// Конвертер bool в object.
/// </summary>
[ValueConversion(typeof(bool), typeof(object))]
public sealed class BoolToObjectConverter : IValueConverter {
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public BoolToObjectConverter() { }

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="trueValue">Значение, которое будет отображаться если true.</param>
    /// <param name="falseValue">Значение, которое будет отображаться если false.</param>
    public BoolToObjectConverter(object trueValue, object falseValue)
        : this(trueValue, falseValue, default) {
    }

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="trueValue">Значение, которое будет отображаться если true.</param>
    /// <param name="falseValue">Значение, которое будет отображаться если false.</param>
    /// <param name="defaultValue">Значение, которое будет отображаться если null.</param>
    public BoolToObjectConverter(object trueValue, object falseValue, object? defaultValue) {
        TrueValue = trueValue;
        FalseValue = falseValue;
        DefaultValue = defaultValue;
    }

    /// <summary>
    /// Вывод, когда значение bool == True.
    /// </summary>
    public object? TrueValue { get; set; }

    /// <summary>
    /// Вывод, когда значение bool == False.
    /// </summary>
    public object? FalseValue { get; set; }

    /// <summary>
    /// Вывод, когда значение bool == default.
    /// </summary>
    public object? DefaultValue { get; set; } = default;

    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        bool? boolValue = (bool?) value;

        if(boolValue is null) {
            return DefaultValue;
        }

        return boolValue == true ? TrueValue : FalseValue;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if(Equals(value, TrueValue)) {
            return true;
        } else if(Equals(value, FalseValue)) {
            return false;
        }

        return default;
    }
}