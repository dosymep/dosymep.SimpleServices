using System.Windows;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Интерфейс по обновлению тем окон.
    /// </summary>
    public interface IThemeUpdaterService {
        /// <summary>
        /// Устанавливает текущую тему окна.
        /// </summary>
        /// <param name="window">Окно, которому устанавливают тему.</param>
        /// <param name="theme">Устанавливаемая тема окна.</param>
        void SetTheme(Window window, UIThemes theme);
    }
}