using System;
using System.ComponentModel;
using System.IO;

namespace dosymep.SimpleServices
{
    public interface ISaveFileDialogService : IFileDialogServiceBase
    {
        string DefaultExt { get; set; }
        string DefaultFileName { get; set; }
        
        FileInfo File { get; }
        bool ShowDialog(string directoryName, string fileName);
    }
}