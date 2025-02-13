using System.ComponentModel;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;

using dosymep.SimpleServices;

namespace dosymep.Wpf.Core.MarkupExtensions;

/// <summary>
/// Конвертирует enum в список значений.
/// </summary>
[MarkupExtensionReturnType(typeof(string[]))]
public sealed class EnumToItemsSource : MarkupExtension {
    private readonly MarkupValueObject _markupValueObject = new();
    private readonly Binding _binding = new(nameof(MarkupValueObject.Value));

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public EnumToItemsSource() { }

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="enumType">Тип enum.</param>
    public EnumToItemsSource(Type enumType) => EnumType = enumType;

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
            localization.LanguageChanged += _ => _markupValueObject.Value = GetEnumValues(localizationService);
        }

        _binding.Source = _markupValueObject;
        _markupValueObject.Value = GetEnumValues(localizationService);

        return _binding.ProvideValue(serviceProvider);
    }

    private string[] GetEnumValues(ILocalizationService? localizationService) {
        return EnumType?.GetFields(BindingFlags.Static | BindingFlags.Public)
            .Select(item => GetEnumName(item, localizationService))
            .ToArray() ?? Array.Empty<string>();
    }

    private string GetEnumName(FieldInfo item, ILocalizationService? localizationService) {
        string? desciption = GetDescription(item);

        if(string.IsNullOrEmpty(desciption)) {
            return item.Name;
        }

        return localizationService?.GetLocalizedString(desciption) ?? desciption!;
    }

    private string? GetDescription(FieldInfo fieldInfo) {
        return fieldInfo.GetCustomAttribute<DescriptionAttribute>()?.Description;
    }
}