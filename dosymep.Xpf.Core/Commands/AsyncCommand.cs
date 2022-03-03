using System;
using System.Threading.Tasks;

namespace dosymep.Xpf.Core.Commands {
    public class AsyncCommand : DevExpress.Mvvm.AsyncCommand {
        public AsyncCommand(Func<Task> executeMethod)
            : base(executeMethod) {
        }

        public AsyncCommand(Func<Task> executeMethod, bool useCommandManager)
            : base(executeMethod, useCommandManager) {
        }

        public AsyncCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod, bool? useCommandManager = null)
            : base(executeMethod, canExecuteMethod, useCommandManager) {
        }

        public AsyncCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod, bool allowMultipleExecution,
            bool? useCommandManager = null)
            : base(executeMethod, canExecuteMethod, allowMultipleExecution, useCommandManager) {
        }
    }

    public class AsyncCommand<T> : DevExpress.Mvvm.AsyncCommand<T> {
        public AsyncCommand(Func<T, Task> executeMethod) : base(executeMethod) {
        }

        public AsyncCommand(Func<T, Task> executeMethod, bool useCommandManager)
            : base(executeMethod, useCommandManager) {
        }

        public AsyncCommand(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod, bool? useCommandManager = null)
            : base(executeMethod, canExecuteMethod, useCommandManager) {
        }

        public AsyncCommand(Func<T, Task> executeMethod,
            Func<T, bool> canExecuteMethod, bool allowMultipleExecution, bool? useCommandManager = null)
            : base(executeMethod, canExecuteMethod, allowMultipleExecution, useCommandManager) {
        }
    }
}