using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace dosymep.WpfCore.Converters;

/// <summary>
/// Конвертирует object в object.
/// </summary>
[ContentProperty(nameof(CoupleItems))]
[ValueConversion(typeof(object), typeof(object))]
public sealed class ObjectToObjectConverter : IValueConverter {
    /// <summary>
    /// Значение по умолчанию для исходника.
    /// </summary>
    public object? DefaultSource { get; set; }

    /// <summary>
    /// Значение по умолчанию для назначения.
    /// </summary>
    public object? DefaultTarget { get; set; }

    /// <summary>
    /// Список пар.
    /// </summary>
    // ReSharper disable once CollectionNeverUpdated.Global
    public ObservableCollection<CoupleItem> CoupleItems { get; set; } = new();

    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        CoupleItem? coupleItem = CoupleItems.FirstOrDefault(item => Equals(item.Source, value));
        return coupleItem?.Target ?? DefaultTarget;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        CoupleItem? coupleItem = CoupleItems.FirstOrDefault(item => Equals(item.Target, value));
        return coupleItem?.Source ?? DefaultSource;
    }
}

/// <summary>
/// Класс пар объектов.
/// </summary>
public sealed class CoupleItem {
    /// <summary>
    /// Исходник.
    /// </summary>
    public object? Source { get; set; }

    /// <summary>
    /// Назначение.
    /// </summary>
    public object? Target { get; set; }
}