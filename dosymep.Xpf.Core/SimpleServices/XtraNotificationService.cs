using System;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;

using INotification = dosymep.SimpleServices.INotification;
using INotificationService = dosymep.SimpleServices.INotificationService;

namespace dosymep.Xpf.Core.SimpleServices
{
    public class XtraNotificationService : INotificationService {
        private readonly NotificationService _notificationService
            = new NotificationService() {
                ApplicationId = "dosymep",
                UseWin8NotificationsIfAvailable = false,
                Sound = PredefinedSound.Notification_Default,
                CustomNotificationVisibleMaxCount = 5,
                CustomNotificationPosition = NotificationPosition.BottomRight,
                CustomNotificationScreen = NotificationScreen.ApplicationWindow,
                PredefinedNotificationDuration = PredefinedNotificationDuration.Default,
                PredefinedNotificationTemplate = NotificationTemplate.ShortHeaderAndLongText
            };
        
        public XtraNotificationService() { }

        public XtraNotificationService(string applicationId) {
            _notificationService.ApplicationId = applicationId;
        }

        public INotification CreateNotification(string title, string body, string footer, ImageSource imageSource = null)
        {
            var notification = _notificationService.CreatePredefinedNotification(title, body, footer, imageSource);
            return new XtraNotification(notification);
        }
        
        private class XtraNotification : INotification
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
}