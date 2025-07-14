using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;

using dosymep.SimpleServices;
using dosymep.WpfCore.MarkupExtensions.Internal;

using static System.Array;

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

        // устанавливаем имена свойств обновляемого объекта
        SetPropertyPaths(serviceProvider);
        
        // получаем корневой элемент,
        // может быть Window, Page, UserControl
        FrameworkElement? rootObject = serviceProvider.GetRootObject<FrameworkElement>();
        if(rootObject?.IsLoaded == true) {
            // если элемент был загружен, устанавливаем значения
            SetLocalizationStrings(serviceProvider.GetRootObject<IHasLocalization>());
        } else {
            if(rootObject != null) {
                // либо ждем его загрузку,
                // чтобы можно было получить корневой элемент Window
                rootObject.Loaded += (s, e) => {
                    if(s is not DependencyObject dependencyObject) {
                        return;
                    }

                    Window? rootWindow = Window.GetWindow(dependencyObject);
                    SetLocalizationStrings(rootWindow as IHasLocalization);
                };
            }
        }

        _binding.Source = _markupValueObject;
        return _binding.ProvideValue(serviceProvider);
    }

    private void SetLocalizationStrings(IHasLocalization? localization) {
        ILocalizationService? localizationService = localization?.LocalizationService;

        if(localization is not null) {
            localization.LanguageChanged += _ => UpdateDisplayName(_markupValueObject.Value);
        }
        
        _markupValueObject.Value = GetEnumValues(localizationService);
    }

    private static void SetPropertyPaths(IServiceProvider serviceProvider) {
        IProvideValueTarget? provideValueTarget = serviceProvider.GetService<IProvideValueTarget>();
        if(provideValueTarget?.TargetObject is Selector selector) {
            selector.SelectedValuePath = nameof(MarkupDisplayObject.Value);
        }

        if(provideValueTarget?.TargetObject is ItemsControl itemsControl) {
            itemsControl.DisplayMemberPath = nameof(MarkupDisplayObject.DisplayName);
        }
    }

    private static void UpdateDisplayName(object? value) {
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
            .ToArray() ?? [];
    }

    internal static MarkupDisplayObject CreateMarkupDisplayObject(FieldInfo item,
        ILocalizationService? localizationService) {
        return new MarkupDisplayObject(() => GetEnumName(item, localizationService)) {
            Value = item.GetValue(null), DisplayName = GetEnumName(item, localizationService)
        };
    }

    internal static string GetEnumName(FieldInfo item, ILocalizationService? localizationService) {
        string? description = GetDescription(item);

        if(string.IsNullOrEmpty(description)) {
            return localizationService?.GetLocalizedString($"{item.FieldType.Name}.{item.Name}")
                   ?? item.Name;
        }

        return localizationService?.GetLocalizedString(description)
               ?? localizationService?.GetLocalizedString($"{item.FieldType.Name}.{item.Name}")
               ?? description
               ?? item.Name;
    }

    internal static string? GetDescription(FieldInfo fieldInfo) {
        return fieldInfo.GetCustomAttribute<DescriptionAttribute>()?.Description;
    }
}