using RefitExample.ApiClient.Interface.Service.Microservice.Authentication;
using RefitExample.ApiClient.Refit.Microservice.Endpoint.Authentication;
using RefitExample.Arguments.Argument.Authenticate;

namespace RefitExample.ApiClient.Service.Microservice.Authentication;

public class AuthenticationService(IMicroserviceAuthenticationRefit microserviceAuthenticationRefit) : IAuthenticationService
{
    public async Task<OutputAuthenticate> Login(InputAuthenticate inputAuthenticateUser)
    {
        var response = await microserviceAuthenticationRefit.Login(inputAuthenticateUser);
        if (response.IsSuccessStatusCode)
        {
            return response.Content!;
        }
        else
        {
            throw response.Error;
        }
    }
}