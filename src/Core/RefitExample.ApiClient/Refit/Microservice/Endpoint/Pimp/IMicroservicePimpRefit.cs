using Refit;
using RefitExample.ApiClient.DataAnnotation;
using RefitExample.ApiClient.Refit.Microservice.Interface;
using RefitExample.Arguments.Enum.Microservice;

namespace RefitExample.ApiClient.Refit.Microservice.Endpoint.Pimp;

[MicroserviceRefit(EnumMicroservice.Pimp)]
public interface IMicroservicePimpRefit : IMicroserviceRefitInterface
{
    [Get("/api/User/Pimp")]
    Task<ApiResponse<string>> Pimp();
}