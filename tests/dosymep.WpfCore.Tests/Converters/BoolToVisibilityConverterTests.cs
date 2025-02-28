using System.Globalization;
using System.Windows;

using dosymep.WpfCore.Converters;

namespace dosymep.WpfCore.Tests.Converters;

public sealed class BoolToVisibilityConverterTests {
    private readonly BoolToVisibilityConverter _valueConveter;

    public BoolToVisibilityConverterTests() {
        Visibility trueValue = Visibility.Visible;
        Visibility falseValue = Visibility.Hidden;
        Visibility defaultValue = Visibility.Collapsed;

        _valueConveter = new BoolToVisibilityConverter(trueValue, falseValue, defaultValue);
    }

    [Fact]
    public void Convert_Default_ReturnsDefault() {
        Visibility trueValue = Visibility.Visible;
        Visibility falseValue = Visibility.Hidden;
        Visibility defaultValue = Visibility.Collapsed;

        Assert.Equal(default,
            new BoolToVisibilityConverter(trueValue, falseValue)
                .Convert(default, typeof(Visibility), default, CultureInfo.CurrentCulture));

        Assert.Equal(defaultValue,
            new BoolToVisibilityConverter(trueValue, falseValue, defaultValue)
                .Convert(default, typeof(Visibility), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void Convert_True_ReturnsTrueValue() {
        Assert.Equal(_valueConveter.TrueValue,
            _valueConveter.Convert(true, typeof(Visibility), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void Convert_False_ReturnsFalseValue() {
        Assert.Equal(_valueConveter.FalseValue,
            _valueConveter.Convert(false, typeof(Visibility), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void ConvertBack_Default_ReturnsDefault() {
        Visibility trueValue = Visibility.Visible;
        Visibility falseValue = Visibility.Hidden;
        Visibility defaultValue = Visibility.Collapsed;

        Assert.Equal(default,
            new BoolToVisibilityConverter(trueValue, falseValue)
                .ConvertBack(default, typeof(bool), default, CultureInfo.CurrentCulture));

        Assert.Equal(default,
            new BoolToVisibilityConverter(trueValue, falseValue, defaultValue)
                .ConvertBack(defaultValue, typeof(bool), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void ConvertBack_TrueValue_ReturnsTrue() {
        Assert.Equal(true,
            _valueConveter.ConvertBack(_valueConveter.TrueValue, typeof(bool), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void ConvertBack_FalseValue_ReturnsFalse() {
        Assert.Equal(false,
            _valueConveter.ConvertBack(_valueConveter.FalseValue, typeof(bool), default, CultureInfo.CurrentCulture));
    }
}