using System.Globalization;

using dosymep.WpfCore.Converters;

namespace dosymep.WpfCore.Tests.Converters;

public sealed class IntOffsetConverterTests {
    private readonly IntOffsetConverter _valueConverter;
    private readonly IntOffsetConverter _invertedValueConverter;

    public IntOffsetConverterTests() {
        int offsetValue = 42;
        _valueConverter = new IntOffsetConverter(offsetValue);
        _invertedValueConverter = new IntOffsetConverter(true, offsetValue);
    }

    [Fact]
    public void Convert_Default_ReturnsDefault() {
        Assert.Equal(default, 
            _valueConverter.Convert(default, typeof(int), default, CultureInfo.CurrentCulture));
        
        Assert.Equal(default,
            _invertedValueConverter.ConvertBack(default, typeof(int), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void Convert_Value_ReturnsOffsetedValue() {
        int value = 73;
        Assert.Equal(value + _valueConverter.OffsetValue,
            _valueConverter.Convert(value, typeof(int), default, CultureInfo.CurrentCulture));

        Assert.Equal(value + _invertedValueConverter.OffsetValue,
            _invertedValueConverter.ConvertBack(value, typeof(int), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void ConvertBack_OffsetedValue_ReturnsValue() {
        int value = 73;
        Assert.Equal(value - _valueConverter.OffsetValue,
            _valueConverter.ConvertBack(value, typeof(int), default, CultureInfo.CurrentCulture));

        Assert.Equal(value - _invertedValueConverter.OffsetValue,
            _invertedValueConverter.Convert(value, typeof(int), default, CultureInfo.CurrentCulture));
    }
}