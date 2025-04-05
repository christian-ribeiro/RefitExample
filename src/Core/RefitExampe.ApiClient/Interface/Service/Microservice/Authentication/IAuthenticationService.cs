using RefitExample.Arguments.Argument.Refit.Microservice.Endpoint.Authentication;

namespace RefitExampe.ApiClient.Interface.Service.Microservice.Authentication;

public interface IAuthenticationService
{
    Task<OutputAuthResponse> Login(InputAuthenticateUser inputAuthenticateUser);
}