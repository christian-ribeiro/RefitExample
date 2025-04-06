using Microsoft.Extensions.DependencyInjection;
using Refit;
using RefitExample.ApiClient.Refit.Microservice.Interface;

namespace RefitExample.ApiClient.Refit.Extensions;

public static class RefitServiceCollectionExtensions
{
    public static IServiceCollection AppendRefitInterfaces<TTypeInterface>(this IServiceCollection services, Action<HttpClient, Type, IServiceProvider> httpClientConfigurator, RefitSettings? refitSettings, Func<IHttpClientBuilder, IHttpClientBuilder>? configureHttpClientBuilder = null)
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

    public static IServiceCollection AppendRefitInterfaces<TTypeInterface>(this IServiceCollection services, Action<HttpClient, Type> httpClientConfigurator, RefitSettings? refitSettings = null, Func<IHttpClientBuilder, IHttpClientBuilder>? configureHttpClientBuilder = null)
        where TTypeInterface : IBaseRefitInterface
    {
        return services.AppendRefitInterfaces<TTypeInterface>((client, type, _) => httpClientConfigurator(client, type), refitSettings, configureHttpClientBuilder);
    }

    public static IServiceCollection AppendRefitInterfaces<TTypeInterface>(this IServiceCollection services, Action<HttpClient> httpClientConfigurator, RefitSettings? refitSettings = null, Func<IHttpClientBuilder, IHttpClientBuilder>? configureHttpClientBuilder = null)
        where TTypeInterface : IBaseRefitInterface
    {
        return services.AppendRefitInterfaces<TTypeInterface>((client, _, _) => httpClientConfigurator(client), refitSettings, configureHttpClientBuilder);
    }

    private static IEnumerable<Type> GetListInterfaceType(Type baseInterface)
    {
        return typeof(IBaseRefitInterface).Assembly.GetTypes()
            .Where(t => baseInterface.IsAssignableFrom(t) && t.IsInterface && t != baseInterface);
    }
}
