using System.Windows;

using dosymep.SimpleServices;

namespace dosymep.WpfUI.Core.SimpleServices;

/// <summary>
/// Базовый класс прикрепления сервисов.
/// </summary>
public abstract class WpfUIBaseService : IAttachableService {
    /// <inheritdoc />
    public virtual bool IsAttached => AssociatedObject != default;

    /// <inheritdoc />
    public virtual bool AllowAttach { get; } = true;
    
    /// <inheritdoc />
    public virtual DependencyObject? AssociatedObject { get; protected set; }
    
    /// <inheritdoc />
    public virtual void Detach() {
        AssociatedObject = default;
    }

    /// <inheritdoc />
    public virtual void Attach(DependencyObject dependencyObject) {
        AssociatedObject = dependencyObject;
    }
}