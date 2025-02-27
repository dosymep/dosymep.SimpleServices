using System.Globalization;

using dosymep.WpfCore.Converters;

namespace dosymep.WpfCore.Tests;

public sealed class DefaultToStringConverterTests {
    [Fact]
    public void Convert_Default_ReturnsDefaultValue() {
        Assert.Equal("<Нет данных>",
            new DefaultToStringConverter()
                .Convert(default, typeof(string), default, CultureInfo.CurrentCulture));

        Assert.Equal("Default value",
            new DefaultToStringConverter("Default value")
                .Convert(default, typeof(string), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void Convert_NotDefault_ReturnsSameValue() {
        Assert.Equal(42,
            new DefaultToStringConverter().Convert(42, typeof(int), default, CultureInfo.CurrentCulture));

        Assert.Equal("42",
            new DefaultToStringConverter().Convert("42", typeof(string), default, CultureInfo.CurrentCulture));
    }
}