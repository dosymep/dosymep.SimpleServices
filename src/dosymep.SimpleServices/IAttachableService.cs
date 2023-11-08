using System.Windows;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Интерфейс прикрепления сервиса.
    /// </summary>
    public interface IAttachableService {
        /// <summary>
        /// true - если прикреплен, иначе false.
        /// </summary>
        bool IsAttached { get; }
        
        /// <summary>
        /// Разрешение прикрепления. true - разрешено, false - запрещено.
        /// </summary>
        /// <remarks>При запрете прикрепления, прикреплении не активируется и пропускается.</remarks>
        bool AllowAttach { get; }
        
        /// <summary>
        /// Ассоциированный объект с сервисом.
        /// </summary>
        DependencyObject AssociatedObject { get; }
        
        /// <summary>
        /// Открепление сервиса.
        /// </summary>
        void Detach();
        
        /// <summary>
        /// Прикрепление сервиса.
        /// </summary>
        /// <param name="dependencyObject">Объект с которым прикрепляется сервис.</param>
        void Attach(DependencyObject dependencyObject);
    }
}