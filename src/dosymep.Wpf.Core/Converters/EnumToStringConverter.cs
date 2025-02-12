using System.Globalization;
using System.Windows.Data;

namespace dosymep.Wpf.Core.Converters;

/// <summary>
/// Конвертирует enum в string.
/// </summary>
[ValueConversion(typeof(Enum), typeof(string))]
public sealed class EnumToStringConverter : IValueConverter {
    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}