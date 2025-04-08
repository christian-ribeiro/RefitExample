using RefitExample.Arguments.Cache.Interface;
using RefitExample.Arguments.Enum.Microservice;
using System.Collections.Concurrent;

namespace RefitExample.Arguments.Cache.Cache;

using EnterpriseId = Int64;

public class MicroserviceAuthCache : IMicroserviceAuthCache
{
    private readonly ConcurrentDictionary<EnterpriseId, ConcurrentDictionary<EnumMicroservice, MicroserviceAuthentication>> _cache = new();

    public MicroserviceAuthentication? TryGetValidAuth(long enterpriseId, EnumMicroservice service)
    {
        return _cache.TryGetValue(enterpriseId, out var serviceAuths) && serviceAuths.TryGetValue(service, out var authentication)
            ? authentication
            : null;
    }

    public void AddOrUpdateAuth(long enterpriseId, EnumMicroservice service, MicroserviceAuthentication authentication)
    {
        var serviceAuths = _cache.GetOrAdd(enterpriseId, _ => new ConcurrentDictionary<EnumMicroservice, MicroserviceAuthentication>());
        serviceAuths.AddOrUpdate(service, authentication, (_, _) => authentication);
    }
}