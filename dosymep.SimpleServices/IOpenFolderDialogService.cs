using System.Collections.Generic;
using System.IO;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Предоставляет методы для открытия папок.
    /// </summary>
    public interface IOpenFolderDialogService : IOpenDialogServiceBase, IFileDialogServiceBase {
        /// <summary>
        /// Возвращает объект определяющий папку выбранную в диалоговом окне.
        /// </summary>
        DirectoryInfo Folder { get; }
        
        /// <summary>
        /// Возвращает коллекцию указывающую на все папки выбранные в диалоговом окне.
        /// </summary>
        IEnumerable<DirectoryInfo> Folders { get; }
    }
}