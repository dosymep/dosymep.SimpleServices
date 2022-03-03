using System;

namespace dosymep.Xpf.Core.Commands {
    public class DelegateCommand : DevExpress.Mvvm.DelegateCommand {
        public DelegateCommand(Action executeMethod)
            : base(executeMethod) {
        }

        public DelegateCommand(Action executeMethod, bool useCommandManager)
            : base(executeMethod, useCommandManager) {
        }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod, bool? useCommandManager = null)
            : base(executeMethod, canExecuteMethod, useCommandManager) {
        }
    }

    public class DelegateCommand<T> : DevExpress.Mvvm.DelegateCommand<T> {
        public DelegateCommand(Action<T> executeMethod)
            : base(executeMethod) {
        }

        public DelegateCommand(Action<T> executeMethod, bool useCommandManager)
            : base(executeMethod, useCommandManager) {
        }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod, bool? useCommandManager = null)
            : base(executeMethod, canExecuteMethod, useCommandManager) {
        }
    }
}