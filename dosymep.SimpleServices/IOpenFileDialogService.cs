using System.Collections.Generic;
using System.IO;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Предоставляет методы для просмотра и открытия файлов.
    /// </summary>
    public interface IOpenFileDialogService : IOpenDialogServiceBase, IFileDialogServiceBase {
        /// <summary>
        /// Возвращает объект определяющий файл выбранный в диалоговом окне.
        /// </summary>
        FileInfo File { get; }
        
        /// <summary>
        /// Возвращает коллекцию указывающую на все файлы выбранные в диалоговом окне.
        /// </summary>
        IEnumerable<FileInfo> Files { get; }
    }
}