using System;
using System.Threading;
using System.Windows;

using dosymep.SimpleServices;
using dosymep.Xpf.Core.Windows;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Класс сервиса прогресс диалога.
    /// </summary>
    public class XtraProgressDialogService : IProgressDialogService {
        private readonly XtraProgressWindow _xtraProgressWindow;

        /// <summary>
        /// Создает экземпляр класса сервиса прогресс диалога.
        /// </summary>
        /// <param name="window"></param>
        public XtraProgressDialogService(Window window) {
            _xtraProgressWindow = new XtraProgressWindow();
            _xtraProgressWindow.Owner = window;
        }

        /// <inheritdoc />
        public void Dispose() {
            _xtraProgressWindow.Dispose();
        }

        /// <inheritdoc />
        public int MaxValue {
            get => _xtraProgressWindow.MaxValue;
            set => _xtraProgressWindow.MaxValue = value;
        }

        /// <inheritdoc />
        public int StepValue {
            get => _xtraProgressWindow.StepValue;
            set => _xtraProgressWindow.StepValue = value;
        }

        /// <inheritdoc />
        public string DisplayTitleFormat {
            get => _xtraProgressWindow.DisplayTitleFormat;
            set => _xtraProgressWindow.DisplayTitleFormat = value;
        }

        /// <inheritdoc />
        public IProgress<int> CreateProgress() {
            return _xtraProgressWindow.CreateProgress();
        }

        /// <inheritdoc />
        public IProgress<int> CreateAsyncProgress() {
            return _xtraProgressWindow.CreateAsyncProgress();
        }

        /// <inheritdoc />
        public CancellationToken CreateCancellationToken() {
            return _xtraProgressWindow.CreateCancellationToken();
        }
    }
}