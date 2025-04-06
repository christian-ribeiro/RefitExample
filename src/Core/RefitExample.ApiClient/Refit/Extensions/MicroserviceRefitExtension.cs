using Microsoft.Extensions.DependencyInjection;
using Refit;
using RefitExample.ApiClient.Refit.Microservice.Handler;
using RefitExample.ApiClient.Refit.Microservice.Interface;
using System.Text.RegularExpressions;

namespace RefitExample.ApiClient.Refit.Extensions;

public static class MicroserviceRefitExtension
{
    private static IEnumerable<Type> GetListInterfaceType(Type interfaceType) => (from i in typeof(IBaseRefitInterface).Assembly.GetTypes()
                                                                                  where interfaceType.IsAssignableFrom(i) && i != interfaceType && i.IsInterface
                                                                                  select i);
    public static IServiceCollection AppendRefitInterfaces<TTypeInterface>(this IServiceCollection services, Func<Uri> baseAddressProvider, Func<IHttpClientBuilder, IHttpClientBuilder>? configureHttpClientBuilder = null)
        where TTypeInterface : IBaseRefitInterface
    {
        var types = GetListInterfaceType(typeof(TTypeInterface));

        string pattern = @"IMicroservice(.*?)Refit";

        foreach (var item in types)
        {
            var httpClientBuilder = services.AddRefitClient(item).ConfigureHttpClient((client) =>
            {
                client.BaseAddress = baseAddressProvider();
                client.DefaultRequestHeaders.Add(MicroserviceHandler.RefitClientHeader, Regex.Match(item.Name, pattern).Groups[1].Value);
            });

            configureHttpClientBuilder?.Invoke(httpClientBuilder);
        }

        return services;
    }
}