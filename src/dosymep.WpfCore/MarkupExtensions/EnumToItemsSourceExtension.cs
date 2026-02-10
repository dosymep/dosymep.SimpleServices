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

using Binding = System.Windows.Data.Binding;

namespace dosymep.WpfCore.MarkupExtensions;

/// <summary>
/// Конвертирует enum в список значений.
/// </summary>
[MarkupExtensionReturnType(typeof(MarkupValueObject))]
public sealed class EnumToItemsSourceExtension : MarkupExtension {
    private readonly MarkupValueObject _markupValueObject = new();
    private readonly Binding _binding = new(nameof(MarkupValueObject.Value));

    private IHasLocalization? _localization;

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

    internal IReadOnlyCollection<EnumInfo>? ItemsSource => _markupValueObject.Value as IReadOnlyCollection<EnumInfo>;

    /// <inheritdoc />
    public override object? ProvideValue(IServiceProvider serviceProvider) {
        if(EnumType is null) {
            throw new InvalidOperationException("EnumType is not set.");
        }

        if(!EnumType.IsEnum) {
            throw new InvalidOperationException("EnumType must be an enum.");
        }

        // получаем корневой элемент,
        // может быть Window, Page, UserControl
        FrameworkElement? rootObject = serviceProvider.GetRootObject<FrameworkElement>();

        // для случая, если окно уже получено
        _localization = rootObject as IHasLocalization;

        if(_localization is null && rootObject is not null) {
            // либо ждем его загрузку,
            // чтобы можно было получить корневой элемент Window
            rootObject.Loaded += RootObjectOnLoaded;
        }

        _binding.Source = _markupValueObject;
        _markupValueObject.Value = GetEnumValues();

        // попытка установить значения
        // отображаемого имени, контрол может быть уже загружен
        TryUpdateDisplayName();

        // устанавливаем имена свойств обновляемого объекта
        UpdateTargetControlProperties(serviceProvider);
        
        _binding.Mode = BindingMode.OneWay;
        _binding.FallbackValue = Array.Empty<EnumInfo>();
        _binding.TargetNullValue = Array.Empty<EnumInfo>();

        return _binding.ProvideValue(serviceProvider);
    }

    private void RootObjectOnLoaded(object sender, RoutedEventArgs e) {
        if(sender is not FrameworkElement frameworkElement) {
            return;
        }

        // отписываемся от события,
        // потому что при смене темы может повторно вызваться
        frameworkElement.Loaded -= RootObjectOnLoaded;

        _localization = Window.GetWindow(frameworkElement) as IHasLocalization;
        if(_localization is not null) {
            TryUpdateDisplayName();
            _localization.LanguageChanged += _ => TryUpdateDisplayName();
        }
    }

    private void TryUpdateDisplayName() {
        if(ItemsSource is null) {
            return;
        }

        foreach(EnumInfo enumInfo in ItemsSource) {
            enumInfo.UpdateDisplayName(_localization?.LocalizationService);
        }
    }

    private static void UpdateTargetControlProperties(IServiceProvider serviceProvider) {
        IProvideValueTarget? provideValueTarget = serviceProvider.GetService<IProvideValueTarget>();
        if(provideValueTarget?.TargetObject is Selector selector) {
            selector.SelectedValuePath = nameof(EnumInfo.Id);
        }

        if(provideValueTarget?.TargetObject is ItemsControl itemsControl) {
            itemsControl.DisplayMemberPath = nameof(EnumInfo.DisplayName);
        }
    }

    private EnumInfo[] GetEnumValues() {
        return EnumType?.GetFields(BindingFlags.Static | BindingFlags.Public)
            .Select(CreateEnumInfo)
            .ToArray() ?? [];
    }

    private static EnumInfo CreateEnumInfo(FieldInfo fieldInfo) {
        return new EnumInfo(fieldInfo.GetValue(null), fieldInfo);
    }
}