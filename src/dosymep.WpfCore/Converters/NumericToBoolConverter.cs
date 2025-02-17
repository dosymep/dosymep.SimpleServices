using System.Globalization;
using System.Windows.Data;

namespace dosymep.WpfCore.Converters;

/// <summary>
/// Конвертер числа в bool.
/// </summary>
[ValueConversion(typeof(int), typeof(bool))]
[ValueConversion(typeof(double), typeof(bool))]
public sealed class NumericToBoolConverter : IValueConverter {
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public NumericToBoolConverter() { }
    
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="inverted">Признак инвертированости.</param>
    public NumericToBoolConverter(bool inverted) => Inverted = inverted;
    
    /// <summary>
    /// Признак инвертированости.
    /// </summary>
    public bool Inverted { get; set; }
    
    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        double doubleValue = System.Convert.ToDouble(value);
        return doubleValue != 0.0 ^ Inverted;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}