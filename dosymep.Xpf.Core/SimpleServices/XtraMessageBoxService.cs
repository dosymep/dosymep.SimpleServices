using System;
using System.Windows;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;

namespace dosymep.Xpf.Core.SimpleServices
{
    public class XtraMessageBoxService : dosymep.SimpleServices.IMessageBoxService
    {
        private readonly DXMessageBoxService _messageBoxService = new DXMessageBoxService();

        public MessageBoxResult Show(string messageBoxText, string caption,
            MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult) {
            return _messageBoxService.Show(messageBoxText, caption, button, icon, defaultResult);
        }
    }
}