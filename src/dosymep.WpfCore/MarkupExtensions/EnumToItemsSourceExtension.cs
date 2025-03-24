using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;

using dosymep.SimpleServices;
using dosymep.WpfCore.MarkupExtensions.Internal;

namespace dosymep.WpfCore.MarkupExtensions;

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
            throw new InvalidOperationException("EnumType is not set.");
        }

        if(!EnumType.IsEnum) {
            throw new InvalidOperationException("EnumType must be an enum.");
        }

        SetPropertyPaths(serviceProvider);
        SetLocalizationStrings(serviceProvider);

        return _binding.ProvideValue(serviceProvider);
    }

    private void SetLocalizationStrings(IServiceProvider serviceProvider) {
        IHasLocalization? localization = serviceProvider.GetRootObject<IHasLocalization>();
        ILocalizationService? localizationService = localization?.LocalizationService;

        if(localization is not null) {
            localization.LanguageChanged += _ => UpdateDisplayName(_markupValueObject.Value);
        }

        _binding.Source = _markupValueObject;
        _markupValueObject.Value = GetEnumValues(localizationService);
    }

    private static void SetPropertyPaths(IServiceProvider serviceProvider) {
        IProvideValueTarget? provideValueTarget = serviceProvider.GetRootObject<IProvideValueTarget>();
        if(provideValueTarget?.TargetObject is Selector selector) {
            selector.SelectedValuePath = nameof(MarkupDisplayObject.Value);
        }

        if(provideValueTarget?.TargetObject is ItemsControl itemsControl) {
            itemsControl.DisplayMemberPath = nameof(MarkupDisplayObject.DisplayName);
        }
    }

    private void UpdateDisplayName(object? value) {
        if(value == null) {
            return;
        }

        IEnumerable<MarkupDisplayObject> list = ((IEnumerable) value)
            .OfType<MarkupDisplayObject>();

        foreach(MarkupDisplayObject magicObject in list) {
            magicObject.UpdateDisplayName();
        }
    }

    private object[] GetEnumValues(ILocalizationService? localizationService) {
        return EnumType?.GetFields(BindingFlags.Static | BindingFlags.Public)
            .Select(item => CreateMarkupDisplayObject(item, localizationService))
            .Cast<object>()
            .ToArray() ?? Array.Empty<object>();
    }

    internal static MarkupDisplayObject CreateMarkupDisplayObject(FieldInfo item,
        ILocalizationService? localizationService) {
        return new MarkupDisplayObject(() => GetEnumName(item, localizationService)) {
            Value = item.GetValue(null), DisplayName = GetEnumName(item, localizationService)
        };
    }

    internal static string GetEnumName(FieldInfo item, ILocalizationService? localizationService) {
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

    internal static string? GetDescription(FieldInfo fieldInfo) {
        return fieldInfo.GetCustomAttribute<DescriptionAttribute>()?.Description;
    }
}