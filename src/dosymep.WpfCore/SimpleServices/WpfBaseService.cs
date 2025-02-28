using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;

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

    /// <summary>
    /// Возвращает ассоциированное окно с сервисом.
    /// </summary>
    /// <returns>Возвращает ассоциированное окно с сервисом.</returns>
    protected Window? GetAssociatedWindow() {
        if(AssociatedObject == null) {
            return default;
        }

        return AssociatedObject is Window window
            ? window
            : Window.GetWindow(AssociatedObject);
    }

    /// <summary>
    /// Устанавливает родителя для переданного окна.
    /// </summary>
    /// <param name="window">Окно для которого требуется установить родителя.</param>
    protected void SetAssociatedOwner(Window window) {
        Window? associatedWindow = GetAssociatedWindow();
        if(associatedWindow is not null && associatedWindow.IsVisible) {
            window.Owner = associatedWindow;
        } else {
            new WindowInteropHelper(window).Owner = Process.GetCurrentProcess().MainWindowHandle;
        }
    }
}