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
        return await SendWithAuthRetryAsync(request, false, cancellationToken);
    }

    private async Task<HttpResponseMessage> SendWithAuthRetryAsync(HttpRequestMessage request, bool isRetry, CancellationToken cancellationToken)
    {
        var guidSessionDataRequest = GetGuidSessionDataRequest();
        long loggedUserId = SessionData.GetLoggedUser(guidSessionDataRequest) ?? 0;
        EnumMicroservice microservice = GetMicroservice(request);

        var authentication = SessionData.GetMicroserviceAuthentication(microservice, loggedUserId);
        if (authentication == null && !isRetry)
        {
            await Authenticate(loggedUserId, microservice);
            return await SendWithAuthRetryAsync(request, true, cancellationToken);
        }

        if (authentication != null)
        {
            request.Headers.Remove("Authorization");
            request.Headers.Add("Authorization", $"Bearer {authentication!.Token}");
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.Unauthorized && !isRetry)
        {
            await Authenticate(loggedUserId, microservice);
            return await SendWithAuthRetryAsync(request, true, cancellationToken);
        }

        return response;
    }

    private async Task Authenticate(long loggedUserId, EnumMicroservice microservice)
    {
        if (loggedUserId == 0)
            return;

        var authenticate = await authenticationService.Login(new InputAuthenticateUser("eve.holt@reqres.in", "cityslicka"));
        SessionData.SetMicroserviceAuthentication(new MicroserviceAuthentication(microservice, loggedUserId, authenticate.Token));
    }

    private Guid GetGuidSessionDataRequest()
    {
        if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue("GuidSessionDataRequest", out var values) && Guid.TryParse(values.FirstOrDefault(), out var guidSessionDataRequest))
        {
            return guidSessionDataRequest;
        }

        return Guid.Empty;
    }

    private static EnumMicroservice GetMicroservice(HttpRequestMessage request)
    {
        if (request.Headers.TryGetValues("X-Refit-Client", out var values) && Enum.TryParse<EnumMicroservice>(values.FirstOrDefault(), out var enumValue))
        {
            return enumValue;
        }

        return EnumMicroservice.None;
    }
}