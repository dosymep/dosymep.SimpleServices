using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

using DevExpress.Mvvm;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.Windows {
    /// <summary>
    /// Класс окна прогресса.
    /// </summary>
    public partial class XtraProgressDialogService : IProgressDialogService {
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Инициализирует окно прогресс бара.
        /// </summary>
        public XtraProgressDialogService() {
            InitializeComponent();
        }

        /// <inheritdoc />
        public int MaxValue { get; set; }

        /// <inheritdoc />
        public int StepValue { get; set; }

        /// <inheritdoc />
        public string DisplayTitleFormat { get; set; } = "Progress ...";

        /// <inheritdoc />
        public IProgress<int> CreateProgress() {
            return new CustomProgress(this);
        }

        /// <inheritdoc />
        public IProgress<int> CreateAsyncProgress() {
            return new Progress<int>(UpdateWindow);
        }

        /// <summary>
        /// Возвращает токен отмены.
        /// </summary>
        /// <returns>Возвращает токен отмены.</returns>
        public CancellationToken CreateCancellationToken() {
            if(_cancellationTokenSource == null) {
                _cancelButton.Visibility = Visibility.Visible;
                _cancellationTokenSource = new CancellationTokenSource();
            }

            return _cancellationTokenSource.Token;
        }

        /// <summary>
        /// Обновляет окно.
        /// </summary>
        internal void DispatcherUpdateWindow(int currentValue) {
            Dispatcher.Invoke(() => UpdateWindow(currentValue), DispatcherPriority.Background);
        }

        /// <summary>
        /// Обновляет окно.
        /// </summary>
        internal void UpdateWindow(int currentValue) {
            if(StepValue > 0) {
                if(currentValue % StepValue == 0) {
                    UpdateWindowImpl(currentValue);
                }
            } else {
                UpdateWindowImpl(currentValue);
            }
        }

        private void UpdateWindowImpl(int currentValue) {
            _progressEdit.Maximum = MaxValue;
            _progressEdit.Value = currentValue;
            _textEdit.EditValue = string.Format(DisplayTitleFormat, currentValue, MaxValue);
        }

        private void _cancelButton_OnClick(object sender, RoutedEventArgs e) {
            _cancelButton.IsEnabled = false;
            _cancellationTokenSource?.Cancel();
        }

        #region IDispose

        ~XtraProgressDialogService() {
            Dispose(false);
        }

        /// <inheritdoc />
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Очищает подписку на событие.
        /// </summary>
        /// <param name="disposing">Указывает на очистку ресурсов.</param>
        protected virtual void Dispose(bool disposing) {
            if(disposing) {
                Close();
            }
        }

        #endregion
    }

    internal class CustomProgress : IProgress<int> {
        private readonly XtraProgressDialogService _window;

        public CustomProgress(XtraProgressDialogService window) {
            _window = window;
        }

        public void Report(int value) {
            _window.DispatcherUpdateWindow(value);
        }
    }
}