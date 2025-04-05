using Refit;
using RefitExample.Arguments.Argument.Refit.Microservice.Endpoint.Authentication;

namespace RefitExample.ApiClient.Refit.Microservice.Endpoint.Customer;

public interface IMicroserviceCustomerRefit
{
    [Get("/api/users")]
    Task<ApiResponse<OutputUserResponse>> GetUsers(int page);
}