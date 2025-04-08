using Refit;
using RefitMicroserviceAuth.ApiClient.DataAnnotation;
using RefitMicroserviceAuth.ApiClient.Refit.Microservice.Interface;
using RefitMicroserviceAuth.Arguments.Enum.Microservice;

namespace RefitMicroserviceAuth.ApiClient.Refit.Microservice.Endpoint.Mercos;

[MicroserviceRefit(EnumMicroservice.Mercos)]
public interface IMicroserviceMercosRefit : IMicroserviceRefitInterface
{
    [Get("/api/Mercos")]
    Task<ApiResponse<string>> Mercos();
}