using System.Globalization;
using System.Windows.Data;

namespace dosymep.Wpf.Core.Converters;

/// <summary>
/// Конвертер null в string.
/// </summary>
[ValueConversion(typeof(object), typeof(string))]
[ValueConversion(typeof(string), typeof(string))]
public sealed class NullToStringConverter : IValueConverter {
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public NullToStringConverter() { }

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="nullValue">Значение, которое будет отображаться если null.</param>
    public NullToStringConverter(string nullValue) => NullValue = nullValue;

    /// <summary> 
    /// Вывод, когда значение Null.
    /// </summary>
    public string NullValue { get; set; } = "<Нет данных>";

    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        string? stringValue = value?.ToString();
        return string.IsNullOrEmpty(stringValue) ? NullValue : value;
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}