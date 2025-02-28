using System.Globalization;
using System.Windows.Data;

namespace dosymep.WpfCore.Converters;

/// <summary>
/// Конвертер bool в string.
/// </summary>
[ValueConversion(typeof(bool), typeof(string))]
public class BoolToStringConverter : IValueConverter {
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public BoolToStringConverter() { }

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="trueValue">Значение, которое будет отображаться если true.</param>
    /// <param name="falseValue">Значение, которое будет отображаться если false.</param>
    public BoolToStringConverter(string trueValue, string falseValue)
        : this(trueValue, falseValue, default) { }

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="trueValue">Значение, которое будет отображаться если true.</param>
    /// <param name="falseValue">Значение, которое будет отображаться если false.</param>
    /// <param name="defaultValue">Значение, которое будет отображаться если null.</param>
    public BoolToStringConverter(string trueValue, string falseValue, string? defaultValue) {
        TrueValue = trueValue;
        FalseValue = falseValue;
        DefaultValue = defaultValue;
    }
    
    /// <summary> 
    /// Вывод, когда значение bool == True.
    /// </summary>
    public string TrueValue { get; set; } = "Да";

    /// <summary>
    /// Вывод, когда значение bool == False.
    /// </summary>
    public string FalseValue { get; set; } = "Нет";

    /// <summary>
    /// Вывод, когда значение bool == default.
    /// </summary>
    public string? DefaultValue { get; set; } = default;


    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        bool? boolValue = (bool?) value;

        if(boolValue is null) {
            return DefaultValue;
        }

        return boolValue == true ? TrueValue : FalseValue;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        string? stringValue = value as string;
       
        if(stringValue?.Equals(TrueValue) == true) {
            return true;
        } else if(stringValue?.Equals(FalseValue) == true) {
            return false;
        }

        return default;
    }
}