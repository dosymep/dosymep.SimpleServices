using System;
using System.Threading.Tasks;

namespace dosymep.SimpleServices
{
    public interface IDispatcherService
    {
        Task BeginInvoke(Action action);
        void Invoke(Action action);
    }
}