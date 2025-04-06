using Refit;
using RefitExample.ApiClient.Refit.Microservice.Interface;
using RefitExample.Arguments.Argument.Refit.Microservice.Endpoint.Authentication;

namespace RefitExample.ApiClient.Refit.Microservice.Endpoint.Pimp;

public interface IMicroservicePimpRefit : IMicroserviceRefitInterface
{
    [Get("/api/users")]
    Task<ApiResponse<OutputUserResponse>> GetUsers(int page);
}