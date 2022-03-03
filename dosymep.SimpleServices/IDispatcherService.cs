using System;
using System.Threading.Tasks;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Предоставляет доступ к <see cref="Dispatcher"/> 
    /// </summary>
    public interface IDispatcherService {
        /// <summary>
        /// Запускает асинхронно указанный делегат.
        /// </summary>
        /// <param name="action">Выполняемый делегат.</param>
        /// <returns></returns>
        Task BeginInvoke(Action action);
        
        /// <summary>
        /// Запускает указанный делегат.
        /// </summary>
        /// <param name="action">Выполняемый делегат.</param>
        void Invoke(Action action);
    }
}