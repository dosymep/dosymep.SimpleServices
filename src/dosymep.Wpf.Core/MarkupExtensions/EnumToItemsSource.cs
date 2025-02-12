using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;

namespace dosymep.Wpf.Core.MarkupExtensions;

/// <summary>
/// Конвертирует enum в список значений.
/// </summary>
[MarkupExtensionReturnType(typeof(string[]))]
public sealed class EnumToItemsSource : MarkupExtension {
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

        return EnumType.GetFields(BindingFlags.Static | BindingFlags.Public)
            .Select(item => item
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .FirstOrDefault()?.Description ?? item.Name)
            .ToArray();
    }
}