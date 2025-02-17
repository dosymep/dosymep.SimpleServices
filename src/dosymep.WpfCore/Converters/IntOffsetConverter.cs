using System.Globalization;
using System.Windows.Data;

namespace dosymep.WpfCore.Converters;

/// <summary>
/// Добавляет указаное значение.
/// </summary>
[ValueConversion(typeof(int), typeof(int))]
public sealed class IntOffsetConverter : IValueConverter {
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public IntOffsetConverter() { }

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="offsetValue">Значение на которое изменяется.</param>
    public IntOffsetConverter(int offsetValue) => OffsetValue = offsetValue;
    
    /// <summary>
    /// Значение на которое изменяется.
    /// </summary>
    public int OffsetValue { get; set; } = 1;

    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return value is int intValue ? intValue + OffsetValue : value;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return value is int intValue ? intValue - OffsetValue : value;
    }
}