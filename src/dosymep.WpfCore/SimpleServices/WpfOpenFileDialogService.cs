using System.IO;
using System.Windows;

using dosymep.SimpleServices;

using Microsoft.Win32;

namespace dosymep.WpfCore.SimpleServices;

internal sealed class WpfOpenFileDialogService : WpfBaseService, IOpenFileDialogService {
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

    public FileInfo? File { get; private set; }
    public IEnumerable<FileInfo>? Files { get; private set; }

    public bool ShowDialog() {
        return ShowDialog(InitialDirectory ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
    }

    public bool ShowDialog(string directoryName) {
        OpenFileDialog dialog = new() {
            Multiselect = Multiselect,
            AddExtension = AddExtension,
            CheckFileExists = CheckFileExists,
            CheckPathExists = CheckPathExists,
            ValidateNames = ValidateNames,
            DereferenceLinks = DereferenceLinks,
            RestoreDirectory = RestoreDirectory,
            FilterIndex = FilterIndex,
            Title = Title ?? string.Empty,
            Filter = Filter ?? string.Empty,
            InitialDirectory = directoryName
        };

        bool? result = dialog.ShowDialog();

        File = new FileInfo(dialog.SafeFileName);
        Files = dialog.SafeFileNames.Select(item => new FileInfo(item)).ToArray();

        return result == true;
    }

    public void Reset() {
        // nothing to do
    }
}