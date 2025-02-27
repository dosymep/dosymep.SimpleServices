using System.Globalization;

using dosymep.WpfCore.Converters;

namespace dosymep.WpfCore.Tests;

public sealed class NumericToBoolConverterTests {
    [Fact]
    public void Convert_Default_ReturnsFalse() {
        Assert.Equal(false, 
            new NumericToBoolConverter().Convert(default, typeof(bool), default, CultureInfo.CurrentCulture));
        
        Assert.Equal(false, 
            new NumericToBoolConverter(false).Convert(default, typeof(bool), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void InvertConvert_Default_ReturnsTrue() {
        Assert.Equal(true, 
            new NumericToBoolConverter(true).Convert(default, typeof(bool), default, CultureInfo.CurrentCulture));
    }
    
    [Fact]
    public void Convert_IntZero_ReturnsFalse() {
        Assert.Equal(false, 
            new NumericToBoolConverter().Convert(0, typeof(bool), default, CultureInfo.CurrentCulture));
        
        Assert.Equal(false, 
            new NumericToBoolConverter(false).Convert(0, typeof(bool), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void InvertConvert_IntZero_ReturnsTrue() {
        Assert.Equal(true, 
            new NumericToBoolConverter(true).Convert(0, typeof(bool), default, CultureInfo.CurrentCulture));
    }
    
    [Fact]
    public void Convert_FloatZero_ReturnsFalse() {
        Assert.Equal(false, 
            new NumericToBoolConverter().Convert(0.0f, typeof(bool), default, CultureInfo.CurrentCulture));
        
        Assert.Equal(false, 
            new NumericToBoolConverter(false).Convert(0.0f, typeof(bool), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void InvertConvert_FloatZero_ReturnsTrue() {
        Assert.Equal(true, 
            new NumericToBoolConverter(true).Convert(0.0f, typeof(bool), default, CultureInfo.CurrentCulture));
    }
    
    [Fact]
    public void Convert_DoubleZero_ReturnsFalse() {
        Assert.Equal(false, 
            new NumericToBoolConverter().Convert(0.0, typeof(bool), default, CultureInfo.CurrentCulture));
        
        Assert.Equal(false, 
            new NumericToBoolConverter(false).Convert(0.0, typeof(bool), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void InvertConvert_DoubleZero_ReturnsTrue() {
        Assert.Equal(true, 
            new NumericToBoolConverter(true).Convert(0.0, typeof(bool), default, CultureInfo.CurrentCulture));
    }
    
    [Fact]
    public void Convert_IntNotZero_ReturnsTrue() {
        Assert.Equal(true, 
            new NumericToBoolConverter().Convert(42, typeof(bool), default, CultureInfo.CurrentCulture));
        
        Assert.Equal(true, 
            new NumericToBoolConverter(false).Convert(42, typeof(bool), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void InvertConvert_IntNotZero_ReturnsFalse() {
        Assert.Equal(false, 
            new NumericToBoolConverter(true).Convert(42, typeof(bool), default, CultureInfo.CurrentCulture));
    }
    
    [Fact]
    public void Convert_FloatNotZero_ReturnsTrue() {
        Assert.Equal(true, 
            new NumericToBoolConverter().Convert(42.0f, typeof(bool), default, CultureInfo.CurrentCulture));
        
        Assert.Equal(true, 
            new NumericToBoolConverter(false).Convert(42.0f, typeof(bool), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void InvertConvert_FloatNotZero_ReturnsFalse() {
        Assert.Equal(false, 
            new NumericToBoolConverter(true).Convert(42.0f, typeof(bool), default, CultureInfo.CurrentCulture));
    }
    
    [Fact]
    public void Convert_DoubleNotZero_ReturnsTrue() {
        Assert.Equal(true, 
            new NumericToBoolConverter().Convert(42.0, typeof(bool), default, CultureInfo.CurrentCulture));
        
        Assert.Equal(true, 
            new NumericToBoolConverter(false).Convert(42.0, typeof(bool), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void InvertConvert_DoubleNotZero_ReturnsTrue() {
        Assert.Equal(false, 
            new NumericToBoolConverter(true).Convert(42.0, typeof(bool), default, CultureInfo.CurrentCulture));
    }
}