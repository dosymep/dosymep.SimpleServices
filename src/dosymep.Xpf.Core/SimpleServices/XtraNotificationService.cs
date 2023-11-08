using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;

using dosymep.SimpleServices;

using INotification = dosymep.SimpleServices.INotification;
using INotificationService = dosymep.SimpleServices.INotificationService;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Класс сервиса уведомлений
    /// </summary>
    public class XtraNotificationService : XtraBaseWindowService<NotificationService>, INotificationService {
        private UIThemes? _oldTheme;
        private IUIThemeService _uiThemeService;

        private string DataTemplate =>
            "<DataTemplate DataType=\"dosymep.Xpf.Core.SimpleServices:CustomNotificationViewModel\""
            + " xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\""
            + " xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\""
            + " xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\""
            + " xmlns:dx=\"http://schemas.devexpress.com/winfx/2008/xaml/core\">"
            + $"<dx:BackgroundPanel dx:ThemeManager.ThemeName=\"{GetThemeName()}\" x:Name=\"_rootElement\">"
            + "<Grid Margin=\"5\">"
            + "<Grid.ColumnDefinitions>"
            + "<ColumnDefinition Width=\"Auto\" />"
            + "<ColumnDefinition />"
            + "</Grid.ColumnDefinitions>"
            + "<Grid.RowDefinitions>"
            + "<RowDefinition Height=\"Auto\"/>"
            + "<RowDefinition Height=\"*\"/>"
            + "<RowDefinition Height=\"Auto\"/>"
            + "<RowDefinition Height=\"Auto\"/>"
            + "</Grid.RowDefinitions>"
            + "<dxe:TextEdit Grid.Column=\"1\" Grid.Row=\"0\" FontSize=\"16\" IsReadOnly=\"True\""
            + " EditMode=\"InplaceActive\" HorizontalAlignment=\"Left\""
            + " EditValue=\"{Binding Title}\" FontWeight=\"Bold\" />"
            + "<dxe:TextEdit Grid.Column=\"1\" Grid.Row=\"1\" Margin=\"0\" Padding=\"0\" IsReadOnly=\"True\" FontSize=\"13\""
            + " EditMode=\"InplaceActive\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\"  TextWrapping=\"Wrap\" VerticalContentAlignment=\"Top\""
            + " EditValue=\"{Binding Body}\" />"
            + "<dxe:TextEdit Grid.Column=\"1\" Grid.Row=\"3\" IsReadOnly=\"True\""
            + " EditMode=\"InplaceActive\" HorizontalAlignment=\"Left\" FontStyle=\"Italic\" FontSize=\"10\""
            + " EditValue=\"{Binding Footer}\" />"
            + "<dxe:TextEdit Grid.Column=\"1\" Grid.Row=\"3\" IsReadOnly=\"True\""
            + " EditMode=\"InplaceActive\" HorizontalAlignment=\"Right\" FontStyle=\"Italic\" FontSize=\"10\""
            + " EditValue=\"{Binding Author}\" />"
            + "<dxe:ImageEdit Grid.Column=\"0\" Grid.Row=\"0\" Grid.RowSpan=\"4\"" +
            " EditMode=\"InplaceActive\" Width=\"96\" IsReadOnly=\"False\" ShowMenu=\"False\" EditValue=\"{Binding ImageSource}\">"
            + "<dxe:ImageEdit.Style>"
            + "<Style TargetType=\"{x:Type dxe:ImageEdit}\">"
            + "<Style.Triggers>"
            + "<Trigger Property=\"Source\" Value=\"{x:Null}\">"
            + "<Setter Property=\"Visibility\" Value=\"Collapsed\"></Setter>"
            + "</Trigger>"
            + "</Style.Triggers>"
            + "</Style>"
            + "</dxe:ImageEdit.Style>"
            + "</dxe:ImageEdit>"
            + "	</Grid>"
            + "</dx:BackgroundPanel>"
            + "</DataTemplate>";

        private static string FatalStyleXaml => "<Style x:Key=\"_style\" TargetType=\"dxe:TextEdit\""
                                                + " xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\""
                                                + " xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\""
                                                + " xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\" xmlns:dx=\"http://schemas.devexpress.com/winfx/2008/xaml/core\">"
                                                + "<Setter Property=\"Foreground\" Value=\"Brown\" />"
                                                + "</Style>";

        private static string WarningStyleXaml => "<Style x:Key=\"_style\" TargetType=\"dxe:TextEdit\""
                                                  + " xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\""
                                                  + " xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\""
                                                  + " xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\" xmlns:dx=\"http://schemas.devexpress.com/winfx/2008/xaml/core\">"
                                                  + "<Setter Property=\"Foreground\" Value=\"Orange\" />"
                                                  + "</Style>";

        /// <summary>
        /// Устанавливает и получает сервис тем.
        /// </summary>
        public IUIThemeService UIThemeService { get; set; }

        /// <summary>
        /// Создает экземпляр сервиса уведомлений.
        /// </summary>
        public XtraNotificationService()
            : base(new NotificationService()) {
            _serviceBase.UseWin8NotificationsIfAvailable = false;
            _serviceBase.PredefinedNotificationDuration = PredefinedNotificationDuration.Default;
            _serviceBase.PredefinedNotificationTemplate = NotificationTemplate.ShortHeaderAndLongText;
        }
        
        /// <summary>
        /// Идентификатор приложения.
        /// </summary>
        public string ApplicationId {
            get => _serviceBase.ApplicationId;
            set => _serviceBase.ApplicationId = value;
        }
        
        /// <summary>
        /// Значение автора по умолчанию.
        /// </summary>
        public string DefaultAutor { get; set; }

        /// <summary>
        /// Значение футера по умолчанию.
        /// </summary>
        public string DefaultFooter { get; set; }
        
        /// <summary>
        /// Значение изображения по умолчанию.
        /// </summary>
        public ImageSource DefaultImage { get; set; }

        /// <summary>
        /// Предопределенные звуки уведомления.
        /// </summary>
        public PredefinedSound Sound {
            get => _serviceBase.Sound;
            set => _serviceBase.Sound = value;
        }
        
        /// <summary>
        /// Определяет на каком мониторе будет отображаться уведомление.
        /// </summary>
        public NotificationScreen NotificationScreen {
            get => _serviceBase.CustomNotificationScreen;
            set => _serviceBase.CustomNotificationScreen = value;
        }

        /// <summary>
        /// Позиция уведомления на экране.
        /// </summary>
        public NotificationPosition NotificationPosition {
            get => _serviceBase.CustomNotificationPosition;
            set => _serviceBase.CustomNotificationPosition = value;
        }
        
        /// <summary>
        /// Максимальное количество отображаемых уведомлений на экране.
        /// </summary>
        public int NotificationVisibleMaxCount {
            get => _serviceBase.CustomNotificationVisibleMaxCount;
            set => _serviceBase.CustomNotificationVisibleMaxCount = value;
        }
        
        /// <inheritdoc />
        public INotification CreateNotification(string title, string body, string footer = null, string author = null,
            ImageSource imageSource = null) {
            return CreatePredefinedNotification(title, body, footer, author, imageSource);
        }

        /// <inheritdoc />
        public INotification CreateFatalNotification(string title, string body, string author = null,
            ImageSource imageSource = null) {
            return CreatePredefinedNotification("Ошибка", body, title, author, imageSource);
        }

        /// <inheritdoc />
        public INotification CreateWarningNotification(string title, string body, string author = null,
            ImageSource imageSource = null) {
            return CreatePredefinedNotification("Предупреждение", body, title, author, imageSource);
        }

        private XtraNotification CreatePredefinedNotification(string title, string body,
            string footer = null, string author = null, ImageSource imageSource = null) {
            var viewModel = new CustomNotificationViewModel() {
                Title = title
                    .Replace(Environment.NewLine, " ")
                    .Replace("\n", " "),
                Footer = footer?
                    .Replace(Environment.NewLine, " ")
                    .Replace("\n", " ") ?? DefaultFooter,
                Body = body,
                Author = author ?? DefaultAutor,
                ImageSource = imageSource ?? DefaultImage,
            };
            
            UpdateDataTemplate();
            return new XtraNotification(_serviceBase.CreateCustomNotification(viewModel));
        }

        private void UpdateDataTemplate() {
            if(UIThemeService.HostTheme != _oldTheme) {
                _oldTheme = UIThemeService.HostTheme;
                _serviceBase.CustomNotificationTemplate
                    = (DataTemplate) XamlReader.Parse(DataTemplate);
            }
        }

        private string GetThemeName() {
            if(UIThemeService.HostTheme == UIThemes.Dark) {
                return Theme.Win10DarkName;
            } else if(UIThemeService.HostTheme == UIThemes.Light) {
                return Theme.Win10LightName;
            }

            return Theme.Win10LightName;
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
                NotificationResult result = await _notification.ShowAsync();
                if(result == NotificationResult.UserCanceled) {
                    return false;
                }

                if(result == NotificationResult.Activated) {
                    return true;
                }

                return default;
            }

            public Task<bool?> ShowAsync(int millisecond) {
                return ShowAsync(TimeSpan.FromMilliseconds(millisecond));
            }

            public async Task<bool?> ShowAsync(TimeSpan interval) {
                DispatcherTimer timer = new DispatcherTimer {Interval = interval};
                timer.Tick += (s, e) => Hide();

                timer.Start();
                try {
                    return await ShowAsync();
                } finally {
                    timer.Stop();
                }
            }
        }
    }

    internal class CustomNotificationViewModel {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Footer { get; set; }
        public string Author { get; set; }
        public ImageSource ImageSource { get; set; }
    }
}