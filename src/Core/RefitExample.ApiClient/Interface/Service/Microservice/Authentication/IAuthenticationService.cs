using RefitExample.Arguments.Argument.Authenticate;

namespace RefitExample.ApiClient.Interface.Service.Microservice.Authentication;

public interface IAuthenticationService
{
    Task<OutputAuthenticate> Login(InputAuthenticate inputAuthenticateUser);
}