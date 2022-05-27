using System.Collections.Generic;
using System.IO;
using System.Windows;

using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Dialogs;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Класс сервиса открытия диалога сохранения файла.
    /// </summary>
    public class XtraSaveFileDialogService : XtraBaseWindowService<DXSaveFileDialogService>, ISaveFileDialogService {
        /// <summary>
        /// Создает экземпляр сервиса открытия диалога сохранения файла.
        /// </summary>
        /// <param name="window">Родительское окно сервиса.</param>
        public XtraSaveFileDialogService(Window window)
            : base(window, new DXSaveFileDialogService()) {
        }

        /// <inheritdoc />
        public bool CheckFileExists {
            get => _serviceBase.CheckFileExists;
            set => _serviceBase.CheckFileExists = value;
        }

        /// <inheritdoc />
        public bool AddExtension {
            get => _serviceBase.AddExtension;
            set => _serviceBase.AddExtension = value;
        }

        /// <inheritdoc />
        public bool AutoUpgradeEnabled {
            get => _serviceBase.AutoUpgradeEnabled;
            set => _serviceBase.AutoUpgradeEnabled = value;
        }

        /// <inheritdoc />
        public bool CheckPathExists {
            get => _serviceBase.CheckPathExists;
            set => _serviceBase.CheckPathExists = value;
        }

        /// <inheritdoc />
        public bool DereferenceLinks {
            get => _serviceBase.DereferenceLinks;
            set => _serviceBase.DereferenceLinks = value;
        }

        /// <inheritdoc />
        public bool RestoreDirectory {
            get => _serviceBase.RestoreDirectory;
            set => _serviceBase.RestoreDirectory = value;
        }

        /// <inheritdoc />
        public bool ShowHelp {
            get => _serviceBase.ShowHelp;
            set => _serviceBase.ShowHelp = value;
        }

        /// <inheritdoc />
        public bool SupportMultiDottedExtensions {
            get => _serviceBase.SupportMultiDottedExtensions;
            set => _serviceBase.SupportMultiDottedExtensions = value;
        }

        /// <inheritdoc />
        public bool ValidateNames {
            get => _serviceBase.ValidateNames;
            set => _serviceBase.ValidateNames = value;
        }

        /// <inheritdoc />
        public int FilterIndex {
            get => _serviceBase.FilterIndex;
            set => _serviceBase.FilterIndex = value;
        }

        /// <inheritdoc />
        public string DefaultExt {
            get => _serviceBase.DefaultExt;
            set => _serviceBase.DefaultExt = value;
        }

        /// <inheritdoc />
        public string DefaultFileName {
            get => _serviceBase.DefaultFileName;
            set => _serviceBase.DefaultFileName = value;
        }

        /// <inheritdoc />
        public string Title {
            get => _serviceBase.Title;
            set => _serviceBase.Title = value;
        }

        /// <inheritdoc />
        public string Filter {
            get => _serviceBase.Filter;
            set => _serviceBase.Filter = value;
        }

        /// <inheritdoc />
        public string InitialDirectory {
            get => _serviceBase.InitialDirectory;
            set => _serviceBase.InitialDirectory = value;
        }

        /// <inheritdoc />
        public void Reset() {
            ((DevExpress.Mvvm.ISaveFileDialogService)_serviceBase).Reset();
        }

        /// <inheritdoc />
        public bool ShowDialog(string directoryName, string fileName) {
            return ((DevExpress.Mvvm.ISaveFileDialogService)_serviceBase).ShowDialog(null, directoryName, fileName);
        }

        /// <inheritdoc />
        public FileInfo File => ((FileInfoWrapper) ((DevExpress.Mvvm.ISaveFileDialogService)_serviceBase).File).FileInfo;
    }
}