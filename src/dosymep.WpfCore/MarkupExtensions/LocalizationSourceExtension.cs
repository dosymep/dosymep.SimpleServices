using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;

using dosymep.SimpleServices;
using dosymep.WpfCore.MarkupExtensions.Internal;

namespace dosymep.WpfCore.MarkupExtensions;

/// <summary>
/// Класс для получения локализации.
/// </summary>
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

        if(DesignerProperties.GetIsInDesignMode(
               serviceProvider.GetRootObject<DependencyObject>()!)) {
            return ResourceKey;
        }

        // use dynamic resource instead of own realization with MarkupValueObject
        // to fix update localization text when not update because of sequence subscribe to event
        return new DynamicResourceExtension(ResourceKey!).ProvideValue(serviceProvider);
    }
}