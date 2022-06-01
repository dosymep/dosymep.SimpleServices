using System;
using System.Threading;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Интерфейс прогресс диалога.
    /// </summary>
    public interface IProgressDialogService : IDisposable {
        /// <summary>
        /// Максимальное значение прогресса.
        /// </summary>
        int MaxValue { get; set; }

        /// <summary>
        /// Шаг обновления значение прогресса.
        /// </summary>
        int StepValue { get; set; }

        /// <summary>
        /// Строка форматирования отображения.
        /// </summary>
        string DisplayTitleFormat { get; set; }

        /// <summary>
        /// Создает класс прогресса.
        /// </summary>
        /// <returns>Возвращает прогресс.</returns>
        IProgress<int> CreateProgress();

        /// <summary>
        /// Создает класс прогресса.
        /// </summary>
        /// <returns>Возвращает прогресс.</returns>
        IProgress<int> CreateAsyncProgress();

        /// <summary>
        /// Возвращает токен отмены.
        /// </summary>
        /// <returns>Возвращает токен отмены.</returns>
        CancellationToken CreateCancellationToken();
    }
}