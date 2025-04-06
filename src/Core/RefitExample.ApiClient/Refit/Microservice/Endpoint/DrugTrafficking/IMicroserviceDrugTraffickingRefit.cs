using Refit;
using RefitExample.ApiClient.Refit.Microservice.Interface;
using RefitExample.Arguments.Argument.Refit.Microservice.Endpoint.Authentication;

namespace RefitExample.ApiClient.Refit.Microservice.Endpoint.DrugTrafficking;

public interface IMicroserviceDrugTraffickingRefit : IMicroserviceRefitInterface
{
    [Get("/api/users")]
    Task<ApiResponse<OutputUserResponse>> GetUsers(int page);
}