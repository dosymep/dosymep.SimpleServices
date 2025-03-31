using dosymep.SimpleServices;

namespace WpfUIDemoApp.Views.Pages;

public partial class AboutPage {
    public AboutPage() { }

    public AboutPage(
        IHasTheme hasTheme,
        IHasLocalization hasLocalization)
        : base(hasTheme, hasLocalization) {
        InitializeComponent();
    }
}