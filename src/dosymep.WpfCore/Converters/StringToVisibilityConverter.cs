using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace dosymep.WpfCore.Converters;

/// <summary>
/// Класс конвертации string в Visibility
/// </summary>
[ValueConversion(typeof(string), typeof(Visibility))]
public sealed class StringToVisibilityConverter : IValueConverter {
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public StringToVisibilityConverter() { }
    
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="inverted">Признак инвертированости.</param>
    public StringToVisibilityConverter(bool inverted) => Inverted = inverted;
    
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="inverted">Признак инвертированости.</param>
    /// <param name="falseValue">Вывод, когда значение bool == False.</param>
    public StringToVisibilityConverter(bool inverted, Visibility falseValue) {
        Inverted = inverted;
        FalseValue = falseValue;
    }

    /// <summary>
    /// Признак инвертированости.
    /// </summary>
    public bool Inverted { get; set; }
    
    /// <summary>
    /// Вывод, когда значение bool == False.
    /// </summary>
    public Visibility FalseValue { get; set; } = Visibility.Collapsed;
    
    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        string? stringValue = value?.ToString();
        return string.IsNullOrEmpty(stringValue) ^ Inverted ? FalseValue : Visibility.Visible;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}