namespace dosymep.SimpleServices {
    /// <summary>
    /// Интерфейс с сервисами тем.
    /// </summary>
    public interface IHasTheme {
        /// <summary>
        /// Текущая используемая тема.
        /// </summary>
        UIThemes HostTheme { get; }
        
        /// <summary>
        /// Сервис тем.
        /// </summary>
        IUIThemeService UIThemeService { get; }
        
        /// <summary>
        /// Сервис обновления темы.
        /// </summary>
        IUIThemeUpdaterService UIThemeUpdaterService { get; }
    }
}