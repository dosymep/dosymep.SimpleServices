using System.Xaml;

using dosymep.SimpleServices;

namespace dosymep.WpfCore;

public static class ServiceProviderExtensions {
    public static T? GetRootObject<T>(this IServiceProvider serviceProvider) where T : class {
        IRootObjectProvider rootObjectProvider =
            (IRootObjectProvider) serviceProvider.GetService(typeof(IRootObjectProvider));
        
        return rootObjectProvider.RootObject as T;
    }
}