namespace dosymep.SimpleServices {
    /// <summary>
    /// Предоставляет общие члены <see cref="IOpenFileDialogService"/> и <see cref="IOpenFolderDialogService"/>.
    /// </summary>
    public interface IOpenDialogServiceBase {
        /// <summary>
        /// Возвращает или устанавливает признак выбора нескольких файлов.
        /// </summary>
        bool Multiselect { get; set; }
        
        /// <summary>
        /// Показывает диалог выбора.
        /// </summary>
        /// <returns>Возвращает результат true - если пользователь кликнул OK, иначе возвращает false.</returns>
        bool ShowDialog();
        
        /// <summary>
        /// Показывает диалог выбора.
        /// </summary>
        /// <param name="directoryName">Устанавливает исходную папку.</param>
        /// <returns>Возвращает результат true - если пользователь кликнул OK, иначе возвращает false.</returns>
        bool ShowDialog(string directoryName);
    }
}