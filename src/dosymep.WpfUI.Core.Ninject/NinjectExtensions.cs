using System.Windows.Media;

using dosymep.SimpleServices;
using dosymep.WpfCore.Behaviors;
using dosymep.WpfCore.SimpleServices;
using dosymep.WpfUI.Core.SimpleServices;

using Ninject;

using Wpf.Ui.Controls;

namespace dosymep.WpfUI.Core.Ninject;

/// <summary>
/// Расширения для настройки <see cref="IKernel"/>.
/// </summary>
public static class NinjectExtensions {
    /// <summary>
    /// Добавляет в контейнер <see cref="IUIThemeUpdaterService"/>,
    /// который устанавливает тему WpfUI для окна <see cref="FluentWindow"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseWpfUIThemeUpdater(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IUIThemeUpdaterService>()
            .To<WpfUIThemeUpdaterService>()
            .InSingletonScope();

        return kernel;
    }
    
    /// <summary>
    /// Добавляет в контейнер <see cref="IMessageBoxService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseWpfUIMessageBox(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IMessageBoxService>()
            .To<WpfUIMessageBoxService>()
            .InSingletonScope()
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), false);

        return kernel;
    }

    /// <summary>
    /// Добавляет в контейнер <see cref="IMessageBoxService"/>.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <typeparam name="T">Тип ViewModel к которой будет прикрепление сервиса.</typeparam>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <remarks>Обязательно требуется прикрепить к элементу управления через <see cref="WpfAttachServiceBehavior"/>.</remarks>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseWpfUIMessageBox<T>(this IKernel kernel) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IMessageBoxService>()
            .To<WpfUIMessageBoxService>()
            .WhenInjectedInto<T>()
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
    public static IKernel UseWpfUIProgressDialog(this IKernel kernel,
        int stepValue = 10,
        bool indeterminate = false,
        string displayTitleFormat = "Пожалуйста подождите [{0}/{1}] ...") {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IProgressDialogService>()
            .To<WpfUIProgressDialogService>()
            .InSingletonScope()
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), false)
            .WithPropertyValue(nameof(WpfUIProgressDialogService.DisplayTitleFormat), displayTitleFormat)
            .WithPropertyValue(nameof(WpfUIProgressDialogService.StepValue), stepValue)
            .WithPropertyValue(nameof(WpfUIProgressDialogService.Indeterminate), indeterminate);

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
    /// <remarks>Обязательно требуется прикрепить к элементу управления через <see cref="WpfAttachServiceBehavior"/>.</remarks>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseWpfUIProgressDialog<T>(this IKernel kernel,
        int stepValue = 10,
        bool indeterminate = false,
        string displayTitleFormat = "Пожалуйста подождите [{0}/{1}] ...") {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<IProgressDialogFactory>()
            .To<WpfUIProgressDialogFactory>()
            .WhenInjectedInto<T>()
            .WithPropertyValue(nameof(WpfUIProgressDialogService.DisplayTitleFormat), displayTitleFormat)
            .WithPropertyValue(nameof(WpfUIProgressDialogService.StepValue), stepValue)
            .WithPropertyValue(nameof(WpfUIProgressDialogService.Indeterminate), indeterminate);

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
    /// <param name="notificationScreen">Значение выбора окна.
    /// По умолчанию <see cref="NotificationScreen.Primary"/>.</param>
    /// <param name="notificationPosition">Значение места отображения уведомления.
    /// По умолчанию <see cref="NotificationPosition.BottomRight"/>.</param>
    /// <param name="notificationVisibleMaxCount">Максимальное количество уведомлений на экране. По умолчанию "5".</param>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseWpfUINotifications(this IKernel kernel,
        string? applicationId = null,
        string? defaultAuthor = null,
        string? defaultFooter = null,
        ImageSource? defaultImage = null,
        NotificationScreen notificationScreen = NotificationScreen.Primary,
        NotificationPosition notificationPosition = NotificationPosition.BottomRight,
        int notificationVisibleMaxCount = 5) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<INotificationService>()
            .To<WpfUINotificationService>()
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), false)
            .WithPropertyValue(nameof(WpfUINotificationService.ApplicationId), applicationId!)
            .WithPropertyValue(nameof(WpfUINotificationService.DefaultAuthor), defaultAuthor!)
            .WithPropertyValue(nameof(WpfUINotificationService.DefaultFooter), defaultFooter!)
            .WithPropertyValue(nameof(WpfUINotificationService.DefaultImage), defaultImage!)
            .WithPropertyValue(nameof(WpfUINotificationService.NotificationScreen), notificationScreen)
            .WithPropertyValue(nameof(WpfUINotificationService.NotificationPosition), notificationPosition)
            .WithPropertyValue(nameof(WpfUINotificationService.NotificationVisibleMaxCount),
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
    /// <param name="notificationScreen">Значение выбора окна.
    /// По умолчанию <see cref="NotificationScreen.Primary"/>.</param>
    /// <param name="notificationPosition">Значение места отображения уведомления.
    /// По умолчанию <see cref="NotificationPosition.BottomRight"/>.</param>
    /// <param name="notificationVisibleMaxCount">Максимальное количество уведомлений на экране. По умолчанию "5".</param>
    /// <typeparam name="T">Тип ViewModel к которой будет прикрепление сервиса.</typeparam>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <remarks>Обязательно требуется прикрепить к элементу управления через <see cref="WpfAttachServiceBehavior"/>.</remarks>
    /// <exception cref="ArgumentNullException">kernel is null.</exception>
    public static IKernel UseWpfUINotifications<T>(this IKernel kernel,
        string? applicationId = null,
        string? defaultAuthor = null,
        string? defaultFooter = null,
        ImageSource? defaultImage = null,
        NotificationScreen notificationScreen = NotificationScreen.ApplicationWindow,
        NotificationPosition notificationPosition = NotificationPosition.BottomRight,
        int notificationVisibleMaxCount = 5) {
        if(kernel == null) {
            throw new ArgumentNullException(nameof(kernel));
        }

        kernel.Bind<INotificationService>()
            .To<WpfUINotificationService>()
            .WhenInjectedInto<T>()
            .WithPropertyValue(nameof(IAttachableService.AllowAttach), true)
            .WithPropertyValue(nameof(WpfUINotificationService.ApplicationId), applicationId!)
            .WithPropertyValue(nameof(WpfUINotificationService.DefaultAuthor), defaultAuthor!)
            .WithPropertyValue(nameof(WpfUINotificationService.DefaultFooter), defaultFooter!)
            .WithPropertyValue(nameof(WpfUINotificationService.DefaultImage), defaultImage!)
            .WithPropertyValue(nameof(WpfUINotificationService.NotificationScreen), notificationScreen)
            .WithPropertyValue(nameof(WpfUINotificationService.NotificationPosition), notificationPosition)
            .WithPropertyValue(nameof(WpfUINotificationService.NotificationVisibleMaxCount), notificationVisibleMaxCount);

        return kernel;
    }
}