using System.Windows;
using System.Windows.Media;

using DevExpress.Xpf.Core;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Класс сервиса окна сообщений.
    /// </summary>
    public class XtraMessageBoxService : IMessageBoxService, IAttachableService {
        /// <summary>
        /// Сервис по получению тем.
        /// </summary>
        public IUIThemeService UIThemeService { get;  set; }

        /// <summary>
        /// Сервис по установке тем.
        /// </summary>
        public IUIThemeUpdaterService UIThemeUpdaterService { get; set; }

        /// <inheritdoc />
        public MessageBoxResult Show(string messageBoxText, string caption,
            MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult) {
            if(AssociatedObject != null) {
                var window = GetWindow();
                if(window.IsVisible) {
                    return ThemedMessageBox.Show(
                        window, caption,
                        messageBoxText, button,
                        defaultResult, icon,
                        showActivated: false);
                }
            }

            Window owner = CreateWindow();
            try {
                owner.Show();
                return ThemedMessageBox.Show(
                    owner, caption,
                    messageBoxText, button,
                    defaultResult, icon,
                    showActivated: false);
            } finally {
                owner.Close();
            }
        }

        #region IAttacheableService

        /// <inheritdoc />
        public bool IsAttached => AssociatedObject != null;

        /// <inheritdoc />
        public bool AllowAttach { get; set; }

        /// <inheritdoc />
        public DependencyObject AssociatedObject { get; private set; }

        /// <inheritdoc />
        public void Attach(DependencyObject dependencyObject) {
            if(AllowAttach) {
                AssociatedObject = dependencyObject;
            }
        }

        /// <inheritdoc />
        public void Detach() {
            AssociatedObject = null;
        }

        #endregion
        
        private Window GetWindow() {
            if(AssociatedObject == null) {
                return null;
            }
            
            return AssociatedObject is Window window
                ? window
                : Window.GetWindow(AssociatedObject);
        }

        private Window CreateWindow() {
            Window window = new Window {
                ShowActivated = false,
                ShowInTaskbar = false,
                AllowsTransparency = true,
                WindowStyle = WindowStyle.None,
                WindowState = WindowState.Minimized,
                Background = Brushes.Transparent
            };

            UIThemeUpdaterService.SetTheme(UIThemeService.HostTheme, window);
            return window;
        }
    }
}