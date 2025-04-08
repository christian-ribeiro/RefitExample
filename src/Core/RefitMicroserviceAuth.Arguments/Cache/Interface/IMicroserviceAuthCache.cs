using RefitMicroserviceAuth.Arguments.Cache.Cache;
using RefitMicroserviceAuth.Arguments.Enum.Microservice;

namespace RefitMicroserviceAuth.Arguments.Cache.Interface;

public interface IMicroserviceAuthCache
{
    MicroserviceAuthentication? TryGetValidAuth(long enterpriseId, EnumMicroservice service);
    void AddOrUpdateAuth(long enterpriseId, EnumMicroservice service, MicroserviceAuthentication authentication);
}