using System;
using System.Threading.Tasks;
using System.Windows.Threading;

using DevExpress.Mvvm;

using INotification = dosymep.SimpleServices.INotification;

namespace dosymep.Xpf.Core.SimpleServices
{
    internal class XtraNotification : INotification
    {
        private readonly DevExpress.Mvvm.INotification _notification;

        public XtraNotification(DevExpress.Mvvm.INotification notification)
        {
            _notification = notification;
        }

        public void Hide()
        {
            _notification.Hide();
        }

        public async Task<bool?> ShowAsync()
        {
            var result = await _notification.ShowAsync();
            if (result == NotificationResult.UserCanceled)
            {
                return false;
            }

            return true;
        }

        public Task<bool?> ShowAsync(int millisecond)
        {
            return ShowAsync(TimeSpan.FromMilliseconds(millisecond));
        }

        public async Task<bool?> ShowAsync(TimeSpan interval)
        {
            var timer = new DispatcherTimer();
            timer.Interval = interval;
            timer.Tick += (s, e) => Hide();
            timer.Start();
            
            var result = await ShowAsync();
            timer.Stop();

            return result;
        }
    }
}