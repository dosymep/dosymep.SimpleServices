using System.Globalization;

using dosymep.WpfCore.Converters;

namespace dosymep.WpfCore.Tests.Converters;

public sealed class BoolNegationConverterTests {
    [Fact]
    public void Convert_Default_ReturnsDefault() {
        Assert.Equal(default, 
            new BoolNegationConverter().Convert(default, typeof(bool), default, CultureInfo.CurrentCulture));
    }
        
    [Fact]
    public void Convert_True_ReturnsFalse() {
        Assert.Equal(false, 
            new BoolNegationConverter().Convert(true, typeof(bool), default, CultureInfo.CurrentCulture));
    }
        
    [Fact]
    public void Convert_False_ReturnsTrue() {
        Assert.Equal(true, 
            new BoolNegationConverter().Convert(false, typeof(bool), default, CultureInfo.CurrentCulture));
    }
        
    [Fact]
    public void ConvertBack_Default_ReturnsDefault() {
        Assert.Equal(default, 
            new BoolNegationConverter().ConvertBack(default, typeof(bool), default, CultureInfo.CurrentCulture));
    }
        
    [Fact]
    public void ConvertBack_True_ReturnsFalse() {
        Assert.Equal(false, 
            new BoolNegationConverter().ConvertBack(true, typeof(bool), default, CultureInfo.CurrentCulture));
    }
        
    [Fact]
    public void ConvertBack_False_ReturnsTrue() {
        Assert.Equal(true, 
            new BoolNegationConverter().ConvertBack(false, typeof(bool), default, CultureInfo.CurrentCulture));
    }
}