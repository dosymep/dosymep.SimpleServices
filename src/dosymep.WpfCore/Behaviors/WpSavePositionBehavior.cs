using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;

using dosymep.SimpleServices;
using dosymep.WpfCore.Native;

using Microsoft.Xaml.Behaviors;

namespace dosymep.WpfCore.Behaviors;

/// <summary>
/// Поведение сохранения позиции окна.
/// </summary>
public sealed class WpSavePositionBehavior : Behavior<Window> {
    /// <summary>
    /// Сервис сериализации.
    /// </summary>
    public static readonly DependencyProperty SerializationServiceProperty = DependencyProperty.Register(
        nameof(SerializationService), typeof(ISerializationService), typeof(WpSavePositionBehavior),
        new PropertyMetadata(default(ISerializationService)));

    /// <summary>
    /// Сервис сериализации.
    /// </summary>
    public ISerializationService SerializationService {
        get => (ISerializationService) GetValue(SerializationServiceProperty);
        set => SetValue(SerializationServiceProperty, value);
    }

    /// <summary>
    /// Путь до конфигурации позиции окна.
    /// </summary>
    public static readonly DependencyProperty ConfigPathProperty = DependencyProperty.Register(
        nameof(ConfigPath), typeof(string), typeof(WpSavePositionBehavior), new PropertyMetadata(default(string)));

    /// <summary>
    /// Путь до конфигурации позиции окна.
    /// </summary>
    public string ConfigPath {
        get => (string) GetValue(ConfigPathProperty);
        set => SetValue(ConfigPathProperty, value);
    }

    /// <inheritdoc />
    protected override void OnAttached() {
        if(ConfigPath is null) {
            throw new InvalidOperationException("ConfigPath cannot be null.");
        }

        if(AssociatedObject is null) {
            throw new InvalidOperationException("AssociatedObject cannot be null.");
        }

        // Если окно было уже загружено
        // такое может быть, когда не используется behaviour в конструкторе
        if(AssociatedObject.IsLoaded) {
            ReadPosition();
        }

        AssociatedObject.Loaded += AssociatedObjectOnLoaded;
        AssociatedObject.Closing += AssociatedObjectOnClosing;
    }

    /// <inheritdoc />
    protected override void OnDetaching() {
        if(ConfigPath is null) {
            throw new InvalidOperationException("ConfigPath cannot be null.");
        }

        if(AssociatedObject is null) {
            throw new InvalidOperationException("AssociatedObject cannot be null.");
        }

        AssociatedObject.Loaded -= AssociatedObjectOnLoaded;
        AssociatedObject.Closing -= AssociatedObjectOnClosing;
    }

    private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs e) {
        ReadPosition();
    }

    private void AssociatedObjectOnClosing(object sender, CancelEventArgs e) {
        SavePosition();
    }

    private void ReadPosition() {
        if(File.Exists(ConfigPath)) {
            WindowPosition windowPosition =
                SerializationService.Deserialize<WindowPosition>(File.ReadAllText(ConfigPath));
            if(windowPosition.WindowPlacement.HasValue) {
                AssociatedObject.SetPlacement(windowPosition.WindowPlacement.Value);
            }
        }
    }

    private void SavePosition() {
        WindowPosition windowPosition = new() {WindowPlacement = AssociatedObject.GetPlacement()};
        string? content = SerializationService.Serialize(windowPosition);

        Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);
        File.WriteAllText(ConfigPath, content);
    }

    internal class WindowPosition {
        public WINDOWPLACEMENT? WindowPlacement { get; set; }
    }
}