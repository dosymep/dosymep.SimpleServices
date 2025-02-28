using System.Globalization;

using dosymep.WpfCore.Converters;

namespace dosymep.WpfCore.Tests.Converters;

public sealed class BoolToStringConverterTests {
    private readonly BoolToStringConverter _valueConveter;

    public BoolToStringConverterTests() {
        string trueValue = "730";
        string falseValue = "420";
        string defaultValue = "777";

        _valueConveter = new BoolToStringConverter(trueValue, falseValue, defaultValue);
    }

    [Fact]
    public void Convert_Default_ReturnsDefault() {
        string trueValue = "730";
        string falseValue = "420";
        string defaultValue = "777";

        Assert.Equal(default,
            new BoolToStringConverter(trueValue, falseValue)
                .Convert(default, typeof(string), default, CultureInfo.CurrentCulture));

        Assert.Equal(defaultValue,
            new BoolToStringConverter(trueValue, falseValue, defaultValue)
                .Convert(default, typeof(string), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void Convert_True_ReturnsTrueValue() {
        Assert.Equal(_valueConveter.TrueValue,
            _valueConveter.Convert(true, typeof(string), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void Convert_False_ReturnsFalseValue() {
        Assert.Equal(_valueConveter.FalseValue,
            _valueConveter.Convert(false, typeof(string), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void ConvertBack_Default_ReturnsDefault() {
        string trueValue = "730";
        string falseValue = "420";
        string defaultValue = "777";

        Assert.Equal(default,
            new BoolToStringConverter(trueValue, falseValue)
                .ConvertBack(default, typeof(bool), default, CultureInfo.CurrentCulture));

        Assert.Equal(default,
            new BoolToStringConverter(trueValue, falseValue, defaultValue)
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