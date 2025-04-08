using RefitExample.Arguments.Enum.Microservice;
using System.Collections.Concurrent;

namespace RefitExample.Arguments.Argument.Session;

using EnterpriseId = Int64;

public static class MicroserviceAuthCache
{
    private static readonly ConcurrentDictionary<EnterpriseId, ConcurrentDictionary<EnumMicroservice, MicroserviceAuthentication>> _cache = new();

    public static MicroserviceAuthentication? TryGetValidAuth(long enterpriseId, EnumMicroservice service)
    {
        return _cache.TryGetValue(enterpriseId, out var serviceAuths) && serviceAuths.TryGetValue(service, out var authentication)
               ? authentication
               : null;
    }

    public static void AddOrUpdateAuth(long enterpriseId, EnumMicroservice service, MicroserviceAuthentication authentication)
    {
        var serviceAuths = _cache.GetOrAdd(enterpriseId, _ => new ConcurrentDictionary<EnumMicroservice, MicroserviceAuthentication>());
        serviceAuths.AddOrUpdate(service, authentication, (_, _) => authentication);
    }
}