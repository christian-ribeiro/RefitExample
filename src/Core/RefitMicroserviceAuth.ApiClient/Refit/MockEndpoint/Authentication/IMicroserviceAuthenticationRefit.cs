using Refit;
using RefitMicroserviceAuth.Arguments.Argument.Authenticate;

namespace RefitMicroserviceAuth.ApiClient.Refit.MockEndpoint.Authentication;

public interface IMicroserviceAuthenticationRefit
{
    [Post("/api/Authenticate")]
    Task<ApiResponse<OutputAuthenticate>> Login([Body] InputAuthenticate inputAuthenticate);
}