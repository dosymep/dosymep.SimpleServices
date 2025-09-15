using System.IO;

using dosymep.SimpleServices;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace dosymep.WpfCore.SimpleServices;

internal sealed class WpfOpenFolderDialogService:WpfBaseService, IOpenFolderDialogService {
    public bool Multiselect { get; set; }
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

    public DirectoryInfo? Folder { get; private set; }
    public IEnumerable<DirectoryInfo>? Folders { get; private set; }
    
    public bool ShowDialog() {
        return ShowDialog(InitialDirectory ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
    }

    public bool ShowDialog(string directoryName) {
        CommonOpenFileDialog dialog = new();
        dialog.IsFolderPicker = true;

        dialog.Multiselect = Multiselect;
        dialog.EnsureFileExists = CheckFileExists;
        dialog.EnsurePathExists = CheckPathExists;
        dialog.EnsureValidNames = ValidateNames;
        dialog.RestoreDirectory = RestoreDirectory;
        dialog.Title = Title ?? string.Empty;
        dialog.InitialDirectory = directoryName;
        
        CommonFileDialogResult? result = dialog.ShowDialog();

        Folder = new DirectoryInfo(dialog.FileName);
        Folders = dialog.FileNames.Select(item => new DirectoryInfo(item)).ToArray();

        return result == CommonFileDialogResult.Ok;
    }
    
    public void Reset() {
        throw new NotImplementedException();
    }
}