using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Dialogs;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.SimpleServices
{
    public class XtraOpenFileDialogService : IOpenFileDialogService
    {
        private readonly DevExpress.Mvvm.IOpenFileDialogService _openFileDialogService
            = new DXOpenFileDialogService();
        
        public bool Multiselect
        {
            get => _openFileDialogService.Multiselect;
            set => _openFileDialogService.Multiselect = value;
        }

        public bool CheckFileExists
        {
            get => _openFileDialogService.CheckFileExists;
            set => _openFileDialogService.CheckFileExists = value;
        }

        public bool AddExtension
        {
            get => _openFileDialogService.AddExtension;
            set => _openFileDialogService.AddExtension = value;
        }

        public bool AutoUpgradeEnabled
        {
            get => _openFileDialogService.AutoUpgradeEnabled;
            set => _openFileDialogService.AutoUpgradeEnabled = value;
        }

        public bool CheckPathExists
        {
            get => _openFileDialogService.CheckPathExists;
            set => _openFileDialogService.CheckPathExists = value;
        }

        public bool DereferenceLinks
        {
            get => _openFileDialogService.DereferenceLinks;
            set => _openFileDialogService.DereferenceLinks = value;
        }

        public bool RestoreDirectory
        {
            get => _openFileDialogService.RestoreDirectory;
            set => _openFileDialogService.RestoreDirectory = value;
        }

        public bool ShowHelp
        {
            get => _openFileDialogService.ShowHelp;
            set => _openFileDialogService.ShowHelp = value;
        }

        public bool SupportMultiDottedExtensions
        {
            get => _openFileDialogService.SupportMultiDottedExtensions;
            set => _openFileDialogService.SupportMultiDottedExtensions = value;
        }

        public bool ValidateNames
        {
            get => _openFileDialogService.ValidateNames;
            set => _openFileDialogService.ValidateNames = value;
        }

        public int FilterIndex
        {
            get => _openFileDialogService.FilterIndex;
            set => _openFileDialogService.FilterIndex = value;
        }

        public string Title
        {
            get => _openFileDialogService.Title;
            set => _openFileDialogService.Title = value;
        }

        public string Filter
        {
            get => _openFileDialogService.Filter;
            set => _openFileDialogService.Filter = value;
        }

        public string InitialDirectory
        {
            get => _openFileDialogService.InitialDirectory;
            set => _openFileDialogService.InitialDirectory = value;
        }

        public void Reset()
        {
            _openFileDialogService.Reset();
        }

        public bool ShowDialog(string directoryName)
        {
            return _openFileDialogService.ShowDialog(null, directoryName);
        }

        public FileInfo File => ((FileInfoWrapper) _openFileDialogService.File).FileInfo;

        public IEnumerable<FileInfo> Files =>
            _openFileDialogService.Files.Select(item => ((FileInfoWrapper) item).FileInfo);
    }
}