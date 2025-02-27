using System.Globalization;

using dosymep.WpfCore.Converters;

namespace dosymep.WpfCore.Tests;

public sealed class BoolToObjectConverterTests {
    private readonly BoolToObjectConverter _valueConveter;

    public BoolToObjectConverterTests() {
        object trueValue = 730;
        object falseValue = 420;
        object defaultValue = 777;

        _valueConveter = new BoolToObjectConverter(trueValue, falseValue, defaultValue);
    }

    [Fact]
    public void Convert_Default_ReturnsDefault() {
        object trueValue = 730;
        object falseValue = 420;
        object defaultValue = 777;

        Assert.Equal(default,
            new BoolToObjectConverter(trueValue, falseValue)
                .Convert(default, typeof(object), default, CultureInfo.CurrentCulture));

        Assert.Equal(defaultValue,
            new BoolToObjectConverter(trueValue, falseValue, defaultValue)
                .Convert(default, typeof(object), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void Convert_True_ReturnsTrueValue() {
        Assert.Equal(_valueConveter.TrueValue,
            _valueConveter.Convert(true, typeof(object), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void Convert_False_ReturnsFalseValue() {
        Assert.Equal(_valueConveter.FalseValue,
            _valueConveter.Convert(false, typeof(object), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void ConvertBack_Default_ReturnsDefault() {
        object trueValue = 730;
        object falseValue = 420;
        object defaultValue = 777;

        Assert.Equal(default,
            new BoolToObjectConverter(trueValue, falseValue)
                .ConvertBack(default, typeof(bool), default, CultureInfo.CurrentCulture));

        Assert.Equal(default,
            new BoolToObjectConverter(trueValue, falseValue, defaultValue)
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