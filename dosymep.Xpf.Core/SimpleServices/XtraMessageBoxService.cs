using System;
using System.Windows;
using DevExpress.Mvvm;
using DevExpress.Pdf;
using DevExpress.Xpf.Core;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Класс сервиса окна сообщений.
    /// </summary>
    public class XtraMessageBoxService :
        XtraBaseWindowService<DXMessageBoxService>,
        dosymep.SimpleServices.IMessageBoxService {

        /// <summary>
        /// Создает экземпляр класса сервиса окна сообщений.
        /// </summary>
        /// <param name="window">Родительское окно сервиса.</param>
        public XtraMessageBoxService(Window window)
            : base(window, new DXMessageBoxService()) {
            _serviceBase.SetMessageBoxOwner = true;
        }
        
        /// <inheritdoc />
        public MessageBoxResult Show(string messageBoxText, string caption,
            MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult) {
            return _serviceBase.Show(messageBoxText, caption, button, icon, defaultResult);
        }
    }
}