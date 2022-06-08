using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Класс текущего рабочего окна.
    /// </summary>
    public class RootWindowService : IRootWindowService {
        private readonly Stack<Window> _stackWindows = new Stack<Window>();

        /// <inheritdoc />
        public Window RootWindow {
            get => _stackWindows.Count == 0
                ? null 
                : _stackWindows.Peek();
            set {
                if(value != null) {
                    _stackWindows.Push(value);
                    value.Closed += RootWindowOnClosed;
                }
            }
        }

        private void RootWindowOnClosed(object sender, EventArgs e) {
            Window rootWindow = _stackWindows.Pop();
            rootWindow.Closed -= RootWindowOnClosed;
        }
    }
}