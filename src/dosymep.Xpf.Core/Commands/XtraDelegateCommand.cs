using System;

namespace dosymep.Xpf.Core.Commands {
    public abstract class XtraDelegateCommand : DevExpress.Mvvm.DelegateCommand {
        public XtraDelegateCommand(Action executeMethod)
            : base(executeMethod) {
        }

        public XtraDelegateCommand(Action executeMethod, bool useCommandManager)
            : base(executeMethod, useCommandManager) {
        }

        public XtraDelegateCommand(Action executeMethod, Func<bool> canExecuteMethod, bool? useCommandManager = null)
            : base(executeMethod, canExecuteMethod, useCommandManager) {
        }
    }

    public abstract class XtraDelegateCommand<T> : DevExpress.Mvvm.DelegateCommand<T> {
        public XtraDelegateCommand(Action<T> executeMethod)
            : base(executeMethod) {
        }

        public XtraDelegateCommand(Action<T> executeMethod, bool useCommandManager)
            : base(executeMethod, useCommandManager) {
        }

        public XtraDelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod, bool? useCommandManager = null)
            : base(executeMethod, canExecuteMethod, useCommandManager) {
        }
    }
}