using System.Globalization;

using dosymep.WpfCore.Converters;

namespace dosymep.WpfCore.Tests.Converters;

public sealed class ObjectToBoolConverterTests {
    [Fact]
    public void Convert_Default_ReturnsFalse() {
        Assert.Equal(false, 
            new ObjectToBoolConverter().Convert(default, typeof(bool), default, CultureInfo.CurrentCulture));
        
        Assert.Equal(false, 
            new ObjectToBoolConverter(false).Convert(default, typeof(bool), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void InvertConvert_Default_ReturnsTrue() {
        Assert.Equal(true, 
            new ObjectToBoolConverter(true).Convert(default, typeof(bool), default, CultureInfo.CurrentCulture));
    }
    
    [Fact]
    public void Convert_NotDefault_ReturnsTrue() {
        Assert.Equal(true, 
            new ObjectToBoolConverter().Convert(new object(), typeof(bool), default, CultureInfo.CurrentCulture));
        
        Assert.Equal(true, 
            new ObjectToBoolConverter(false).Convert(new object(), typeof(bool), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void InvertConvert_NotDefault_ReturnsFalse() {
        Assert.Equal(false, 
            new ObjectToBoolConverter(true).Convert(new object(), typeof(bool), default, CultureInfo.CurrentCulture));
    }
}