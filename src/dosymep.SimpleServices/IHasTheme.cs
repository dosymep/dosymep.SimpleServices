namespace dosymep.SimpleServices {
    /// <summary>
    /// Интерфейс с сервисами тем.
    /// </summary>
    public interface IHasTheme {
        /// <summary>
        /// Текущая используемая тема.
        /// </summary>
        UIThemes HostTheme { get; }
    }
}