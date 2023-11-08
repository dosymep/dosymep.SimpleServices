using System;
using System.Windows;

using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Grid;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Абстрактный класс сервиса окна.
    /// </summary>
    public abstract class XtraBaseWindowService<TServiceBase> : IAttachableService
        where TServiceBase : ServiceBase {

        /// <summary>
        /// Привязанный сервис к окну.
        /// </summary>
        protected readonly TServiceBase _serviceBase;

        /// <summary>
        /// Создает экземпляр сервиса окна.
        /// </summary>
        /// <param name="serviceBase">Привязываемый сервис к окну.</param>
        protected XtraBaseWindowService(TServiceBase serviceBase) {
            _serviceBase = serviceBase;
        }

        /// <inheritdoc />
        public bool IsAttached => _serviceBase.IsAttached;

        /// <inheritdoc />
        public bool AllowAttach { get; set; } = true;
        
        /// <inheritdoc />
        public DependencyObject AssociatedObject => _serviceBase.AssociatedObject;
        
        /// <inheritdoc />
        public void Detach() {
            if(AllowAttach) {
                _serviceBase.Detach();
            }
        }

        /// <inheritdoc />
        public void Attach(DependencyObject dependencyObject) {
            if(AllowAttach) {
                _serviceBase.Attach(dependencyObject);
            }
        }
    }
}