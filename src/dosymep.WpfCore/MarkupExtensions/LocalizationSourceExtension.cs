using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

using dosymep.WpfCore.Converters;

namespace dosymep.WpfCore.MarkupExtensions;

/// <summary>
/// Класс для получения локализации.
/// </summary>
[MarkupExtensionReturnType(typeof(object))]
[TypeConverter(typeof(LocalizationSourceExtensionConverter))]
public sealed class LocalizationSourceExtension : MarkupExtension {
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public LocalizationSourceExtension() { }

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="resourceKey">Наименование ресурса.</param>
    public LocalizationSourceExtension(string resourceKey) => ResourceKey = resourceKey;

    /// <summary>
    /// Наименование ресурса.
    /// </summary>
    public string? ResourceKey { get; set; }

    /// <inheritdoc />
    public override object? ProvideValue(IServiceProvider serviceProvider) {
        if(string.IsNullOrEmpty(ResourceKey)) {
            throw new InvalidOperationException("ResourceKey is not set.");
        }

        DependencyObject? rootObject = serviceProvider.GetRootObject<DependencyObject>();
        if(rootObject == null || DesignerProperties.GetIsInDesignMode(rootObject)) {
            return ResourceKey;
        }

        // use dynamic resource instead of own realization with MarkupValueObject
        // to fix update localization text when not update because of sequence subscribe to event
        return new DynamicResourceExtension(ResourceKey!).ProvideValue(serviceProvider);
    }
}