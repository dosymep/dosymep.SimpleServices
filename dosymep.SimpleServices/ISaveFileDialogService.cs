using System.IO;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Предоставляет методы для сохранения данных в файл.
    /// </summary>
    public interface ISaveFileDialogService : IFileDialogServiceBase {
        /// <summary>
        /// Расширение сохраняемого файла по умолчанию.
        /// </summary>
        string DefaultExt { get; set; }
        
        /// <summary>
        /// Наименование сохраняемого файла по умолчанию.
        /// </summary>
        string DefaultFileName { get; set; }

        /// <summary>
        /// Возвращает объект определяющий файл выбранный в диалоговом окне.
        /// </summary>
        FileInfo File { get; }
        
        /// <summary>
        /// Показывает диалог сохранения файла.
        /// </summary>
        /// <param name="directoryName">Устанавливает исходную папку.</param>
        /// <param name="fileName">Наименование файла.</param>
        /// <returns>Возвращает результат true - если пользователь кликнул OK, иначе возвращает false.</returns>
        bool ShowDialog(string directoryName, string fileName);
    }
}