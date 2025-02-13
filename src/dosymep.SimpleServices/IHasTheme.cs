using System;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Интерфейс с сервисами тем.
    /// </summary>
    public interface IHasTheme {
        /// <summary>
        /// Событие изменения темы.
        /// </summary>
        event Action<UIThemes> ThemeChanged;
        
        /// <summary>
        /// Текущая используемая тема.
        /// </summary>
        UIThemes HostTheme { get; }
        
        /// <summary>
        /// Сервис изменения тем.
        /// </summary>
        IUIThemeUpdaterService ThemeUpdaterService { get; }
    }
}