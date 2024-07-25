using System.Globalization;
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

/// <summary>
/// Расширения для настройки <see cref="IKernel"/>.
/// </summary>
public static class NinjectExtensions {
    /// <summary>
    /// Добавляет в контейнер <see cref="ILanguageService"/>,
    /// который возвращает установленную локализацию в настройках Windows.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseXtraLanguage(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<ILanguageService>()
            .To<XtraWindowsLanguageService>();

        return kernel;
    }

    /// <summary>
    /// Добавляет в контейнер <see cref="IUIThemeService"/>,
    /// который возвращает установленную тему в настройках Windows.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseXtraTheme(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IUIThemeService>()
            .To<XtraWindowsThemeService>();

        return kernel;
    }

    /// <summary>
    /// Добавляет в контейнер <see cref="IUIThemeUpdaterService"/>,
    /// который устанавливает тему DevExpress для окна <see cref="DevExpress.Xpf.Core.ThemedWindow"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseXtraThemeUpdater(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IUIThemeUpdaterService>()
            .To<XtraThemeUpdaterService>();

        return kernel;
    }

    /// <summary>
    /// Добавляет в контейнер <see cref="IDispatcherService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseXtraDispatcher(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IDispatcherService>()
            .To<XtraDispatcherService>()
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), false);

        return kernel;
    }
    
    /// <summary>
    /// Добавляет в контейнер <see cref="IDispatcherService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <typeparam name="T">Тип ViewModel к которой будет прикрепление сервиса.</typeparam>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <remarks>Обязательно требуется прикрепить к элементу управления через <see cref="AttachServiceBehavior"/>.</remarks>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
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

    /// <summary>
    /// Добавляет в контейнер <see cref="IMessageBoxService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseXtraMessageBox(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IMessageBoxService>()
            .To<XtraMessageBoxService>()
            .WithPropertyValue(nameof(XtraProgressDialogService.UIThemeService),
                c => c.Kernel.Get<IUIThemeService>())
            .WithPropertyValue(nameof(XtraProgressDialogService.UIThemeUpdaterService),
                c => c.Kernel.Get<IUIThemeUpdaterService>())
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), false);

        return kernel;
    }

    /// <summary>
    /// Добавляет в контейнер <see cref="IMessageBoxService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <typeparam name="T">Тип ViewModel к которой будет прикрепление сервиса.</typeparam>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <remarks>Обязательно требуется прикрепить к элементу управления через <see cref="AttachServiceBehavior"/>.</remarks>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
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

    /// <summary>
    /// Добавляет в контейнер <see cref="IProgressDialogService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <param name="stepValue">Значение шага прогресса. По умолчанию "10".</param>
    /// <param name="indeterminate">True - включает неопределенный ход выполнения. По умолчанию выключено.</param>
    /// <param name="displayTitleFormat">Формат отображения хода выполнения.
    /// Значение {0} - текущий прогресс, {1} - максимальное количество.
    /// По умолчанию "Пожалуйста подождите [{0}/{1}] ..."</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
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

    /// <summary>
    /// Добавляет в контейнер <see cref="IProgressDialogFactory"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <param name="stepValue">Значение шага прогресса. По умолчанию "10".</param>
    /// <param name="indeterminate">True - включает неопределенный ход выполнения. По умолчанию выключено.</param>
    /// <param name="displayTitleFormat">Формат отображения хода выполнения.
    /// Значение {0} - текущий прогресс, {1} - максимальное количество.
    /// По умолчанию "Пожалуйста подождите [{0}/{1}] ..."</param>
    /// <typeparam name="T">Тип ViewModel к которой будет прикрепление сервиса.</typeparam>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <remarks>Обязательно требуется прикрепить к элементу управления через <see cref="AttachServiceBehavior"/>.</remarks>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
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

    /// <summary>
    /// Добавляет в контейнер <see cref="INotificationService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <param name="applicationId">Идентификатор приложения.</param>
    /// <param name="defaultAuthor">Имя автора по умолчанию. Расположение снизу справа на уведомлении.</param>
    /// <param name="defaultFooter">Значение футера по умолчанию. Расположение снизу слева на уведомлении.</param>
    /// <param name="defaultImage">Значение изображения по умолчанию. Расположение слева на уведомлении.</param>
    /// <param name="sound">Значение звука уведомления (не используется).
    /// По умолчанию <see cref="PredefinedSound.NoSound"/>.</param>
    /// <param name="notificationScreen">Значение выбора окна.
    /// По умолчанию <see cref="NotificationScreen.Primary"/>.</param>
    /// <param name="notificationPosition">Значение места отображения уведомления.
    /// По умолчанию <see cref="NotificationPosition.BottomRight"/>.</param>
    /// <param name="notificationVisibleMaxCount">Максимальное количество уведомлений на экране. По умолчанию "5".</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
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

    /// <summary>
    /// Добавляет в контейнер <see cref="INotificationService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <param name="applicationId">Идентификатор приложения.</param>
    /// <param name="defaultAuthor">Имя автора по умолчанию. Расположение снизу справа на уведомлении.</param>
    /// <param name="defaultFooter">Значение футера по умолчанию. Расположение снизу слева на уведомлении.</param>
    /// <param name="defaultImage">Значение изображения по умолчанию. Расположение слева на уведомлении.</param>
    /// <param name="sound">Значение звука уведомления (не используется).
    /// По умолчанию <see cref="PredefinedSound.NoSound"/>.</param>
    /// <param name="notificationScreen">Значение выбора окна.
    /// По умолчанию <see cref="NotificationScreen.Primary"/>.</param>
    /// <param name="notificationPosition">Значение места отображения уведомления.
    /// По умолчанию <see cref="NotificationPosition.BottomRight"/>.</param>
    /// <param name="notificationVisibleMaxCount">Максимальное количество уведомлений на экране. По умолчанию "5".</param>
    /// <typeparam name="T">Тип ViewModel к которой будет прикрепление сервиса.</typeparam>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <remarks>Обязательно требуется прикрепить к элементу управления через <see cref="AttachServiceBehavior"/>.</remarks>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
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

    /// <summary>
    /// Добавляет в контейнер <see cref="IOpenFileDialogService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <param name="title">Заголовок окна. По умолчанию "Выбрать файл".</param>
    /// <param name="filter">Применяемый фильтр для файлов. По умолчанию "Все файлы (*.*)|*.*".</param>
    /// <param name="filterIndex">Применяемый индекс фильтра для файлов. По умолчанию "0".</param>
    /// <param name="initialDirectory">Директория открываемая по умолчанию.</param>
    /// <param name="multiSelect">True - включает мультивыбор файлов. По умолчанию выключено.</param>
    /// <param name="addExtension">True включает автоматическое добавление расширения файла. По умолчанию включено.</param>
    /// <param name="autoUpgradeEnabled">True - включает автоматическую смену внешнего вида. По умолчанию включено.</param>
    /// <param name="checkFileExists">True - включает проверку расширения файла. По умолчанию включено.</param>
    /// <param name="checkPathExists">True - включает проверку существования пути до файла. По умолчанию включено.</param>
    /// <param name="validateNames">True - включает проверку правильности набранного имени файла. По умолчанию включено.</param>
    /// <param name="dereferenceLinks">True - включает возвращение расположения файла, на который ссылается ярлык. По умолчанию включено.</param>
    /// <param name="restoreDirectory">True - запоминает выбранное расположение. По умолчанию выключено.</param>
    /// <param name="showHelp">True - включает отображение справки. По умолчанию выключено.</param>
    /// <param name="supportMultiDottedExtensions">True - включает отображение и сохранение файлов с несколькими расширениями. По умолчанию выключено.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
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

    /// <summary>
    /// Добавляет в контейнер <see cref="IOpenFileDialogService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <param name="title">Заголовок окна. По умолчанию "Выбрать файл".</param>
    /// <param name="filter">Применяемый фильтр для файлов. По умолчанию "Все файлы (*.*)|*.*".</param>
    /// <param name="filterIndex">Применяемый индекс фильтра для файлов. По умолчанию "0".</param>
    /// <param name="initialDirectory">Директория открываемая по умолчанию.</param>
    /// <param name="multiSelect">True - включает мультивыбор файлов. По умолчанию выключено.</param>
    /// <param name="addExtension">True включает автоматическое добавление расширения файла. По умолчанию включено.</param>
    /// <param name="autoUpgradeEnabled">True - включает автоматическую смену внешнего вида. По умолчанию включено.</param>
    /// <param name="checkFileExists">True - включает проверку расширения файла. По умолчанию включено.</param>
    /// <param name="checkPathExists">True - включает проверку существования пути до файла. По умолчанию включено.</param>
    /// <param name="validateNames">True - включает проверку правильности набранного имени файла. По умолчанию включено.</param>
    /// <param name="dereferenceLinks">True - включает возвращение расположения файла, на который ссылается ярлык. По умолчанию включено.</param>
    /// <param name="restoreDirectory">True - запоминает выбранное расположение. По умолчанию выключено.</param>
    /// <param name="showHelp">True - включает отображение справки. По умолчанию выключено.</param>
    /// <param name="supportMultiDottedExtensions">True - включает отображение и сохранение файлов с несколькими расширениями. По умолчанию выключено.</param>
    /// <typeparam name="T">Тип ViewModel к которой будет прикрепление сервиса.</typeparam>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <remarks>Обязательно требуется прикрепить к элементу управления через <see cref="AttachServiceBehavior"/>.</remarks>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
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

    /// <summary>
    /// Добавляет в контейнер <see cref="ISaveFileDialogService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <param name="title">Заголовок окна. По умолчанию "Сохранить файл".</param>
    /// <param name="filter">Применяемый фильтр для файлов. По умолчанию "Все файлы (*.*)|*.*".</param>
    /// <param name="filterIndex">Применяемый индекс фильтра для файлов. По умолчанию "0".</param>
    /// <param name="defaultExt">Расширение файла по умолчанию.</param>
    /// <param name="defaultFileName">Имя файла по умолчанию.</param>
    /// <param name="initialDirectory">Директория открываемая по умолчанию.</param>
    /// <param name="addExtension">True включает автоматическое добавление расширения файла. По умолчанию включено.</param>
    /// <param name="autoUpgradeEnabled">True - включает автоматическую смену внешнего вида. По умолчанию включено.</param>
    /// <param name="checkFileExists">True - включает проверку расширения файла. По умолчанию включено.</param>
    /// <param name="checkPathExists">True - включает проверку существования пути до файла. По умолчанию включено.</param>
    /// <param name="validateNames">True - включает проверку правильности набранного имени файла. По умолчанию включено.</param>
    /// <param name="dereferenceLinks">True - включает возвращение расположения файла, на который ссылается ярлык. По умолчанию включено.</param>
    /// <param name="restoreDirectory">True - запоминает выбранное расположение. По умолчанию выключено.</param>
    /// <param name="showHelp">True - включает отображение справки. По умолчанию выключено.</param>
    /// <param name="supportMultiDottedExtensions">True - включает отображение и сохранение файлов с несколькими расширениями. По умолчанию выключено.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseXtraSaveFileDialog(this IKernel kernel,
        string? title = "Сохранить файл",
        string? filter = "Все файлы (*.*)|*.*",
        int filterIndex = 0,
        string? defaultExt = default,
        string? defaultFileName = default,
        string? initialDirectory = default,
        bool addExtension = true,
        bool autoUpgradeEnabled = true,
        bool checkFileExists = false,
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

    /// <summary>
    /// Добавляет в контейнер <see cref="ILocalizationService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <param name="resourceName">Наименование ресурсов.</param>
    /// <param name="defaultCulture">Языковые настройки по умолчанию. Значение по умолчанию <see cref="CultureInfo.CurrentUICulture"/>.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    public static IKernel UseXtraLocalization(this IKernel kernel, string resourceName, CultureInfo? defaultCulture = default) {
        kernel.Bind<ILocalizationService>().To<XtraLocalizationService>()
            .InSingletonScope()
            .WithConstructorArgument(nameof(resourceName), resourceName)
            .WithConstructorArgument(nameof(defaultCulture), defaultCulture ?? CultureInfo.CurrentUICulture)
            .OnActivation((context, service) =>
                service.SetLocalization(
                    context.Kernel.TryGet<ILanguageService>()?.HostLanguage
                    ?? defaultCulture
                    ?? CultureInfo.CurrentUICulture));
        
        return kernel;
    }

    /// <summary>
    /// Добавляет в контейнер <see cref="ISaveFileDialogService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <param name="title">Заголовок окна. По умолчанию "Сохранить файл".</param>
    /// <param name="filter">Применяемый фильтр для файлов. По умолчанию "Все файлы (*.*)|*.*".</param>
    /// <param name="filterIndex">Применяемый индекс фильтра для файлов. По умолчанию "0".</param>
    /// <param name="defaultExt">Расширение файла по умолчанию.</param>
    /// <param name="defaultFileName">Имя файла по умолчанию.</param>
    /// <param name="initialDirectory">Директория открываемая по умолчанию.</param>
    /// <param name="addExtension">True включает автоматическое добавление расширения файла. По умолчанию включено.</param>
    /// <param name="autoUpgradeEnabled">True - включает автоматическую смену внешнего вида. По умолчанию включено.</param>
    /// <param name="checkFileExists">True - включает проверку расширения файла. По умолчанию включено.</param>
    /// <param name="checkPathExists">True - включает проверку существования пути до файла. По умолчанию включено.</param>
    /// <param name="validateNames">True - включает проверку правильности набранного имени файла. По умолчанию включено.</param>
    /// <param name="dereferenceLinks">True - включает возвращение расположения файла, на который ссылается ярлык. По умолчанию включено.</param>
    /// <param name="restoreDirectory">True - запоминает выбранное расположение. По умолчанию выключено.</param>
    /// <param name="showHelp">True - включает отображение справки. По умолчанию выключено.</param>
    /// <param name="supportMultiDottedExtensions">True - включает отображение и сохранение файлов с несколькими расширениями. По умолчанию выключено.</param>
    /// <typeparam name="T">Тип ViewModel к которой будет прикрепление сервиса.</typeparam>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <remarks>Обязательно требуется прикрепить к элементу управления через <see cref="AttachServiceBehavior"/>.</remarks>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseXtraSaveFileDialog<T>(this IKernel kernel,
        string? title = "Сохранить файл",
        string? filter = "Все файлы (*.*)|*.*",
        int filterIndex = 0,
        string? defaultExt = default,
        string? defaultFileName = default,
        string? initialDirectory = default,
        bool addExtension = true,
        bool autoUpgradeEnabled = true,
        bool checkFileExists = false,
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

    /// <summary>
    /// Добавляет в контейнер <see cref="IOpenFolderDialogService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <param name="title">Заголовок окна. По умолчанию "Выбрать папку".</param>
    /// <param name="initialDirectory">Директория открываемая по умолчанию.</param>
    /// <param name="multiSelect">True - разрешает мультивыбор. По умолчанию отключено.</param>
    /// <param name="autoUpgradeEnabled">True - включает автоматическую смену внешнего вида. По умолчанию включено.</param>
    /// <param name="checkPathExists">True - включает проверку существования пути до файла. По умолчанию включено.</param>
    /// <param name="validateNames">True - включает проверку правильности набранного имени файла. По умолчанию включено.</param>
    /// <param name="restoreDirectory">True - запоминает выбранное расположение. По умолчанию выключено.</param>
    /// <param name="showHelp">True - включает отображение справки. По умолчанию выключено.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
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

    /// <summary>
    /// Добавляет в контейнер <see cref="IOpenFolderDialogService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <param name="title">Заголовок окна. По умолчанию "Выбрать папку".</param>
    /// <param name="initialDirectory">Директория открываемая по умолчанию.</param>
    /// <param name="multiSelect">True - разрешает мультивыбор. По умолчанию отключено.</param>
    /// <param name="autoUpgradeEnabled">True - включает автоматическую смену внешнего вида. По умолчанию включено.</param>
    /// <param name="checkPathExists">True - включает проверку существования пути до файла. По умолчанию включено.</param>
    /// <param name="validateNames">True - включает проверку правильности набранного имени файла. По умолчанию включено.</param>
    /// <param name="restoreDirectory">True - запоминает выбранное расположение. По умолчанию выключено.</param>
    /// <param name="showHelp">True - включает отображение справки. По умолчанию выключено.</param>
    /// <typeparam name="T">Тип ViewModel к которой будет прикрепление сервиса.</typeparam>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <remarks>Обязательно требуется прикрепить к элементу управления через <see cref="AttachServiceBehavior"/>.</remarks>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
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