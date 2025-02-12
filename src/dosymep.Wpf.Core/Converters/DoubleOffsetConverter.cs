using System.Globalization;
using System.Windows.Data;

namespace dosymep.Wpf.Core.Converters;

/// <summary>
/// Добавляет указаное значение.
/// </summary>
[ValueConversion(typeof(double), typeof(double))]
internal sealed class DoubleOffsetConverter : IValueConverter {
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
    /// Значение на которое изменяется.
    /// </summary>
    public double OffsetValue { get; set; } = 1.0;

    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return value is double doubleValue ? doubleValue + OffsetValue : value;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return value is double doubleValue ? doubleValue - OffsetValue : value;
    }
}