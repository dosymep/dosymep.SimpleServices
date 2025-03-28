using System.Globalization;
using System.Windows.Controls;

using dosymep.SimpleServices;

using Ninject;
using Ninject.Syntax;

using Wpf.Ui.Abstractions;

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

internal sealed class NavigationViewPageProvider : INavigationViewPageProvider {
    private readonly IResolutionRoot _resolutionRoot;

    public NavigationViewPageProvider(IResolutionRoot resolutionRoot) {
        _resolutionRoot = resolutionRoot;
    }

    public object GetPage(Type pageType) {
        return _resolutionRoot.Get(pageType);
    }
}