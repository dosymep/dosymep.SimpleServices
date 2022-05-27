using System;
using System.Windows;

using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Grid;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Абстрактный класс сервиса окна.
    /// </summary>
    public abstract class XtraBaseWindowService<TServiceBase> : IDisposable
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
                Interaction.GetBehaviors(_window).Add(_serviceBase);
            }
        }

        /// <summary>
        /// Очищает привязку к окну.
        /// </summary>
        /// <param name="disposing">Указывает на очистку ресурсов.</param>
        protected virtual void Dispose(bool disposing) {
            if(disposing) {
                if(_window != null) {
                    Interaction.GetBehaviors(_window).Remove(_serviceBase);
                }
            }
        }

        /// <inheritdoc />
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}