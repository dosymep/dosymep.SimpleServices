using System.Windows;

using dosymep.SimpleServices;

using Wpf.Ui.Markup;

using WpfDemoLib.ViewModels;

namespace WpfUIDemoApp.Views.Pages;

public partial class GridViewPage {
    public GridViewPage() { }

    public GridViewPage(
        IHasTheme hasTheme,
        IHasLocalization hasLocalization)
        : base(hasTheme, hasLocalization) {
        InitializeComponent();

        DataContext = new GroupsViewModel();
    }
}