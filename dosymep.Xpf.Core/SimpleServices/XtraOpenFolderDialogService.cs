using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

using DevExpress.Xpf.Dialogs;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Класс сервиса открытия диалога выбора папки.
    /// </summary>
    public class XtraOpenFolderDialogService : 
        XtraBaseWindowService<DXOpenFolderDialogService>,
        IOpenFolderDialogService {
        
        /// <summary>
        /// Создает экземпляр сервиса открытия диалога выбора папки.
        /// </summary>
        /// <param name="window">Родительское окно сервиса.</param>
        public XtraOpenFolderDialogService(Window window)
            : base(window, new DXOpenFolderDialogService()) {
        }

        /// <inheritdoc />
        public bool Multiselect {
            get => _serviceBase.Multiselect;
            set => _serviceBase.Multiselect = value;
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
            ((DevExpress.Mvvm.IOpenFolderDialogService) _serviceBase).Reset();
        }

        /// <inheritdoc />
        public bool ShowDialog(string directoryName) {
            return ((DevExpress.Mvvm.IOpenFolderDialogService) _serviceBase).ShowDialog(null, directoryName);
        }

        /// <inheritdoc />
        public DirectoryInfo Folder
            => ((DirectoryInfoWrapper) ((DevExpress.Mvvm.IOpenFolderDialogService) _serviceBase).Folder).DirectoryInfo;

        /// <inheritdoc />
        public IEnumerable<DirectoryInfo> Folders => ((DevExpress.Mvvm.IOpenFolderDialogService) _serviceBase).Folders.Select(item => ((DirectoryInfoWrapper) item).DirectoryInfo);
    }
}