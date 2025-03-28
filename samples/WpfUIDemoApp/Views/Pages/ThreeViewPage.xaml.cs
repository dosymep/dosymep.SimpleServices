using dosymep.SimpleServices;

namespace WpfUIDemoApp.Views.Pages;

public partial class ThreeViewPage {
    public ThreeViewPage() { }

    public ThreeViewPage(
        IHasTheme hasTheme,
        IHasLocalization hasLocalization)
        : base(hasTheme, hasLocalization) {
        InitializeComponent();
    }
}