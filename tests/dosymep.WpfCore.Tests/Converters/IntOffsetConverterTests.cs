using System.Globalization;

using dosymep.WpfCore.Converters;

namespace dosymep.WpfCore.Tests.Converters;

public sealed class IntOffsetConverterTests {
    private readonly IntOffsetConverter _valueConverter;

    public IntOffsetConverterTests() {
        int offsetValue = 42;
        _valueConverter = new IntOffsetConverter(offsetValue);
    }

    [Fact]
    public void Convert_Default_ReturnsDefault() {
        Assert.Equal(default, _valueConverter.Convert(default, typeof(int), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void Convert_Value_ReturnsOffsetedValue() {
        int value = 73;
        Assert.Equal(value + _valueConverter.OffsetValue,
            _valueConverter.Convert(value, typeof(int), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void ConvertBack_OffsetedValue_ReturnsValue() {
        int value = 73;
        Assert.Equal(value - _valueConverter.OffsetValue,
            _valueConverter.ConvertBack(value, typeof(int), default, CultureInfo.CurrentCulture));
    }
}