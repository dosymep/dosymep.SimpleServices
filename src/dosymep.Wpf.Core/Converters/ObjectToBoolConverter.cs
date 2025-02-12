using System.Globalization;
using System.Windows.Data;

namespace dosymep.Wpf.Core.Converters;

/// <summary>
/// Конвертер object в bool.
/// </summary>
[ValueConversion(typeof(object), typeof(bool))]
public sealed class ObjectToBoolConverter : IValueConverter {
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public ObjectToBoolConverter() { }
    
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="inverted">Признак инвертированости.</param>
    public ObjectToBoolConverter(bool inverted) => Inverted = inverted;
    
    /// <summary>
    /// Признак инвертированости.
    /// </summary>
    public bool Inverted { get; set; }
    
    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return value != null ^ Inverted;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}