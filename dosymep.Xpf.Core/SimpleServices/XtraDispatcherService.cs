using System;
using System.Threading.Tasks;
using DevExpress.Mvvm.UI;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.SimpleServices
{
    public class XtraDispatcherService : IDispatcherService
    {
        private readonly DispatcherService _dispatcherService
            = new DispatcherService();

        public Task BeginInvoke(Action action)
        {
            return _dispatcherService.BeginInvoke(action);
        }

        public void Invoke(Action action)
        {
            _dispatcherService.Invoke(action);
        }
    }
}