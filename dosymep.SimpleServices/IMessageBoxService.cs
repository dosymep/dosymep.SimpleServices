using System.Windows;

namespace dosymep.SimpleServices
{
    public interface IMessageBoxService
    {
        MessageBoxResult Show(string messageBoxText, string caption,
            MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult);
    }
}