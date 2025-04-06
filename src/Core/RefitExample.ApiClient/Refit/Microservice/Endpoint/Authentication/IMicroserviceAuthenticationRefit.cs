using Refit;
using RefitExample.Arguments.Argument.Authenticate;

namespace RefitExample.ApiClient.Refit.Microservice.Endpoint.Authentication;

public interface IMicroserviceAuthenticationRefit
{
    [Post("/api/Authenticate")]
    Task<ApiResponse<OutputAuthenticate>> Login([Body] InputAuthenticate inputAuthenticate);
}