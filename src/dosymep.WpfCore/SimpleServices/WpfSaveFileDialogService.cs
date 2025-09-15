using System.IO;

using dosymep.SimpleServices;

using Microsoft.Win32;

namespace dosymep.WpfCore.SimpleServices;

internal sealed class WpfSaveFileDialogService :WpfBaseService, ISaveFileDialogService{
    public bool AddExtension { get; set; }
    public bool AutoUpgradeEnabled { get; set; }
    public bool CheckFileExists { get; set; }
    public bool CheckPathExists { get; set; }
    public bool ValidateNames { get; set; }
    public bool DereferenceLinks { get; set; }
    public bool RestoreDirectory { get; set; }
    public bool ShowHelp { get; set; }
    public bool SupportMultiDottedExtensions { get; set; }
    public int FilterIndex { get; set; }
    public string? Title { get; set; }
    public string? Filter { get; set; }
    public string? InitialDirectory { get; set; }
    public string? DefaultExt { get; set; }
    public string? DefaultFileName { get; set; }
    
    public FileInfo? File { get; private set; }
    
    public bool ShowDialog(string directoryName, string fileName) {
        SaveFileDialog dialog = new() {
            AddExtension = AddExtension,
            CheckFileExists = CheckFileExists,
            CheckPathExists = CheckPathExists,
            ValidateNames = ValidateNames,
            DereferenceLinks = DereferenceLinks,
            RestoreDirectory = RestoreDirectory,
            FilterIndex = FilterIndex,
            Title = Title ?? string.Empty,
            Filter = Filter ?? string.Empty,
            InitialDirectory = InitialDirectory ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            DefaultExt = DefaultExt ?? string.Empty,
            FileName = DefaultFileName ?? string.Empty
        };

        bool? result = dialog.ShowDialog();

        File = new FileInfo(dialog.SafeFileName);

        return result == true;
    }
    
    public void Reset() {
        // nothing to do
    }
}