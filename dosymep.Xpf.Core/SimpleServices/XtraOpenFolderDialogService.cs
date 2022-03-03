using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevExpress.Xpf.Dialogs;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.SimpleServices
{
    public class XtraOpenFolderDialogService : IOpenFolderDialogService
    {
        private readonly DevExpress.Mvvm.IOpenFolderDialogService _openFolderDialogService
            = new DXOpenFolderDialogService();

        public bool Multiselect
        {
            get => _openFolderDialogService.Multiselect;
            set => _openFolderDialogService.Multiselect = value;
        }

        public bool CheckFileExists
        {
            get => _openFolderDialogService.CheckFileExists;
            set => _openFolderDialogService.CheckFileExists = value;
        }

        public bool AddExtension
        {
            get => _openFolderDialogService.AddExtension;
            set => _openFolderDialogService.AddExtension = value;
        }

        public bool AutoUpgradeEnabled
        {
            get => _openFolderDialogService.AutoUpgradeEnabled;
            set => _openFolderDialogService.AutoUpgradeEnabled = value;
        }

        public bool CheckPathExists
        {
            get => _openFolderDialogService.CheckPathExists;
            set => _openFolderDialogService.CheckPathExists = value;
        }

        public bool DereferenceLinks
        {
            get => _openFolderDialogService.DereferenceLinks;
            set => _openFolderDialogService.DereferenceLinks = value;
        }

        public bool RestoreDirectory
        {
            get => _openFolderDialogService.RestoreDirectory;
            set => _openFolderDialogService.RestoreDirectory = value;
        }

        public bool ShowHelp
        {
            get => _openFolderDialogService.ShowHelp;
            set => _openFolderDialogService.ShowHelp = value;
        }

        public bool SupportMultiDottedExtensions
        {
            get => _openFolderDialogService.SupportMultiDottedExtensions;
            set => _openFolderDialogService.SupportMultiDottedExtensions = value;
        }

        public bool ValidateNames
        {
            get => _openFolderDialogService.ValidateNames;
            set => _openFolderDialogService.ValidateNames = value;
        }

        public int FilterIndex
        {
            get => _openFolderDialogService.FilterIndex;
            set => _openFolderDialogService.FilterIndex = value;
        }

        public string Title
        {
            get => _openFolderDialogService.Title;
            set => _openFolderDialogService.Title = value;
        }

        public string Filter
        {
            get => _openFolderDialogService.Filter;
            set => _openFolderDialogService.Filter = value;
        }

        public string InitialDirectory
        {
            get => _openFolderDialogService.InitialDirectory;
            set => _openFolderDialogService.InitialDirectory = value;
        }

        public void Reset()
        {
            _openFolderDialogService.Reset();
        }

        public bool ShowDialog(string directoryName)
        {
            return _openFolderDialogService.ShowDialog(null, directoryName);
        }

        public DirectoryInfo Folder => ((DirectoryInfoWrapper) _openFolderDialogService.Folder).DirectoryInfo;

        public IEnumerable<DirectoryInfo> Folders =>
            _openFolderDialogService.Folders.Select(item => ((DirectoryInfoWrapper) item).DirectoryInfo);
    }
}