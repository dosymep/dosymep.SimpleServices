using System.Collections.Generic;
using System.IO;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Dialogs;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.SimpleServices
{
    public class XtraSaveFileDialogService : ISaveFileDialogService
    {
        private readonly DevExpress.Mvvm.ISaveFileDialogService _saveFileDialogService
            = new DXSaveFileDialogService();

        public bool CheckFileExists
        {
            get => _saveFileDialogService.CheckFileExists;
            set => _saveFileDialogService.CheckFileExists = value;
        }

        public bool AddExtension
        {
            get => _saveFileDialogService.AddExtension;
            set => _saveFileDialogService.AddExtension = value;
        }

        public bool AutoUpgradeEnabled
        {
            get => _saveFileDialogService.AutoUpgradeEnabled;
            set => _saveFileDialogService.AutoUpgradeEnabled = value;
        }

        public bool CheckPathExists
        {
            get => _saveFileDialogService.CheckPathExists;
            set => _saveFileDialogService.CheckPathExists = value;
        }

        public bool DereferenceLinks
        {
            get => _saveFileDialogService.DereferenceLinks;
            set => _saveFileDialogService.DereferenceLinks = value;
        }

        public bool RestoreDirectory
        {
            get => _saveFileDialogService.RestoreDirectory;
            set => _saveFileDialogService.RestoreDirectory = value;
        }

        public bool ShowHelp
        {
            get => _saveFileDialogService.ShowHelp;
            set => _saveFileDialogService.ShowHelp = value;
        }

        public bool SupportMultiDottedExtensions
        {
            get => _saveFileDialogService.SupportMultiDottedExtensions;
            set => _saveFileDialogService.SupportMultiDottedExtensions = value;
        }

        public bool ValidateNames
        {
            get => _saveFileDialogService.ValidateNames;
            set => _saveFileDialogService.ValidateNames = value;
        }

        public int FilterIndex
        {
            get => _saveFileDialogService.FilterIndex;
            set => _saveFileDialogService.FilterIndex = value;
        }

        public string DefaultExt
        {
            get => _saveFileDialogService.DefaultExt;
            set => _saveFileDialogService.DefaultExt = value;
        }

        public string DefaultFileName
        {
            get => _saveFileDialogService.DefaultFileName;
            set => _saveFileDialogService.DefaultFileName = value;
        }

        public string Title
        {
            get => _saveFileDialogService.Title;
            set => _saveFileDialogService.Title = value;
        }

        public string Filter
        {
            get => _saveFileDialogService.Filter;
            set => _saveFileDialogService.Filter = value;
        }

        public string InitialDirectory
        {
            get => _saveFileDialogService.InitialDirectory;
            set => _saveFileDialogService.InitialDirectory = value;
        }

        public void Reset()
        {
            _saveFileDialogService.Reset();
        }
        
        public bool ShowDialog(string directoryName, string fileName)
        {
            return _saveFileDialogService.ShowDialog(null, directoryName, fileName);
        }

        public FileInfo File => ((FileInfoWrapper) _saveFileDialogService.File).FileInfo;
    }
}