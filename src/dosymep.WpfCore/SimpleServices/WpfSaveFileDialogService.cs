using System.IO;

using dosymep.SimpleServices;

using Microsoft.Win32;

namespace dosymep.WpfCore.SimpleServices;

/// <summary>
/// Предоставляет методы для открытия диалога сохранения файлов.
/// </summary>
public sealed class WpfSaveFileDialogService : WpfBaseService, ISaveFileDialogService {
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
    public string? DefaultExt { get; set; }

    /// <inheritdoc />
    public string? DefaultFileName { get; set; }

    /// <inheritdoc />
    public FileInfo? File { get; private set; }

    /// <inheritdoc />
    public bool ShowDialog(string? directoryName, string? fileName) {
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
            InitialDirectory =
                directoryName ?? InitialDirectory ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            DefaultExt = DefaultExt ?? string.Empty,
            FileName = fileName ?? DefaultFileName ?? string.Empty
        };

        bool? result = dialog.ShowDialog(GetAssociatedWindow());
        if(result == true) {
            File = new FileInfo(dialog.SafeFileName);
        } else {
            File = null;
        }

        return result == true;
    }

    /// <inheritdoc />
    public void Reset() {
        File = null;
    }
}