using System;
using System.Windows;

using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Grid;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Абстрактный класс сервиса окна.
    /// </summary>
    public abstract class XtraBaseWindowService<TServiceBase>
        where TServiceBase : ServiceBase {

        /// <summary>
        /// Родительское окно сервиса
        /// </summary>
        protected readonly Window _window;

        /// <summary>
        /// Привязанный сервис к окну.
        /// </summary>
        protected readonly TServiceBase _serviceBase;

        /// <summary>
        /// Создает экземпляр сервиса окна.
        /// </summary>
        /// <param name="window">Родительское окно для сервиса.</param>
        /// <param name="serviceBase">Привязываемый сервис к окну.</param>
        protected XtraBaseWindowService(Window window, TServiceBase serviceBase) {
            _window = window;
            _serviceBase = serviceBase;

            if(_window != null) {
                _serviceBase.Attach(window);
                _window.Closed += WindowOnClosed;
            }
        }

        private void WindowOnClosed(object sender, EventArgs e) {
            _serviceBase.Detach();
        }
    }
}