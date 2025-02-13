using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;

using dosymep.SimpleServices;
using dosymep.Wpf.Core.MarkupExtensions.Internal;

namespace dosymep.Wpf.Core.MarkupExtensions;

/// <summary>
/// Класс для получения локализации.
/// </summary>
public sealed class LocalizationSourceExtension : MarkupExtension {
    private readonly MarkupValueObject _markupValueObject = new();
    private readonly Binding _binding = new(nameof(MarkupValueObject.Value));

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

        IHasLocalization? localization = serviceProvider.GetRootObject<IHasLocalization>();
        if(localization is not null) {
            localization.LanguageChanged += _ => _markupValueObject.Value = GetLocalizedString(localization);
        }

        _binding.Source = _markupValueObject;
        _markupValueObject.Value = GetLocalizedString(localization);
       
        return _binding.ProvideValue(serviceProvider);
    }

    private string? GetLocalizedString(IHasLocalization? localization) {
        return localization?.LocalizationService.GetLocalizedString(ResourceKey) ?? ResourceKey;
    }
}