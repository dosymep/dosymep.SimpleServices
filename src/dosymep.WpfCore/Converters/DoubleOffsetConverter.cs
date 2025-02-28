using System.Globalization;
using System.Windows.Data;

namespace dosymep.WpfCore.Converters;

/// <summary>
/// Добавляет указаное значение.
/// </summary>
[ValueConversion(typeof(double), typeof(double))]
public sealed class DoubleOffsetConverter : IValueConverter {
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public DoubleOffsetConverter() { }
    
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="offsetValue">Значение на которое изменяется.</param>
    public DoubleOffsetConverter(double offsetValue) => OffsetValue = offsetValue;

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="inverted">Признак инвертированости.</param>
    /// <param name="offsetValue">Значение на которое изменяется.</param>
    public DoubleOffsetConverter(bool inverted, double offsetValue) {
        Inverted = inverted;
        OffsetValue = offsetValue;
    }

    /// <summary>
    /// Признак инвертированости.
    /// </summary>
    public bool Inverted { get; set; }

    /// <summary>
    /// Значение на которое изменяется.
    /// </summary>
    public double OffsetValue { get; set; } = 1.0;

    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return !Inverted ? Add(value) : Subtract(value);
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return Inverted ? Add(value) : Subtract(value);
    }

    private object? Add(object? value) {
        return value is double doubleValue ? doubleValue + OffsetValue : value;
    }

    private object? Subtract(object? value) {
        return value is double doubleValue ? doubleValue - OffsetValue : value;
    }
}