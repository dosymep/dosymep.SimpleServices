using System.Collections.ObjectModel;
using System.Globalization;

using dosymep.WpfCore.Converters;

namespace dosymep.WpfCore.Tests.Converters;

public sealed class ObjectToObjectConverterTests {
    [Fact]
    public void Convert_Empty_ReturnsDefaultValue() {
        ObjectToObjectConverter converter = new() {DefaultSource = "Source", DefaultTarget = "Target"};
        Assert.Equal(converter.DefaultTarget,
            converter.Convert(default, typeof(string), null, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void ConvertBack_Empty_ReturnsDefaultValue() {
        ObjectToObjectConverter converter = new() {DefaultSource = "Source", DefaultTarget = "Target"};
        Assert.Equal(converter.DefaultSource,
            converter.ConvertBack(default, typeof(string), null, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void Convert_FoundCouple_ReturnsFindedCouple() {
        object source = "Source Couple";
        object target = "Target Couple";
        ObjectToObjectConverter converter = new() {
            DefaultSource = "Source",
            DefaultTarget = "Target",
            CoupleItems = new ObservableCollection<CoupleItem>() {
                new() {Source = source, Target = target},
                new() {Source = "Some Source", Target = "Some Target"},
                new() {Source = "42", Target = "73"},
                new() {Source = "22", Target = "33"}
            }
        };

        Assert.Equal(target, converter.Convert(source, typeof(string), null, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void ConvertBack_FoundCouple_ReturnsFindedCouple() {
        object source = "Source Couple";
        object target = "Target Couple";
        ObjectToObjectConverter converter = new() {
            DefaultSource = "Source",
            DefaultTarget = "Target",
            CoupleItems = new ObservableCollection<CoupleItem>() {
                new() {Source = source, Target = target},
                new() {Source = "Some Source", Target = "Some Target"},
                new() {Source = "42", Target = "73"},
                new() {Source = "22", Target = "33"}
            }
        };

        Assert.Equal(source, converter.ConvertBack(target, typeof(string), null, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void Convert_NotFoundCouple_ReturnsDefautValue() {
        object source = "Source Find Couple";
        ObjectToObjectConverter converter = new() {
            DefaultSource = "Source",
            DefaultTarget = "Target",
            CoupleItems = new ObservableCollection<CoupleItem>() {
                new() {Source = "Some Source", Target = "Some Target"},
                new() {Source = "42", Target = "73"},
                new() {Source = "22", Target = "33"}
            }
        };

        Assert.Equal(converter.DefaultTarget,
            converter.Convert(source, typeof(string), null, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void ConvertBack_NotFoundCouple_ReturnsDefaultValue() {
        object target = "Target Find Couple";
        ObjectToObjectConverter converter = new() {
            DefaultSource = "Source",
            DefaultTarget = "Target",
            CoupleItems = new ObservableCollection<CoupleItem>() {
                new() {Source = "Some Source", Target = "Some Target"},
                new() {Source = "42", Target = "73"},
                new() {Source = "22", Target = "33"}
            }
        };

        Assert.Equal(converter.DefaultSource,
            converter.ConvertBack(target, typeof(string), null, CultureInfo.CurrentCulture));
    }
}