using System;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Предоставляет доступ к теме интерфейса.
    /// </summary>
    public interface IUIThemeService {
        /// <summary>
        /// Событие возникающее при изменении темы.
        /// </summary>
        event Action<UIThemes> UIThemeChanged;
        
        /// <summary>
        /// Текущая используемая тема.
        /// </summary>
        UIThemes HostTheme { get; }
    }

    /// <summary>
    /// Используемые темы.
    /// </summary>
    public enum UIThemes {
        /// <summary>
        /// Темная.
        /// </summary>
        Dark,
        
        /// <summary>
        /// Светлая тема.
        /// </summary>
        Light
    }
}