using Refit;
using RefitExample.Arguments.Argument.Refit.Microservice.Endpoint.Authentication;

namespace RefitExampe.ApiClient.Refit.Microservice.Endpoint.Authentication
{
    public interface IMicroserviceAuthenticationRefit
    {
        [Post("/api/login")]
        Task<ApiResponse<OutputAuthResponse>> Login([Body] InputAuthenticateUser inputUserAuthentication);
    }
}