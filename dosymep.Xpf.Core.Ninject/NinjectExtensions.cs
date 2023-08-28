using System.Windows;
using System.Windows.Media;

using DevExpress.Mvvm.UI;

using dosymep.SimpleServices;
using dosymep.Xpf.Core.SimpleServices;

using Ninject;
using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Syntax;

namespace dosymep.Xpf.Core.Ninject;

public static class NinjectExtensions {
    public static IKernel UseXtraLanguage(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<ILanguageService>()
            .To<XtraWindowsLanguageService>();

        return kernel;
    }

    public static IKernel UseXtraTheme(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IUIThemeService>()
            .To<XtraWindowsThemeService>();

        return kernel;
    }

    public static IKernel UseXtraThemeUpdater(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IUIThemeUpdaterService>()
            .To<XtraThemeUpdaterService>();

        return kernel;
    }

    public static IKernel UseXtraDispatcher(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IDispatcherService>()
            .To<XtraDispatcherService>()
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), false);

        return kernel;
    }

    public static IKernel UseXtraDispatcher<T>(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IDispatcherService>()
            .To<XtraDispatcherService>()
            .WhenInjectedInto<T>()
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), true);

        return kernel;
    }

    public static IKernel UseXtraMessageBox(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IMessageBoxService>()
            .To<XtraMessageBoxService>()
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), false);

        return kernel;
    }

    public static IKernel UseXtraMessageBox<T>(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IMessageBoxService>()
            .To<XtraMessageBoxService>()
            .WhenInjectedInto<T>()
            .WithPropertyValue(nameof(XtraProgressDialogService.UIThemeService),
                c => c.Kernel.Get<IUIThemeService>())
            .WithPropertyValue(nameof(XtraProgressDialogService.UIThemeUpdaterService),
                c => c.Kernel.Get<IUIThemeUpdaterService>())
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), true);

        return kernel;
    }

    public static IKernel UseXtraProgressDialog(this IKernel kernel,
        int stepValue = 10,
        bool indeterminate = false,
        string displayTitleFormat = "Пожалуйста подождите [{0}/{1}] ...") {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IProgressDialogService>()
            .To<XtraProgressDialogService>()
            .WithPropertyValue(nameof(XtraProgressDialogService.UIThemeService),
                c => c.Kernel.Get<IUIThemeService>())
            .WithPropertyValue(nameof(XtraProgressDialogService.UIThemeUpdaterService),
                c => c.Kernel.Get<IUIThemeUpdaterService>())
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), false)
            .WithPropertyValue(nameof(XtraProgressDialogService.DisplayTitleFormat), displayTitleFormat)
            .WithPropertyValue(nameof(XtraProgressDialogService.StepValue), stepValue)
            .WithPropertyValue(nameof(XtraProgressDialogService.Indeterminate), indeterminate);

        return kernel;
    }

    public static IKernel UseXtraProgressDialog<T>(this IKernel kernel,
        int stepValue = 10,
        bool indeterminate = false,
        string displayTitleFormat = "Пожалуйста подождите [{0}/{1}] ...") {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }
        
        kernel.Bind<IProgressDialogFactory>()
            .To<ProgressDialogFactory>()
            .WhenInjectedInto<T>()
            .WithPropertyValue(nameof(XtraProgressDialogService.UIThemeService),
                c => c.Kernel.Get<IUIThemeService>())
            .WithPropertyValue(nameof(XtraProgressDialogService.UIThemeUpdaterService),
                c => c.Kernel.Get<IUIThemeUpdaterService>())
            .WithPropertyValue(nameof(XtraProgressDialogService.DisplayTitleFormat), displayTitleFormat)
            .WithPropertyValue(nameof(XtraProgressDialogService.StepValue), stepValue)
            .WithPropertyValue(nameof(XtraProgressDialogService.Indeterminate), indeterminate);

        return kernel;
    }

    public static IKernel UseXtraNotifications(this IKernel kernel,
        string? applicationId = default,
        string? defaultAuthor = default,
        string? defaultFooter = default,
        ImageSource? defaultImage = default,
        PredefinedSound sound = PredefinedSound.NoSound,
        NotificationScreen notificationScreen = NotificationScreen.Primary,
        NotificationPosition notificationPosition = NotificationPosition.BottomRight,
        int notificationVisibleMaxCount = 5) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<INotificationService>()
            .To<XtraNotificationService>()
            .WithPropertyValue(nameof(XtraProgressDialogService.UIThemeService),
                c => c.Kernel.Get<IUIThemeService>())
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), false)
            .WithPropertyValue(nameof(XtraNotificationService.ApplicationId), applicationId!)
            .WithPropertyValue(nameof(XtraNotificationService.DefaultAutor), defaultAuthor!)
            .WithPropertyValue(nameof(XtraNotificationService.DefaultFooter), defaultFooter!)
            .WithPropertyValue(nameof(XtraNotificationService.DefaultImage), defaultImage!)
            .WithPropertyValue(nameof(XtraNotificationService.Sound), sound)
            .WithPropertyValue(nameof(XtraNotificationService.NotificationScreen), notificationScreen)
            .WithPropertyValue(nameof(XtraNotificationService.NotificationPosition), notificationPosition)
            .WithPropertyValue(nameof(XtraNotificationService.NotificationVisibleMaxCount),
                notificationVisibleMaxCount);

        return kernel;
    }

    public static IKernel UseXtraNotifications<T>(this IKernel kernel,
        string? applicationId = default,
        string? defaultAuthor = default,
        string? defaultFooter = default,
        ImageSource? defaultImage = default,
        PredefinedSound sound = PredefinedSound.NoSound,
        NotificationScreen notificationScreen = NotificationScreen.ApplicationWindow,
        NotificationPosition notificationPosition = NotificationPosition.BottomRight,
        int notificationVisibleMaxCount = 5) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<INotificationService>()
            .To<XtraNotificationService>()
            .WhenInjectedInto<T>()
            .WithPropertyValue(nameof(XtraProgressDialogService.UIThemeService),
                c => c.Kernel.Get<IUIThemeService>())
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), true)
            .WithPropertyValue(nameof(XtraNotificationService.ApplicationId), applicationId!)
            .WithPropertyValue(nameof(XtraNotificationService.DefaultAutor), defaultAuthor!)
            .WithPropertyValue(nameof(XtraNotificationService.DefaultFooter), defaultFooter!)
            .WithPropertyValue(nameof(XtraNotificationService.DefaultImage), defaultImage!)
            .WithPropertyValue(nameof(XtraNotificationService.Sound), sound)
            .WithPropertyValue(nameof(XtraNotificationService.NotificationScreen), notificationScreen)
            .WithPropertyValue(nameof(XtraNotificationService.NotificationPosition), notificationPosition)
            .WithPropertyValue(nameof(XtraNotificationService.NotificationVisibleMaxCount),
                notificationVisibleMaxCount);

        return kernel;
    }

    public static IKernel UseXtraOpenFileDialog(this IKernel kernel,
        string? title = "Выбрать файл",
        string? filter = "Все файлы (*.*)|*.*",
        int filterIndex = 0,
        string? initialDirectory = default,
        bool multiSelect = false,
        bool addExtension = true,
        bool autoUpgradeEnabled = true,
        bool checkFileExists = true,
        bool checkPathExists = true,
        bool validateNames = true,
        bool dereferenceLinks = true,
        bool restoreDirectory = false,
        bool showHelp = false,
        bool supportMultiDottedExtensions = false) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IOpenFileDialogService>()
            .To<XtraOpenFileDialogService>()
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), false)
            .WithPropertyValue(nameof(IOpenDialogServiceBase.Multiselect), multiSelect)
            .WithPropertyValue(nameof(IFileDialogServiceBase.AddExtension), addExtension)
            .WithPropertyValue(nameof(IFileDialogServiceBase.AutoUpgradeEnabled), autoUpgradeEnabled)
            .WithPropertyValue(nameof(IFileDialogServiceBase.CheckFileExists), checkFileExists)
            .WithPropertyValue(nameof(IFileDialogServiceBase.CheckPathExists), checkPathExists)
            .WithPropertyValue(nameof(IFileDialogServiceBase.ValidateNames), validateNames)
            .WithPropertyValue(nameof(IFileDialogServiceBase.DereferenceLinks), dereferenceLinks)
            .WithPropertyValue(nameof(IFileDialogServiceBase.RestoreDirectory), restoreDirectory)
            .WithPropertyValue(nameof(IFileDialogServiceBase.ShowHelp), showHelp)
            .WithPropertyValue(nameof(IFileDialogServiceBase.SupportMultiDottedExtensions),
                supportMultiDottedExtensions)
            .WithPropertyValue(nameof(IFileDialogServiceBase.FilterIndex), filterIndex)
            .WithPropertyValue(nameof(IFileDialogServiceBase.Title), title!)
            .WithPropertyValue(nameof(IFileDialogServiceBase.Filter), filter!)
            .WithPropertyValue(nameof(IFileDialogServiceBase.InitialDirectory), initialDirectory!);

        return kernel;
    }

    public static IKernel UseXtraOpenFileDialog<T>(this IKernel kernel,
        string? title = "Выбрать файл",
        string? filter = "Все файлы (*.*)|*.*",
        int filterIndex = 0,
        string? initialDirectory = default,
        bool multiSelect = false,
        bool addExtension = true,
        bool autoUpgradeEnabled = true,
        bool checkFileExists = true,
        bool checkPathExists = true,
        bool validateNames = true,
        bool dereferenceLinks = true,
        bool restoreDirectory = false,
        bool showHelp = false,
        bool supportMultiDottedExtensions = false) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IOpenFileDialogService>()
            .To<XtraOpenFileDialogService>()
            .WhenInjectedInto<T>()
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), true)
            .WithPropertyValue(nameof(IOpenDialogServiceBase.Multiselect), multiSelect)
            .WithPropertyValue(nameof(IFileDialogServiceBase.AddExtension), addExtension)
            .WithPropertyValue(nameof(IFileDialogServiceBase.AutoUpgradeEnabled), autoUpgradeEnabled)
            .WithPropertyValue(nameof(IFileDialogServiceBase.CheckFileExists), checkFileExists)
            .WithPropertyValue(nameof(IFileDialogServiceBase.CheckPathExists), checkPathExists)
            .WithPropertyValue(nameof(IFileDialogServiceBase.ValidateNames), validateNames)
            .WithPropertyValue(nameof(IFileDialogServiceBase.DereferenceLinks), dereferenceLinks)
            .WithPropertyValue(nameof(IFileDialogServiceBase.RestoreDirectory), restoreDirectory)
            .WithPropertyValue(nameof(IFileDialogServiceBase.ShowHelp), showHelp)
            .WithPropertyValue(nameof(IFileDialogServiceBase.SupportMultiDottedExtensions),
                supportMultiDottedExtensions)
            .WithPropertyValue(nameof(IFileDialogServiceBase.FilterIndex), filterIndex)
            .WithPropertyValue(nameof(IFileDialogServiceBase.Title), title!)
            .WithPropertyValue(nameof(IFileDialogServiceBase.Filter), filter!)
            .WithPropertyValue(nameof(IFileDialogServiceBase.InitialDirectory), initialDirectory!);

        return kernel;
    }

    public static IKernel UseXtraSaveFileDialog(this IKernel kernel,
        string? title = "Сохранить файл",
        string? filter = "Все файлы (*.*)|*.*",
        int filterIndex = 0,
        string? defaultExt = default,
        string? defaultFileName = default,
        string? initialDirectory = default,
        bool addExtension = true,
        bool autoUpgradeEnabled = true,
        bool checkFileExists = true,
        bool checkPathExists = true,
        bool validateNames = true,
        bool dereferenceLinks = true,
        bool restoreDirectory = false,
        bool showHelp = false,
        bool supportMultiDottedExtensions = false) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<ISaveFileDialogService>()
            .To<XtraSaveFileDialogService>()
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), false)
            .WithPropertyValue(nameof(IFileDialogServiceBase.AddExtension), addExtension)
            .WithPropertyValue(nameof(IFileDialogServiceBase.AutoUpgradeEnabled), autoUpgradeEnabled)
            .WithPropertyValue(nameof(IFileDialogServiceBase.CheckFileExists), checkFileExists)
            .WithPropertyValue(nameof(IFileDialogServiceBase.CheckPathExists), checkPathExists)
            .WithPropertyValue(nameof(IFileDialogServiceBase.ValidateNames), validateNames)
            .WithPropertyValue(nameof(IFileDialogServiceBase.DereferenceLinks), dereferenceLinks)
            .WithPropertyValue(nameof(IFileDialogServiceBase.RestoreDirectory), restoreDirectory)
            .WithPropertyValue(nameof(IFileDialogServiceBase.ShowHelp), showHelp)
            .WithPropertyValue(nameof(IFileDialogServiceBase.SupportMultiDottedExtensions),
                supportMultiDottedExtensions)
            .WithPropertyValue(nameof(IFileDialogServiceBase.FilterIndex), filterIndex)
            .WithPropertyValue(nameof(IFileDialogServiceBase.Title), title!)
            .WithPropertyValue(nameof(IFileDialogServiceBase.Filter), filter!)
            .WithPropertyValue(nameof(IFileDialogServiceBase.InitialDirectory), initialDirectory!)
            .WithPropertyValue(nameof(ISaveFileDialogService.DefaultExt), defaultExt!)
            .WithPropertyValue(nameof(ISaveFileDialogService.DefaultFileName), defaultFileName!);

        return kernel;
    }

    public static IKernel UseXtraSaveFileDialog<T>(this IKernel kernel,
        string? title = "Сохранить файл",
        string? filter = "Все файлы (*.*)|*.*",
        int filterIndex = 0,
        string? defaultExt = default,
        string? defaultFileName = default,
        string? initialDirectory = default,
        bool addExtension = true,
        bool autoUpgradeEnabled = true,
        bool checkFileExists = true,
        bool checkPathExists = true,
        bool validateNames = true,
        bool dereferenceLinks = true,
        bool restoreDirectory = false,
        bool showHelp = false,
        bool supportMultiDottedExtensions = false) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<ISaveFileDialogService>()
            .To<XtraSaveFileDialogService>()
            .WhenInjectedInto<T>()
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), true)
            .WithPropertyValue(nameof(IFileDialogServiceBase.AddExtension), addExtension)
            .WithPropertyValue(nameof(IFileDialogServiceBase.AutoUpgradeEnabled), autoUpgradeEnabled)
            .WithPropertyValue(nameof(IFileDialogServiceBase.CheckFileExists), checkFileExists)
            .WithPropertyValue(nameof(IFileDialogServiceBase.CheckPathExists), checkPathExists)
            .WithPropertyValue(nameof(IFileDialogServiceBase.ValidateNames), validateNames)
            .WithPropertyValue(nameof(IFileDialogServiceBase.DereferenceLinks), dereferenceLinks)
            .WithPropertyValue(nameof(IFileDialogServiceBase.RestoreDirectory), restoreDirectory)
            .WithPropertyValue(nameof(IFileDialogServiceBase.ShowHelp), showHelp)
            .WithPropertyValue(nameof(IFileDialogServiceBase.SupportMultiDottedExtensions),
                supportMultiDottedExtensions)
            .WithPropertyValue(nameof(IFileDialogServiceBase.FilterIndex), filterIndex)
            .WithPropertyValue(nameof(IFileDialogServiceBase.Title), title!)
            .WithPropertyValue(nameof(IFileDialogServiceBase.Filter), filter!)
            .WithPropertyValue(nameof(IFileDialogServiceBase.InitialDirectory), initialDirectory!)
            .WithPropertyValue(nameof(ISaveFileDialogService.DefaultExt), defaultExt!)
            .WithPropertyValue(nameof(ISaveFileDialogService.DefaultFileName), defaultFileName!);

        return kernel;
    }

    public static IKernel UseXtraOpenFolderDialog(this IKernel kernel,
        string? title = "Выбрать папку",
        string? initialDirectory = default,
        bool multiSelect = false,
        bool autoUpgradeEnabled = true,
        bool checkPathExists = true,
        bool validateNames = true,
        bool restoreDirectory = false,
        bool showHelp = false) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IOpenFolderDialogService>()
            .To<XtraOpenFolderDialogService>()
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), false)
            .WithPropertyValue(nameof(IOpenDialogServiceBase.Multiselect), multiSelect)
            .WithPropertyValue(nameof(IFileDialogServiceBase.AutoUpgradeEnabled), autoUpgradeEnabled)
            .WithPropertyValue(nameof(IFileDialogServiceBase.CheckPathExists), checkPathExists)
            .WithPropertyValue(nameof(IFileDialogServiceBase.ValidateNames), validateNames)
            .WithPropertyValue(nameof(IFileDialogServiceBase.RestoreDirectory), restoreDirectory)
            .WithPropertyValue(nameof(IFileDialogServiceBase.ShowHelp), showHelp)
            .WithPropertyValue(nameof(IFileDialogServiceBase.Title), title!)
            .WithPropertyValue(nameof(IFileDialogServiceBase.InitialDirectory), initialDirectory!);

        return kernel;
    }

    public static IKernel UseXtraOpenFolderDialog<T>(this IKernel kernel,
        string? title = "Выбрать папку",
        string? initialDirectory = default,
        bool multiSelect = false,
        bool autoUpgradeEnabled = true,
        bool checkPathExists = true,
        bool validateNames = true,
        bool restoreDirectory = false,
        bool showHelp = false) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IOpenFolderDialogService>()
            .To<XtraOpenFolderDialogService>()
            .WhenInjectedInto<T>()
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), true)
            .WithPropertyValue(nameof(IOpenDialogServiceBase.Multiselect), multiSelect)
            .WithPropertyValue(nameof(IFileDialogServiceBase.AutoUpgradeEnabled), autoUpgradeEnabled)
            .WithPropertyValue(nameof(IFileDialogServiceBase.CheckPathExists), checkPathExists)
            .WithPropertyValue(nameof(IFileDialogServiceBase.ValidateNames), validateNames)
            .WithPropertyValue(nameof(IFileDialogServiceBase.RestoreDirectory), restoreDirectory)
            .WithPropertyValue(nameof(IFileDialogServiceBase.ShowHelp), showHelp)
            .WithPropertyValue(nameof(IFileDialogServiceBase.Title), title!)
            .WithPropertyValue(nameof(IFileDialogServiceBase.InitialDirectory), initialDirectory!);

        return kernel;
    }
}