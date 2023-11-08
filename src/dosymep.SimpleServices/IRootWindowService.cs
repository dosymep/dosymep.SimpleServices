using System.Windows;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Интерфейс рабочего окна
    /// </summary>
    public interface IRootWindowService {
        /// <summary>
        /// Текущее корневое окно.
        /// </summary>
        Window RootWindow { get; set; }
    }
}