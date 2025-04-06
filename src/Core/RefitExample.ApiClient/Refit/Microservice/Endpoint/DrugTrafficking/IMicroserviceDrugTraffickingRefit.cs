using Refit;
using RefitExample.ApiClient.DataAnnotation;
using RefitExample.ApiClient.Refit.Microservice.Interface;
using RefitExample.Arguments.Enum.Microservice;

namespace RefitExample.ApiClient.Refit.Microservice.Endpoint.DrugTrafficking;

[MicroserviceRefit(EnumMicroservice.DrugTrafficking)]
public interface IMicroserviceDrugTraffickingRefit : IMicroserviceRefitInterface
{
    [Get("/api/User/DrugTrafficking")]
    Task<ApiResponse<string>> DrugTrafficking();
}