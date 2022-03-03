using System.Collections.Generic;
using System.IO;

namespace dosymep.SimpleServices
{
    public interface IOpenFileDialogService : IOpenDialogServiceBase, IFileDialogServiceBase
    {
        FileInfo File { get; }
        IEnumerable<FileInfo> Files { get; }
    }
}