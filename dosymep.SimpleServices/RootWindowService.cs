using System;
using System.Windows;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Класс текущего рабочего окна.
    /// </summary>
    public class RootWindowService : IRootWindowService {
        private Window _rootWindow;

        /// <inheritdoc />
        public Window RootWindow {
            get => _rootWindow;
            set {
                _rootWindow = value;
                if(_rootWindow != null) {
                    _rootWindow.Closed += RootWindowOnClosed;
                }
            }
        }

        private void RootWindowOnClosed(object sender, EventArgs e) {
            _rootWindow.Closed -= RootWindowOnClosed;
            _rootWindow = null;
        }
    }
}