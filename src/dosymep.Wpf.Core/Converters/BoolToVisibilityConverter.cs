using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace dosymep.Wpf.Core.Converters;

/// <summary>
/// Конвертер bool в Visibility.
/// </summary>
[ValueConversion(typeof(bool), typeof(Visibility))]
public sealed class BoolToVisibilityConverter : IValueConverter {
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public BoolToVisibilityConverter() { }

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="trueValue">Значение, которое будет отображаться если true.</param>
    /// <param name="falseValue">Значение, которое будет отображаться если false.</param>
    public BoolToVisibilityConverter(Visibility trueValue, Visibility falseValue)
        : this(trueValue, falseValue, default) { }

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="trueValue">Значение, которое будет отображаться если true.</param>
    /// <param name="falseValue">Значение, которое будет отображаться если false.</param>
    /// <param name="defaultValue">Значение, которое будет отображаться если null.</param>
    public BoolToVisibilityConverter(Visibility trueValue, Visibility falseValue, Visibility? defaultValue) {
        TrueValue = trueValue;
        FalseValue = falseValue;
        DefaultValue = defaultValue;
    }

    /// <summary>
    /// Вывод, когда значение bool == True.
    /// </summary>
    public Visibility TrueValue { get; set; } = Visibility.Visible;

    /// <summary>
    /// Вывод, когда значение bool == False.
    /// </summary>
    public Visibility FalseValue { get; set; } = Visibility.Collapsed;

    /// <summary>
    /// Вывод, когда значение bool == default.
    /// </summary>
    public Visibility? DefaultValue { get; set; } = default;

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
        Visibility? visibilityValue = value as Visibility?;

        if(visibilityValue?.Equals(TrueValue) == true) {
            return true;
        } else if(visibilityValue?.Equals(FalseValue) == true) {
            return false;
        }

        return default;
    }
}