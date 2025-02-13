using System.Diagnostics;
using System.Windows.Markup;
using System.Xaml;

using dosymep.SimpleServices;

namespace dosymep.Wpf.Core.MarkupExtensions;

/// <summary>
/// 
/// </summary>
public sealed class LocalizationSourceExtension : MarkupExtension {

    public LocalizationSourceExtension() {

    }

    public LocalizationSourceExtension(string resourceKey) {
        ResourceKey = resourceKey;
    }

    public string? ResourceKey { get; set; }

    public override object? ProvideValue(IServiceProvider serviceProvider) {
        if(string.IsNullOrEmpty(ResourceKey)) {
            throw new InvalidOperationException("ResourceKey cannot be null or empty.");
        }

        IRootObjectProvider rootObjectProvider =
            (IRootObjectProvider) serviceProvider.GetService(typeof(IRootObjectProvider));
        
        Debug.Print("ResourceKey: {0}", ResourceKey);
        Debug.Print("RootObject: {0}", rootObjectProvider.RootObject);

        ILocalizationService? localizationService = rootObjectProvider.RootObject as ILocalizationService;
        return localizationService?.GetLocalizedString(ResourceKey) ?? ResourceKey;
    }
}