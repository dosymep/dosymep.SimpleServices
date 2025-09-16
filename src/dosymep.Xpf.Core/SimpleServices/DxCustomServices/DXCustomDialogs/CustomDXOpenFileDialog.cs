using System;
using System.Windows;

using DevExpress.Xpf.Core;
using DevExpress.Xpf.Dialogs;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.SimpleServices.DxCustomServices.DXCustomDialogs {
    internal class CustomDXOpenFileDialog : DXOpenFileDialog {
        private readonly Func<IUIThemeService> _theme;
        private readonly Func<IUIThemeUpdaterService> _themeUpdaterService;

        public CustomDXOpenFileDialog(Func<IUIThemeService> theme, Func<IUIThemeUpdaterService> themeUpdaterService) {
            _theme = theme;
            _themeUpdaterService = themeUpdaterService;
        }

        protected override IDialogHost CreateDialogHost(IntPtr hwndOwner) {
            FileDialogWindow fileDialogWindow = new FileDialogWindow {
                Title = string.IsNullOrEmpty(Title) ? GetDefaultTitle() : Title,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                WindowStyle = WindowStyle.ToolWindow,
                ShowInTaskbar = false
            };
            
            _themeUpdaterService().SetTheme(_theme().HostTheme, fileDialogWindow);

            return fileDialogWindow;
        }
    }
}