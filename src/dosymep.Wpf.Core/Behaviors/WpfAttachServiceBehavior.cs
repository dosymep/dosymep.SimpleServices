using System.Windows;

using dosymep.SimpleServices;

using Microsoft.Xaml.Behaviors;

namespace dosymep.Wpf.Core.Behaviors;

/// <summary>
/// Поведение прикрепление сервисов.
/// </summary>
public sealed class WpfAttachServiceBehavior : Behavior<DependencyObject> {
    /// <summary>
    /// Свойство прикрепляемого сервиса.
    /// </summary>
    public static readonly DependencyProperty AttachableServiceProperty =
        DependencyProperty.Register(
            nameof(AttachableService),
            typeof(IAttachableService),
            typeof(WpfAttachServiceBehavior),
            new PropertyMetadata(null, OnAttachableServiceChanged));

    private static void OnAttachableServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        (e.OldValue as IAttachableService)?.Detach();
        ((WpfAttachServiceBehavior) d).AttachService();
    }

    /// <summary>
    /// Прикрепляемый сервис.
    /// </summary>
    public IAttachableService? AttachableService {
        get => (IAttachableService) GetValue(AttachableServiceProperty);
        set => SetValue(AttachableServiceProperty, value);
    }

    /// <inheritdoc />
    protected override void OnAttached() {
        base.OnAttached();
        AttachService();
    }

    /// <inheritdoc />
    protected override void OnDetaching() {
        base.OnDetaching();
        AttachableService?.Detach();
    }

    private void AttachService() {
        if(AttachableService == null || AssociatedObject == null) {
            return;
        }

        if(AttachableService.IsAttached) {
            AttachableService.Detach();
        }

        AttachableService.Attach(AssociatedObject);
    }
}