using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;

using dosymep.SimpleServices;
using dosymep.Wpf.Core.MarkupExtensions.Internal;

namespace dosymep.Wpf.Core.MarkupExtensions;

/// <summary>
/// Конвертирует enum в список значений.
/// </summary>
[MarkupExtensionReturnType(typeof(string[]))]
public sealed class EnumToItemsSourceExtension : MarkupExtension {
    private readonly MarkupValueObject _markupValueObject = new();
    private readonly Binding _binding = new(nameof(MarkupValueObject.Value));

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public EnumToItemsSourceExtension() { }

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="enumType">Тип enum.</param>
    public EnumToItemsSourceExtension(Type enumType) => EnumType = enumType;

    /// <summary>
    /// Тип enum.
    /// </summary>
    public Type? EnumType { get; set; }

    /// <inheritdoc />
    public override object? ProvideValue(IServiceProvider serviceProvider) {
        if(EnumType is null) {
            throw new InvalidOperationException("EnumType must be set before calling ProvideValue.");
        }

        if(!EnumType.IsEnum) {
            throw new InvalidOperationException("EnumType must be an enum.");
        }

        IHasLocalization? localization = serviceProvider.GetRootObject<IHasLocalization>();
        ILocalizationService? localizationService = localization?.LocalizationService;

        if(localization is not null) {
            localization.LanguageChanged += _ => UpdateDisplayName(_markupValueObject.Value);
        }

        _binding.Source = _markupValueObject;
        _markupValueObject.Value = GetEnumValues(localizationService);

        return _binding.ProvideValue(serviceProvider);
    }

    private void UpdateDisplayName(object value) {
        IEnumerable<MarkupDisplayObject> list = ((IEnumerable) value)
            .OfType<MarkupDisplayObject>();

        foreach(MarkupDisplayObject magicObject in list) {
            magicObject.UpdateDisplayName();
        }
    }

    private object[] GetEnumValues(ILocalizationService? localizationService) {
        return EnumType?.GetFields(BindingFlags.Static | BindingFlags.Public)
            .Select(item =>
                new MarkupDisplayObject(() => GetEnumName(item, localizationService)) {
                    Value = item.GetValue(null), DisplayName = GetEnumName(item, localizationService)
                })
            .Cast<object>()
            .ToArray() ?? Array.Empty<object>();
    }

    private string GetEnumName(FieldInfo item, ILocalizationService? localizationService) {
        string? desciption = GetDescription(item);

        if(string.IsNullOrEmpty(desciption)) {
            return localizationService?.GetLocalizedString($"{item.FieldType.Name}.{item.Name}")
                   ?? item.Name;
        }

        return localizationService?.GetLocalizedString(desciption)
               ?? localizationService?.GetLocalizedString($"{item.FieldType.Name}.{item.Name}")
               ?? desciption
               ?? item.Name;
    }

    private string? GetDescription(FieldInfo fieldInfo) {
        return fieldInfo.GetCustomAttribute<DescriptionAttribute>()?.Description;
    }
}