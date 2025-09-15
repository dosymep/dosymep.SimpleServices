using System.IO;

using dosymep.SimpleServices;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace dosymep.WpfCore.SimpleServices;

/// <summary>
/// Предоставляет методы для открытия диалога выбора папок.
/// </summary>
public sealed class WpfOpenFolderDialogService : WpfBaseService, IOpenFolderDialogService {
    /// <inheritdoc />
    public bool Multiselect { get; set; }

    /// <inheritdoc />
    public bool AddExtension { get; set; }

    /// <inheritdoc />
    public bool AutoUpgradeEnabled { get; set; }

    /// <inheritdoc />
    public bool CheckFileExists { get; set; }

    /// <inheritdoc />
    public bool CheckPathExists { get; set; }

    /// <inheritdoc />
    public bool ValidateNames { get; set; }

    /// <inheritdoc />
    public bool DereferenceLinks { get; set; }

    /// <inheritdoc />
    public bool RestoreDirectory { get; set; }

    /// <inheritdoc />
    public bool ShowHelp { get; set; }

    /// <inheritdoc />
    public bool SupportMultiDottedExtensions { get; set; }

    /// <inheritdoc />
    public int FilterIndex { get; set; }

    /// <inheritdoc />
    public string? Title { get; set; }

    /// <inheritdoc />
    public string? Filter { get; set; }

    /// <inheritdoc />
    public string? InitialDirectory { get; set; }

    /// <inheritdoc />
    public DirectoryInfo? Folder { get; private set; }

    /// <inheritdoc />
    public IEnumerable<DirectoryInfo>? Folders { get; private set; }

    /// <inheritdoc />
    public bool ShowDialog() {
        return ShowDialog(InitialDirectory ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
    }

    /// <inheritdoc />
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

        if(result == CommonFileDialogResult.Ok) {
            Folder = new DirectoryInfo(dialog.FileName);
            Folders = dialog.FileNames.Select(item => new DirectoryInfo(item)).ToArray();
        } else {
            Folder = null;
            Folders = null;
        }

        return result == CommonFileDialogResult.Ok;
    }

    /// <inheritdoc />
    public void Reset() {
        throw new NotImplementedException();
    }
}