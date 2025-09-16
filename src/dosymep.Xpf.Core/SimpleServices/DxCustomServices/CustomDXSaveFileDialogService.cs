using DevExpress.Utils.CommonDialogs;
using DevExpress.Xpf.Dialogs;

using dosymep.SimpleServices;
using dosymep.Xpf.Core.SimpleServices.DxCustomServices.DXCustomDialogs;

namespace dosymep.Xpf.Core.SimpleServices.DxCustomServices {
    /// <summary>
    /// Internal use only.
    /// </summary>
    public class CustomDXSaveFileDialogService : DXSaveFileDialogService {
        /// <summary>
        /// Сервис по получению тем.
        /// </summary>
        public IUIThemeService UIThemeService { get; set; }

        /// <summary>
        /// Сервис по установке тем.
        /// </summary>
        public IUIThemeUpdaterService UIThemeUpdaterService { get; set; }

        /// <summary>
        /// Internal use only.
        /// </summary>
        protected override IFileDialog CreateFileDialogAdapter() =>
            new CustomDXSaveFileDialog(() => UIThemeService, () => UIThemeUpdaterService);
    }
}