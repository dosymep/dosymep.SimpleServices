using System.Globalization;
using System.Windows.Data;

namespace dosymep.WpfCore.Converters;

/// <summary>
/// Конвертер null в string.
/// </summary>
[ValueConversion(typeof(object), typeof(string))]
[ValueConversion(typeof(string), typeof(string))]
public sealed class DefaultToStringConverter : IValueConverter {
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public DefaultToStringConverter() { }

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="defaultValue">Значение, которое будет отображаться если null.</param>
    public DefaultToStringConverter(string defaultValue) => DefaultValue = defaultValue;

    /// <summary> 
    /// Вывод, когда значение Null.
    /// </summary>
    public string DefaultValue { get; set; } = "<Нет данных>";

    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return ReferenceEquals(value, default) ? DefaultValue : value;
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}