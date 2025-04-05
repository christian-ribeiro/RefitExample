using RefitExampe.ApiClient.Refit.Microservice.Endpoint.Authentication;
using RefitExample.Arguments.Argument.Refit.Microservice.Endpoint.Authentication;
using RefitExampe.ApiClient.Interface.Service.Microservice.Authentication;

namespace RefitExampe.ApiClient.Service.Microservice.Authentication;

public class AuthenticationService(IMicroserviceAuthenticationRefit microserviceAuthenticationRefit) : IAuthenticationService
{
    public async Task<OutputAuthResponse> Login(InputAuthenticateUser inputAuthenticateUser)
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