using System.Globalization;

using dosymep.WpfCore.Converters;

namespace dosymep.WpfCore.Tests;

public sealed class DoubleOffsetConverterTests {
    private readonly DoubleOffsetConverter _valueConverter;

    public DoubleOffsetConverterTests() {
        double offsetValue = 42.0;
        _valueConverter = new DoubleOffsetConverter(offsetValue);
    }

    [Fact]
    public void Convert_Default_ReturnsDefault() {
        Assert.Equal(default, _valueConverter.Convert(default, typeof(double), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void Convert_Value_ReturnsOffsetedValue() {
        double value = 73.0;
        Assert.Equal(value + _valueConverter.OffsetValue,
            _valueConverter.Convert(value, typeof(double), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void ConvertBack_OffsetedValue_ReturnsValue() {
        double value = 73.0;
        Assert.Equal(value - _valueConverter.OffsetValue,
            _valueConverter.ConvertBack(value, typeof(double), default, CultureInfo.CurrentCulture));
    }
}