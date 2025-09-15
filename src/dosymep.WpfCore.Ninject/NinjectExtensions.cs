using System.Globalization;
using System.Windows;

using dosymep.SimpleServices;
using dosymep.WpfCore.Behaviors;
using dosymep.WpfCore.SimpleServices;

using Ninject;

namespace dosymep.WpfCore.Ninject;

/// <summary>
/// Расширения для настройки <see cref="IKernel"/>.
/// </summary>
public static class NinjectExtensions {
    /// <summary>
    /// Регистрирует основное окно с вьюмоделью.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <typeparam name="TViewModel">ViewModel</typeparam>
    /// <typeparam name="TMainWindow">Window</typeparam>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <remarks>Биндит основное окно и вьюмодель как синглтон.</remarks>
    public static IKernel BindMainWindow<TViewModel, TMainWindow>(this IKernel kernel)
        where TMainWindow : Window, IHasTheme, IHasLocalization {

        kernel.Bind<TViewModel>().ToSelf().InSingletonScope();
        kernel.Bind<IHasTheme, IHasLocalization, TMainWindow>()
            .To<TMainWindow>()
            .InSingletonScope()
            .WithPropertyValue(nameof(Window.DataContext), c => c.Kernel.Get<TViewModel>());

        return kernel;
    }
    
    /// <summary>
    /// Регистрирует окно с вьюмоделью.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <typeparam name="TViewModel">ViewModel</typeparam>
    /// <typeparam name="TWindow">Window</typeparam>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <remarks>Биндит окно и вьюмодель как синглтон.</remarks>
    public static IKernel BindOtherWindow<TViewModel, TWindow>(this IKernel kernel)
        where TWindow : Window {

        kernel.Bind<TViewModel>().ToSelf();
        kernel.Bind<TWindow>().ToSelf()
            .WithPropertyValue(nameof(Window.DataContext), c => c.Kernel.Get<TViewModel>());

        return kernel;
    }

    /// <summary>
    /// Добавляет в контейнер <see cref="ILanguageService"/>,
    /// который возвращает установленную локализацию в настройках Windows.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseWpfWindowsLanguage(this IKernel kernel) {
        if(kernel is null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<ILanguageService>()
            .To<WpfWindowsLanguageService>();

        return kernel;
    }

    /// <summary>
    /// Добавляет в контейнер <see cref="IUIThemeService"/>,
    /// который возвращает установленную тему в настройках Windows.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseWpfWindowsTheme(this IKernel kernel) {
        if(kernel is null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IUIThemeService>()
            .To<WpfWindowsThemeService>();

        return kernel;
    }

    /// <summary>
    /// Добавляет в контейнер <see cref="IDispatcherService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseWpfDispatcher(this IKernel kernel) {
        if(kernel is null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IDispatcherService>()
            .To<WpfDispatcherService>()
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), false);

        return kernel;
    }

    /// <summary>
    /// Добавляет в контейнер <see cref="IDispatcherService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <typeparam name="T">Тип ViewModel к которой будет прикрепление сервиса.</typeparam>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <remarks>Обязательно требуется прикрепить к элементу управления через <see cref="WpfWpfAttachServiceBehavior"/>.</remarks>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseWpfDispatcher<T>(this IKernel kernel) {
        if(kernel is null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IDispatcherService>()
            .To<WpfDispatcherService>()
            .WhenInjectedInto<T>()
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), true);

        return kernel;
    }

    /// <summary>
    /// Добавляет в контейнер <see cref="ILocalizationService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <param name="resourceName">Наименование ресурсов.</param>
    /// <param name="defaultCulture">Языковые настройки по умолчанию. Значение по умолчанию <see cref="CultureInfo.CurrentUICulture"/>.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    public static IKernel UseWpfLocalization(this IKernel kernel, 
        string resourceName,
        CultureInfo? defaultCulture = default) {
        if(kernel is null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<ILocalizationService>()
            .To<WpfLocalizationService>()
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
    public static IKernel UseWpfOpenFileDialog(this IKernel kernel,
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
            .To<WpfOpenFileDialogService>()
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
    /// <remarks>Обязательно требуется прикрепить к элементу управления через <see cref="WpfAttachServiceBehavior"/>.</remarks>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseWpfOpenFileDialog<T>(this IKernel kernel,
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
            .To<WpfOpenFileDialogService>()
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
    public static IKernel UseWpfSaveFileDialog(this IKernel kernel,
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
            .To<WpfSaveFileDialogService>()
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
    /// <remarks>Обязательно требуется прикрепить к элементу управления через <see cref="WpfAttachServiceBehavior"/>.</remarks>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseWpfSaveFileDialog<T>(this IKernel kernel,
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
            .To<WpfSaveFileDialogService>()
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
    public static IKernel UseWpfOpenFolderDialog(this IKernel kernel,
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
            .To<WpfOpenFolderDialogService>()
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
    /// <remarks>Обязательно требуется прикрепить к элементу управления через <see cref="WpfAttachServiceBehavior"/>.</remarks>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseWpfOpenFolderDialog<T>(this IKernel kernel,
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
            .To<WpfOpenFolderDialogService>()
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