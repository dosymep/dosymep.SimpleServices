using System.Windows.Input;

namespace dosymep.Xpf.Core.Commands {
    public abstract class XtraBaseCommand : DevExpress.Mvvm.CommandBase {

    }
    
    public abstract class XtraBaseCommand<T> : DevExpress.Mvvm.CommandBase<T> {
        public override void Execute(T parameter) {
            ExecuteImpl(parameter);
        }

        protected abstract void ExecuteImpl(T parameter);
    }
}