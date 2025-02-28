using System.Globalization;

using dosymep.WpfCore.Converters;

namespace dosymep.WpfCore.Tests.Converters;

public sealed class StringToBoolConverterTests {
    [Fact]
    public void Convert_Default_ReturnsFalse() {
        Assert.Equal(false, 
            new StringToBoolConverter().Convert(default, typeof(bool), default, CultureInfo.CurrentCulture));
        
        Assert.Equal(false, 
            new StringToBoolConverter(false).Convert(default, typeof(bool), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void InvertConvert_Default_ReturnsTrue() {
        Assert.Equal(true, 
            new StringToBoolConverter(true).Convert(default, typeof(bool), default, CultureInfo.CurrentCulture));
    }
    
    [Fact]
    public void Convert_NotDefault_ReturnsTrue() {
        Assert.Equal(true, 
            new StringToBoolConverter().Convert("some string", typeof(bool), default, CultureInfo.CurrentCulture));
        
        Assert.Equal(true, 
            new StringToBoolConverter(false).Convert("some string", typeof(bool), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void InvertConvert_NotDefault_ReturnsFalse() {
        Assert.Equal(false, 
            new StringToBoolConverter(true).Convert("some string", typeof(bool), default, CultureInfo.CurrentCulture));
    }
}