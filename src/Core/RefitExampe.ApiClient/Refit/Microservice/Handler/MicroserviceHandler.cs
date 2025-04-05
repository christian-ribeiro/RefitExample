using RefitExampe.ApiClient.Interface.Service.Microservice.Authentication;
using RefitExample.Arguments.Argument.Refit.Microservice.Endpoint.Authentication;
using RefitExample.Arguments.Argument.Session;
using RefitExample.Arguments.Enum.Microservice;
using System.Net;

namespace RefitExampe.ApiClient.Refit.Microservice.Handler;

public class MicroserviceHandler(IAuthenticationService authenticationService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await TrySendAsync(request, cancellationToken, false);
    }

    private async Task<HttpResponseMessage> TrySendAsync(HttpRequestMessage request, CancellationToken cancellationToken, bool exit)
    {
        var authentication = SessionData.GetMicroserviceAuthentication(EnumMicroservice.DrugTrafficking, 1);
        if (authentication == null && !exit)
        {
            Authenticate();
            return await TrySendAsync(request, cancellationToken, true);
        }

        if (authentication != null)
        {
            request.Headers.Add("Authorization", $"Bearer {authentication!.Token}");
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.Unauthorized && !exit)
        {
            Authenticate();
            return await TrySendAsync(request, cancellationToken, true);
        }

        return response;
    }

    private void Authenticate()
    {
        var authenticate = authenticationService.Login(new InputAuthenticateUser("eve.holt@reqres.in", "cityslicka"));
        SessionData.AddMicroserviceAuthentication(new MicroserviceAuthentication(EnumMicroservice.DrugTrafficking, 1, authenticate.Result.Token));
    }
}