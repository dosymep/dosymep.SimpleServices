using System.ComponentModel;
using System.Globalization;

using dosymep.WpfCore.Converters;

namespace dosymep.WpfCore.Tests.Converters;

public sealed class TypeConverterDecoratorTests {
    private readonly TypeConverterDecorator _valueConverter;

    public TypeConverterDecoratorTests() {
        _valueConverter = new TypeConverterDecorator(new GuidConverter());
    }

    [Fact]
    public void Convert_DefaultTypeConverter_ThrowsInvalidOperationException() {
        Assert.Throws<InvalidOperationException>(() =>
            new TypeConverterDecorator()
                .Convert("some string", typeof(string), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void ConvertBack_DefaultTypeConverter_ThrowsInvalidOperationException() {
        Assert.Throws<InvalidOperationException>(() =>
            new TypeConverterDecorator()
                .ConvertBack("some string", typeof(string), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void Convert_CannotConvert_ThrowsInvalidOperationException() {
        Assert.Throws<NotSupportedException>(() =>
            _valueConverter.Convert(42, typeof(int), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void ConvertBack_CannotConvert_ThrowsInvalidOperationException() {
        Assert.Throws<NotSupportedException>(() =>
            _valueConverter.ConvertBack(42, typeof(int), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void Convert_Default_ThrowsInvalidOperationException() {
        Assert.Equal(default,
            _valueConverter.Convert(default, typeof(string), default, CultureInfo.CurrentCulture));
    }
    
    [Fact]
    public void ConvertBack_Default_ThrowsInvalidOperationException() {
        Assert.Equal(default,
            _valueConverter.ConvertBack(default, typeof(string), default, CultureInfo.CurrentCulture));
    }
    
    [Fact]
    public void Convert_NotDefault_ThrowsInvalidOperationException() {
        Guid value = Guid.NewGuid();
        Assert.Equal(value,
            _valueConverter.Convert(value.ToString(), typeof(Guid), default, CultureInfo.CurrentCulture));
    }
    
    [Fact]
    public void ConvertBack_NotDefault_ThrowsInvalidOperationException() {
        Guid value = Guid.NewGuid();
        Assert.Equal(value.ToString(),
            _valueConverter.ConvertBack(value, typeof(string), default, CultureInfo.CurrentCulture));
    }
}