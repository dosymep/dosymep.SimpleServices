using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace dosymep.SimpleServices
{
    public interface INotificationService
    {
        INotification CreateNotification(string title, string body, string footer, ImageSource imageSource = null);
    }

    public interface INotification
    {
        void Hide();
        Task<bool?> ShowAsync();
        Task<bool?> ShowAsync(int millisecond);
        Task<bool?> ShowAsync(TimeSpan interval);
    }
}