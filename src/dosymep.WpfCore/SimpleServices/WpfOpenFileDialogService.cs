using System.IO;
using System.Windows;

using dosymep.SimpleServices;

using Microsoft.Win32;

namespace dosymep.WpfCore.SimpleServices;

/// <summary>
/// Предоставляет методы для открытия  диалога просмотра и открытия файлов.
/// </summary>
public sealed class WpfOpenFileDialogService : WpfBaseService, IOpenFileDialogService {
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
    public FileInfo? File { get; private set; }

    /// <inheritdoc />
    public IEnumerable<FileInfo>? Files { get; private set; }

    /// <inheritdoc />
    public bool ShowDialog() {
        return ShowDialog(InitialDirectory ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
    }

    /// <inheritdoc />
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
        if(result==true) {
            File = new FileInfo(dialog.SafeFileName);
            Files = dialog.SafeFileNames.Select(item => new FileInfo(item)).ToArray();
        } else {
            File = null;
            Files = null;
        }
        
        return result == true;
    }

    /// <inheritdoc />
    public void Reset() {
        // nothing to do
    }
}