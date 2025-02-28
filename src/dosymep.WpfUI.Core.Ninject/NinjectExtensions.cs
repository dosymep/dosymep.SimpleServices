using dosymep.SimpleServices;
using dosymep.WpfCore.Behaviors;
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
            .To<WpfUIThemeUpdaterService>();

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
}