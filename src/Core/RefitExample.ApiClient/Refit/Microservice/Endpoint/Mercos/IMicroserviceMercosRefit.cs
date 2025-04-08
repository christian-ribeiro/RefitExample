using Refit;
using RefitExample.ApiClient.DataAnnotation;
using RefitExample.ApiClient.Refit.Microservice.Interface;
using RefitExample.Arguments.Enum.Microservice;

namespace RefitExample.ApiClient.Refit.Microservice.Endpoint.Mercos;

[MicroserviceRefit(EnumMicroservice.Mercos)]
public interface IMicroserviceMercosRefit : IMicroserviceRefitInterface
{
    [Get("/api/Mercos")]
    Task<ApiResponse<string>> Mercos();
}