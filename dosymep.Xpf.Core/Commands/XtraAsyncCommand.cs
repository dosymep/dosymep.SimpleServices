using System;
using System.Threading.Tasks;

namespace dosymep.Xpf.Core.Commands {
    public class XtraAsyncCommand : DevExpress.Mvvm.AsyncCommand {
        public XtraAsyncCommand(Func<Task> executeMethod)
            : base(executeMethod) {
        }

        public XtraAsyncCommand(Func<Task> executeMethod, bool useCommandManager)
            : base(executeMethod, useCommandManager) {
        }

        public XtraAsyncCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod, bool? useCommandManager = null)
            : base(executeMethod, canExecuteMethod, useCommandManager) {
        }

        public XtraAsyncCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod, bool allowMultipleExecution,
            bool? useCommandManager = null)
            : base(executeMethod, canExecuteMethod, allowMultipleExecution, useCommandManager) {
        }
    }

    public class XtraAsyncCommand<T> : DevExpress.Mvvm.AsyncCommand<T> {
        public XtraAsyncCommand(Func<T, Task> executeMethod) : base(executeMethod) {
        }

        public XtraAsyncCommand(Func<T, Task> executeMethod, bool useCommandManager)
            : base(executeMethod, useCommandManager) {
        }

        public XtraAsyncCommand(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod, bool? useCommandManager = null)
            : base(executeMethod, canExecuteMethod, useCommandManager) {
        }

        public XtraAsyncCommand(Func<T, Task> executeMethod,
            Func<T, bool> canExecuteMethod, bool allowMultipleExecution, bool? useCommandManager = null)
            : base(executeMethod, canExecuteMethod, allowMultipleExecution, useCommandManager) {
        }
    }
}