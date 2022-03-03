using System.Collections.Generic;
using System.IO;

namespace dosymep.SimpleServices
{
    public interface IOpenFolderDialogService : IOpenDialogServiceBase, IFileDialogServiceBase
    {
        DirectoryInfo Folder { get; }
        IEnumerable<DirectoryInfo> Folders { get; }
    }
}