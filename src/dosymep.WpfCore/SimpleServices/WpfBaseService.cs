using System.Windows;

using dosymep.SimpleServices;

namespace dosymep.WpfCore.SimpleServices;

/// <summary>
/// Базовый класс прикрепления сервисов.
/// </summary>
public abstract class WpfBaseService : IAttachableService {
    /// <inheritdoc />
    public virtual bool IsAttached => AssociatedObject != default;

    /// <inheritdoc />
    public virtual bool AllowAttach { get; set; } = true;
    
    /// <inheritdoc />
    public virtual DependencyObject? AssociatedObject { get; protected set; }
    
    /// <inheritdoc />
    public virtual void Detach() {
        if(AllowAttach) {
            AssociatedObject = default;
        }
    }

    /// <inheritdoc />
    public virtual void Attach(DependencyObject dependencyObject) {
        if(AllowAttach) {
            AssociatedObject = dependencyObject;
        }
    }
}