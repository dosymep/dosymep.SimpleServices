using dosymep.SimpleServices;

using WpfUIDemoApp.ViewModels;

namespace WpfUIDemoApp.Views.Pages;

public partial class GridViewPage {
    public GridViewPage() { }

    public GridViewPage(
        IHasTheme hasTheme,
        IHasLocalization hasLocalization)
        : base(hasTheme, hasLocalization) {
        InitializeComponent();

        DataContext = new ExtensionsViewModel();
    }
}