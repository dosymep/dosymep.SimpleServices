using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;

using INotification = dosymep.SimpleServices.INotification;
using INotificationService = dosymep.SimpleServices.INotificationService;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Класс сервиса уведомлений
    /// </summary>
    public class XtraNotificationService : XtraBaseWindowService<NotificationService>, INotificationService {
        /// <summary>
        /// Создает экземпляр сервиса уведомлений.
        /// </summary>
        /// <param name="window">Родительское окно сервиса.</param>
        public XtraNotificationService(Window window)
            : base(window, new NotificationService()) {
            _serviceBase.ApplicationId = "dosymep";
            _serviceBase.UseWin8NotificationsIfAvailable = false;
            _serviceBase.Sound = PredefinedSound.Notification_Default;
            _serviceBase.CustomNotificationVisibleMaxCount = 5;
            _serviceBase.CustomNotificationPosition = NotificationPosition.BottomRight;
            _serviceBase.CustomNotificationScreen = NotificationScreen.ApplicationWindow;
            _serviceBase.PredefinedNotificationDuration = PredefinedNotificationDuration.Default;
            _serviceBase.PredefinedNotificationTemplate = NotificationTemplate.ShortHeaderAndLongText;
        }

        /// <summary>
        /// Создает экземпляр сервиса уведомлений.
        /// </summary>
        /// <param name="window">Родительское окно сервиса.</param>
        /// <param name="applicationId">Идентификатор приложения.</param>
        public XtraNotificationService(Window window, string applicationId)
            : this(window) {
            _serviceBase.ApplicationId = applicationId;
        }

        /// <inheritdoc />
        public INotification CreateNotification(string title, string body, string footer,
            ImageSource imageSource = null) {
            return CreatePredefinedNotification(title, body, footer, imageSource);
        }

        /// <inheritdoc />
        public INotification CreateFatalNotification(string title, string body, string footer = null, 
            ImageSource imageSource = null) {
            return CreateNotification(title, body, footer, imageSource);
        }

        /// <inheritdoc />
        public INotification CreateWarningNotification(string title, string body, string footer = null,
            ImageSource imageSource = null) {
            return CreateNotification(title, body, footer, imageSource);
        }

        private XtraNotification CreatePredefinedNotification(string title, string body, string footer = null,
            ImageSource imageSource = null) {
            return new XtraNotification(_serviceBase.CreatePredefinedNotification(title, body, footer, imageSource));
        }

        private class XtraNotification : INotification {
            private readonly DevExpress.Mvvm.INotification _notification;

            public XtraNotification(DevExpress.Mvvm.INotification notification) {
                _notification = notification;
            }

            public void Hide() {
                _notification.Hide();
            }

            public async Task<bool?> ShowAsync() {
                var result = await _notification.ShowAsync();
                if(result == NotificationResult.UserCanceled) {
                    return false;
                }

                return true;
            }

            public Task<bool?> ShowAsync(int millisecond) {
                return ShowAsync(TimeSpan.FromMilliseconds(millisecond));
            }

            public async Task<bool?> ShowAsync(TimeSpan interval) {
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