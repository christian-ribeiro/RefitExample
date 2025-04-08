using Microsoft.Extensions.DependencyInjection;
using Refit;
using RefitMicroserviceAuth.ApiClient.Refit.Microservice.Interface;

namespace RefitMicroserviceAuth.ApiClient.Refit.Extensions;

public static class RefitServiceCollectionExtensions
{
    public static IServiceCollection AppendRefitInterfaces<TTypeInterface>(this IServiceCollection services, Action<HttpClient, Type, IServiceProvider> httpClientConfigurator, RefitSettings? refitSettings = null, Func<IHttpClientBuilder, IHttpClientBuilder>? configureHttpClientBuilder = null)
        where TTypeInterface : IBaseRefitInterface
    {
        var types = GetListInterfaceType(typeof(TTypeInterface));

        foreach (var item in types)
        {
            var httpClientBuilder = services
                .AddRefitClient(item, refitSettings)
                .ConfigureHttpClient((serviceProvider, client) => httpClientConfigurator(client, item, serviceProvider));

            configureHttpClientBuilder?.Invoke(httpClientBuilder);
        }

        return services;
    }

    private static IEnumerable<Type> GetListInterfaceType(Type baseInterface)
    {
        return typeof(IBaseRefitInterface).Assembly.GetTypes()
            .Where(t => baseInterface.IsAssignableFrom(t) && t.IsInterface && t != baseInterface);
    }
}
