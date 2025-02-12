using System.Globalization;
using System.Windows.Data;

namespace dosymep.Wpf.Core.Converters;

/// <summary>
/// Конвертер string в bool.
/// </summary>
[ValueConversion(typeof(string), typeof(bool))]
public sealed class StringToBoolConverter : IValueConverter {
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public StringToBoolConverter() { }
    
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="inverted">Признак инвертированости.</param>
    public StringToBoolConverter(bool inverted) => Inverted = inverted;

    /// <summary>
    /// Признак инвертированости.
    /// </summary>
    public bool Inverted { get; set; }

    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        string? stringValue = value?.ToString();
        return (string.IsNullOrEmpty(stringValue) ? false : true) ^ Inverted;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}