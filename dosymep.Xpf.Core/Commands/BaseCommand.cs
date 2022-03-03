using System.Windows.Input;

namespace dosymep.Xpf.Core.Commands {
    public abstract class BaseCommand : DevExpress.Mvvm.CommandBase {

    }
    
    public abstract class BaseCommand<T> : DevExpress.Mvvm.CommandBase<T> {
        public override void Execute(T parameter) {
            ExecuteImpl(parameter);
        }

        protected abstract void ExecuteImpl(T parameter);
    }
}