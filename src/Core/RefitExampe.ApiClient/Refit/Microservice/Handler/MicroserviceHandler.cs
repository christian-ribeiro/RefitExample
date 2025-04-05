using Microsoft.AspNetCore.Http;
using RefitExampe.ApiClient.Interface.Service.Microservice.Authentication;
using RefitExample.Arguments.Argument.Refit.Microservice.Endpoint.Authentication;
using RefitExample.Arguments.Argument.Session;
using RefitExample.Arguments.Enum.Microservice;
using System.Net;

namespace RefitExampe.ApiClient.Refit.Microservice.Handler;

public class MicroserviceHandler(IAuthenticationService authenticationService, IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await TrySendAsync(request, cancellationToken, false);
    }

    private async Task<HttpResponseMessage> TrySendAsync(HttpRequestMessage request, CancellationToken cancellationToken, bool exit)
    {
        var guidSessionDataRequest = GetGuidSessionDataRequest();
        long loggedUserId = SessionData.GetLoggedUser(guidSessionDataRequest) ?? 0;

        var authentication = SessionData.GetMicroserviceAuthentication(EnumMicroservice.DrugTrafficking, loggedUserId);
        if (authentication == null && !exit)
        {
            Authenticate(loggedUserId);
            return await TrySendAsync(request, cancellationToken, true);
        }

        if (authentication != null)
        {
            request.Headers.Add("Authorization", $"Bearer {authentication!.Token}");
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.Unauthorized && !exit)
        {
            Authenticate(loggedUserId);
            return await TrySendAsync(request, cancellationToken, true);
        }

        return response;
    }

    private void Authenticate(long loggedUserId)
    {
        if (loggedUserId == 0)
            return;

        var authenticate = authenticationService.Login(new InputAuthenticateUser("eve.holt@reqres.in", "cityslicka"));
        SessionData.SetMicroserviceAuthentication(new MicroserviceAuthentication(EnumMicroservice.DrugTrafficking, loggedUserId, authenticate.Result.Token));
    }

    private Guid GetGuidSessionDataRequest()
    {
        if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue("GuidSessionDataRequest", out var values) && Guid.TryParse(values.FirstOrDefault(), out var guidSessionDataRequest))
        {
            return guidSessionDataRequest;
        }

        return Guid.Empty;
    }
}