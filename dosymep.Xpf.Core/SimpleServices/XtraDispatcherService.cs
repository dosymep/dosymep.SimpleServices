using System;
using System.Threading.Tasks;
using System.Windows;

using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.UI.Interactivity;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Класс сервиса диспетчера окна.
    /// </summary>
    public class XtraDispatcherService : XtraBaseWindowService<DispatcherService>, IDispatcherService {
        /// <summary>
        /// Создает экземпляр сервиса диспетчера окна.
        /// </summary>
        public XtraDispatcherService()
            : base(new DispatcherService()) {
        }
        
        /// <inheritdoc />
        public void Invoke(Action action) {
            _serviceBase.Invoke(action);
        }
        
        /// <inheritdoc />
        public Task BeginInvoke(Action action) {
            return _serviceBase.BeginInvoke(action);
        }
    }
}