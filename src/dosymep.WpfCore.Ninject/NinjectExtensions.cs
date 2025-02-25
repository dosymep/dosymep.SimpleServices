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
    /// Регистрирует окно с вьюмоделью.
    /// </summary>
    /// <param name="kernel">Ninject контейнер.</param>
    /// <typeparam name="TViewModel">ViewModel</typeparam>
    /// <typeparam name="TMainWindow">Window</typeparam>
    /// <returns>Возвращает настроенный контейнер Ninject.</returns>
    /// <remarks>Биндит окно и вьюмодель как синглтон.</remarks>
    public static IKernel BindWindow<TViewModel, TMainWindow>(this IKernel kernel)
        where TMainWindow : Window, IHasTheme, IHasLocalization {

        kernel.Bind<TViewModel>().ToSelf().InSingletonScope();
        kernel.Bind<IHasTheme, IHasLocalization, TMainWindow>()
            .To<TMainWindow>()
            .InSingletonScope()
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
    /// <remarks>Обязательно требуется прикрепить к элементу управления через <see cref="WpfAttachServiceBehavior"/>.</remarks>
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
}