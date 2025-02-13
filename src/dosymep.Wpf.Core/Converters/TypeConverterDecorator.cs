using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace dosymep.Wpf.Core.Converters;

/// <summary>
/// Декоратор для <see cref="TypeConverter" />.
/// </summary>
public sealed class TypeConverterDecorator : IValueConverter {
    /// <summary>
    /// Конструирует объект. 
    /// </summary>
    public TypeConverterDecorator() { }

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="converter">Конвертер типов.</param>
    public TypeConverterDecorator(TypeConverter converter) => Converter = converter;

    /// <summary>
    /// Конвертер типов.
    /// </summary>
    public TypeConverter? Converter { get; set; }

    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if(Converter is null) {
            throw new InvalidOperationException("Converter is not set.");
        }

        return Converter.ConvertFrom(value);
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if(Converter is null) {
            throw new InvalidOperationException("Converter is not set.");
        }

        return Converter.ConvertFrom(value);
    }
}