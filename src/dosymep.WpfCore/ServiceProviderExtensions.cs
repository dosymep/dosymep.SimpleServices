using System.Xaml;

using dosymep.SimpleServices;

namespace dosymep.WpfCore;

/// <summary>
/// Расширения интерфейса <see cref="IServiceProvider"/>.
/// </summary>
public static class ServiceProviderExtensions {
    /// <summary>
    /// Возвращает сервис и приводит его к заданому типу.
    /// </summary>
    /// <param name="serviceProvider">Интерфейс провайдера сервисов.</param>
    /// <typeparam name="T">Тип в который нужно привести root объект.</typeparam>
    /// <returns>Возвращает сервис и приводит его к заданому типу.</returns>
    public static T? GetService<T>(this IServiceProvider serviceProvider) where T : class {
        return serviceProvider.GetService(typeof(T)) as T;
    }

    /// <summary>
    /// Возвращает root объект и приводит к заданому типу.
    /// </summary>
    /// <param name="serviceProvider">Интерфейс провайдера сервисов.</param>
    /// <typeparam name="T">Тип в который нужно привести root объект.</typeparam>
    /// <returns>Возвращает root объект сконвертированный в нужный тип.</returns>
    public static T? GetRootObject<T>(this IServiceProvider serviceProvider) where T : class {
        IRootObjectProvider rootObjectProvider =
            (IRootObjectProvider) serviceProvider.GetService(typeof(IRootObjectProvider));
        
        return rootObjectProvider.RootObject as T;
    }
}