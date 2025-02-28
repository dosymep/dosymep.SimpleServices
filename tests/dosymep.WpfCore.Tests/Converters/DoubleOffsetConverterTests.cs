using System.Globalization;

using dosymep.WpfCore.Converters;

namespace dosymep.WpfCore.Tests.Converters;

public sealed class DoubleOffsetConverterTests {
    private readonly DoubleOffsetConverter _valueConverter;
    private readonly DoubleOffsetConverter _invertedValueConverter;

    public DoubleOffsetConverterTests() {
        double offsetValue = 42.0;
        _valueConverter = new DoubleOffsetConverter(offsetValue);
        _invertedValueConverter = new DoubleOffsetConverter(true, offsetValue);
    }

    [Fact]
    public void Convert_Default_ReturnsDefault() {
        Assert.Equal(default, 
            _valueConverter.Convert(default, typeof(double), default, CultureInfo.CurrentCulture));
        
        Assert.Equal(default, 
            _invertedValueConverter.ConvertBack(default, typeof(double), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void Convert_Value_ReturnsOffsetedValue() {
        double value = 73.0;
        Assert.Equal(value + _valueConverter.OffsetValue,
            _valueConverter.Convert(value, typeof(double), default, CultureInfo.CurrentCulture));
        
        Assert.Equal(value + _invertedValueConverter.OffsetValue,
            _invertedValueConverter.ConvertBack(value, typeof(double), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void ConvertBack_OffsetedValue_ReturnsValue() {
        double value = 73.0;
        Assert.Equal(value - _valueConverter.OffsetValue,
            _valueConverter.ConvertBack(value, typeof(double), default, CultureInfo.CurrentCulture));
        
        Assert.Equal(value - _invertedValueConverter.OffsetValue,
            _invertedValueConverter.Convert(value, typeof(double), default, CultureInfo.CurrentCulture));
    }
}