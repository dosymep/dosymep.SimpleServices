using System.Globalization;
using System.Windows;

using dosymep.WpfCore.Converters;

namespace dosymep.WpfCore.Tests;

public sealed class StringToVisibilityConverterTests {
    [Fact]
    public void Convert_Default_ReturnsCollapsed() {
        Assert.Equal(Visibility.Collapsed,
            new StringToVisibilityConverter()
                .Convert(default, typeof(Visibility), default, CultureInfo.CurrentCulture));

        Assert.Equal(Visibility.Collapsed,
            new StringToVisibilityConverter(false)
                .Convert(default, typeof(Visibility), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void Convert_Default_ReturnsHidden() {
        Assert.Equal(Visibility.Hidden,
            new StringToVisibilityConverter(false, Visibility.Hidden)
                .Convert(default, typeof(Visibility), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void InvertConvert_Default_ReturnsVisible() {
        Assert.Equal(Visibility.Visible,
            new StringToVisibilityConverter(true)
                .Convert(default, typeof(Visibility), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void Convert_NotDefault_ReturnsVisible() {
        Assert.Equal(Visibility.Visible,
            new StringToVisibilityConverter()
                .Convert("some string", typeof(Visibility), default, CultureInfo.CurrentCulture));

        Assert.Equal(Visibility.Visible,
            new StringToVisibilityConverter(false)
                .Convert("some string", typeof(Visibility), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void InvertConvert_NotDefault_ReturnsCollapsed() {
        Assert.Equal(Visibility.Collapsed,
            new StringToVisibilityConverter(true)
                .Convert("some string", typeof(Visibility), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void InvertConvert_NotDefault_ReturnsHidden() {
        Assert.Equal(Visibility.Hidden,
            new StringToVisibilityConverter(true, Visibility.Hidden)
                .Convert("some string", typeof(Visibility), default, CultureInfo.CurrentCulture));
    }
}