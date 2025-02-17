using System.Globalization;
using System.Windows.Data;

namespace dosymep.WpfCore.Converters;

/// <summary>
/// Инвертированный конвертер bool в bool.
/// </summary>
[ValueConversion(typeof(bool), typeof(bool))]
public sealed class BoolNegationConverter : IValueConverter {
    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        bool? boolValue = (bool?) value;

        if(boolValue is null) {
            return default;
        }

        return boolValue == false;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        bool? boolValue = (bool?) value;

        if(boolValue is null) {
            return default;
        }

        return boolValue == true;
    }
}