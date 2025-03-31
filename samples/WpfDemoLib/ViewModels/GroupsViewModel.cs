using System.Collections.ObjectModel;

namespace WpfDemoLib.ViewModels;

public sealed class GroupsViewModel {
    public GroupsViewModel() {
        Extensions = new ObservableCollection<GroupViewModel> {
            new() {Category = "Auto", Name = "Audi", Description = "Audi AG is a German automotive manufacturer of luxury vehicles headquartered in Ingolstadt, Bavaria, Germany. A subsidiary of the Volkswagen Group, Audi produces vehicles in nine production facilities worldwide."},
            new() {Category = "Phone", Name = "Samsung", Description = "Samsung Group is a South Korean multinational manufacturing conglomerate headquartered in the Samsung Town office complex in Seoul. The group consists of numerous affiliated businesses, most of which operate under the Samsung brand, and is the largest chaebol (business conglomerate) in South Korea. As of 2024, Samsung has the world's fifth-highest brand value."},
            new() {Category = "Auto", Name = "Bugatti", Description = "Automobiles Ettore Bugatti was a German then French manufacturer of high-performance automobiles. The company was founded in 1909 in the then-German city of Molsheim, Alsace, by the Italian-born industrial designer Ettore Bugatti. The cars were known for their design beauty and numerous race victories. Famous Bugatti automobiles include the Type 35 Grand Prix cars, the Type 41 \"Royale\", the Type 57 \"Atlantic\" and the Type 55 sports car."},
            new() {Category = "Phone", Name = "Google Pixel", Description = "Google Pixel is a brand of portable consumer electronic devices developed by Google that run either ChromeOS or the Pixel version of the Android operating system. The main line of Pixel products consists of Android-powered smartphones, which have been produced since October 2016 as the replacement for the older Nexus, and of which the Pixel 9, Pixel 9 Pro, Pixel 9 Pro XL and Pixel 9 Pro Fold are the current models. The Pixel brand also includes laptop and tablet computers, as well as several accessories, and was originally introduced in February 2013 with the Chromebook Pixel."},
        };
    }

    public ObservableCollection<GroupViewModel> Extensions { get; }
}