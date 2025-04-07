using Refit;
using RefitExample.Arguments.Argument.Authenticate;

namespace RefitExample.ApiClient.Refit.Microservice.MockEndpoint.Authentication;

public interface IMicroserviceAuthenticationRefit
{
    [Post("/api/Authenticate")]
    Task<ApiResponse<OutputAuthenticate>> Login([Body] InputAuthenticate inputAuthenticate);
}