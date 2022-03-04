﻿using System.Windows.Media;
using DevExpress.Mvvm.UI;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.SimpleServices
{
    internal class XtraNotificationService : INotificationService
    {
        private readonly NotificationService _notificationService
            = new NotificationService()
            {
                ApplicationId = "dosymep", 
                UseWin8NotificationsIfAvailable=true,
                PredefinedNotificationTemplate = NotificationTemplate.ShortHeaderAndTwoTextFields,
                PredefinedNotificationDuration = PredefinedNotificationDuration.Default,
                Sound = PredefinedSound.Notification_Default
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
    }
}