using RefitExample.Arguments.Cache.Cache;
using RefitExample.Arguments.Enum.Microservice;

namespace RefitExample.Arguments.Cache.Interface;

public interface IMicroserviceAuthCache
{
    MicroserviceAuthentication? TryGetValidAuth(long enterpriseId, EnumMicroservice service);
    void AddOrUpdateAuth(long enterpriseId, EnumMicroservice service, MicroserviceAuthentication authentication);
}