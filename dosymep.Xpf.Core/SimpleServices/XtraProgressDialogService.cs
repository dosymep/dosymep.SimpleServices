using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Interop;

using dosymep.SimpleServices;
using dosymep.Xpf.Core.Windows;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Класс сервиса прогресс диалога.
    /// </summary>
    public class XtraProgressDialogService : IProgressDialogService, IAttachableService {
        private XtraProgressWindow _xtraProgressWindow;
        private readonly WindowInteropHelper _windowInteropHelper;

        /// <summary>
        /// Создает экземпляр класса сервиса прогресс диалога.
        /// </summary>
        public XtraProgressDialogService() {
            _xtraProgressWindow = new XtraProgressWindow();
            _windowInteropHelper = new WindowInteropHelper(_xtraProgressWindow) {
                Owner = Process.GetCurrentProcess().MainWindowHandle
            };
        }

        /// <summary>
        /// Сервис по получению тем.
        /// </summary>
        public IUIThemeService UIThemeService {
            get => _xtraProgressWindow.UIThemeService;
            set => _xtraProgressWindow.UIThemeService = value;
        }

        /// <summary>
        /// Сервис по установке тем.
        /// </summary>
        public IUIThemeUpdaterService UIThemeUpdaterService {
            get => _xtraProgressWindow.UIThemeUpdaterService;
            set => _xtraProgressWindow.UIThemeUpdaterService = value;
        }

        /// <inheritdoc />
        public void Dispose() {
            _xtraProgressWindow.Dispose();
        }

        /// <inheritdoc />
        public bool Indeterminate {
            get => _xtraProgressWindow.Indeterminate;
            set => _xtraProgressWindow.Indeterminate = value;
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
        
        /// <inheritdoc />
        public void Close() {
            _xtraProgressWindow.Close();
        }

        /// <inheritdoc />
        public void Show() {
            _xtraProgressWindow.Show();
        }

        /// <inheritdoc />
        public void ShowDialog() {
            _xtraProgressWindow.ShowDialog();
        }

        /// <inheritdoc />
        public bool IsAttached => AssociatedObject != null;

        /// <inheritdoc />
        public bool AllowAttach { get; set; } = true;
        
        /// <inheritdoc />
        public DependencyObject AssociatedObject => _xtraProgressWindow.Owner;
        
        /// <inheritdoc />
        public void Detach() {
            if(AllowAttach) {
                _xtraProgressWindow.ResetOwnerWindowStyle();
                _xtraProgressWindow.Owner = null;
            }
        }

        /// <inheritdoc />
        public void Attach(DependencyObject dependencyObject) {
            if(AllowAttach) {
                _xtraProgressWindow.Owner = (Window) dependencyObject;
                _xtraProgressWindow.SetOwnerWindowStyle();
            }
        }
    }
}