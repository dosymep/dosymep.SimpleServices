using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

using DevExpress.Xpf.Editors;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.Windows {
    /// <summary>
    /// Класс окна прогресса.
    /// </summary>
    internal partial class XtraProgressWindow : IDisposable {
        private bool _indeterminate;
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Инициализирует окно прогресс бара.
        /// </summary>
        public XtraProgressWindow() {
            InitializeComponent();
        }

        public bool Indeterminate {
            get => _indeterminate;
            set {
                _indeterminate = value;
                _progressEdit.StyleSettings =
                    _indeterminate
                        ? (BaseEditStyleSettings) new ProgressBarMarqueeStyleSettings()
                        : (BaseEditStyleSettings) new ProgressBarStyleSettings();
            }
        }

        public int MaxValue { get; set; }

        public int StepValue { get; set; }

        public string DisplayTitleFormat { get; set; } = "Пожалуйста подождите ...";

        public IUIThemeService UIThemeService { get; set; }

        public IUIThemeUpdaterService UIThemeUpdaterService { get; set; }

        public IProgress<int> CreateProgress() {
            return new CustomProgress(this);
        }

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

        internal void SetOwnerWindowStyle() {
            if(Owner != null) {
                Owner.IsEnabled = false;
            }
        }

        internal void ResetOwnerWindowStyle() {
            if(Owner != null) {
                Owner.IsEnabled = true;
            }
        }

        protected override void OnSourceInitialized(EventArgs e) {
            base.OnSourceInitialized(e);
            SetTheme(UIThemeService.HostTheme);
            UIThemeService.UIThemeChanged += SetTheme;
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            ResetOwnerWindowStyle();
            
            _cancellationTokenSource?.Cancel();
            if(UIThemeService != null) {
                UIThemeService.UIThemeChanged -= SetTheme;
            }
        }

        protected void SetTheme(UIThemes uiThemes) {
            UIThemeUpdaterService?.SetTheme(uiThemes, this);
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
            ++currentValue;
            if(StepValue > 0 && currentValue < MaxValue) {
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
            _textEdit.EditValue = "Отмена операции ...";
            _cancellationTokenSource?.Cancel();
        }

        #region IDispose

        ~XtraProgressWindow() {
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
        private readonly XtraProgressWindow _window;

        public CustomProgress(XtraProgressWindow window) {
            _window = window;
        }

        public void Report(int value) {
            _window.DispatcherUpdateWindow(value);
        }
    }
}