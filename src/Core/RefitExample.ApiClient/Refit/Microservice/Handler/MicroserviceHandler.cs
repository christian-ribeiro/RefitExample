using Microsoft.AspNetCore.Http;
using RefitExample.ApiClient.Refit.Microservice.Endpoint.Authentication;
using RefitExample.ApiClient.Refit.Microservice.Endpoint.Credential;
using RefitExample.Arguments.Argument.Authenticate;
using RefitExample.Arguments.Argument.Credential;
using RefitExample.Arguments.Argument.Session;
using RefitExample.Arguments.Enum.Microservice;
using System.Net;
using System.Net.Http.Headers;

namespace RefitExample.ApiClient.Refit.Microservice.Handler;

public class MicroserviceHandler(IMicroserviceCredentialRefit microserviceCredentialRefit, IMicroserviceAuthenticationRefit microserviceAuthenticationRefit, IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    public const string AuthorizationHeader = "Authorization";
    public const string GuidSessionDataRequest = "GuidSessionDataRequest";
    public const string RefitClientHeader = "X-Refit-Client";

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        long loggedEnterpriseId = GetLoggedEnterprise();
        var microservice = GetMicroservice(request);
        var token = await GetOrAuthenticateTokenAsync(loggedEnterpriseId, microservice);
        UpdateAuthorizationHeader(request, token);

        var response = await base.SendAsync(request, cancellationToken);

        if (!string.IsNullOrEmpty(token) && response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await Authenticate(loggedEnterpriseId, microservice);

            var retryToken = MicroserviceAuthCache.TryGetValidAuth(loggedEnterpriseId, microservice)?.Token;
            if (!string.IsNullOrEmpty(retryToken))
            {
                UpdateAuthorizationHeader(request, retryToken);
                return await base.SendAsync(request, cancellationToken);
            }
        }

        return response;
    }

    private async Task<string?> GetOrAuthenticateTokenAsync(long enterpriseId, EnumMicroservice microservice)
    {
        var auth = MicroserviceAuthCache.TryGetValidAuth(enterpriseId, microservice);
        if (auth != null)
            return auth.Token;

        await Authenticate(enterpriseId, microservice);
        return MicroserviceAuthCache.TryGetValidAuth(enterpriseId, microservice)?.Token;
    }

    private static void UpdateAuthorizationHeader(HttpRequestMessage request, string? token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    private async Task Authenticate(long loggedEntepriseId, EnumMicroservice microservice)
    {
        if (loggedEntepriseId == 0)
            return;

        //Consumir Área Admin buscando as chaves
        var credential = await microserviceCredentialRefit.GetCredential(new InputCredential(loggedEntepriseId, microservice));
        if (credential.IsSuccessStatusCode && credential.Content != null)
        {
            //Autenticar no Microservice, após isso salvar as informações necessárias no MicroserviceAuthentication
            var authenticate = await microserviceAuthenticationRefit.Login(new InputAuthenticate(credential.Content!.ApplicationId, credential.Content!.ContractId));
            if (authenticate.IsSuccessStatusCode && authenticate.Content != null)
                MicroserviceAuthCache.AddOrUpdateAuth(loggedEntepriseId, microservice, new MicroserviceAuthentication(authenticate.Content.Token));
        }
    }

    private Guid GetGuidSessionDataRequest()
    {
        var header = httpContextAccessor.HttpContext.Request.Headers[GuidSessionDataRequest].FirstOrDefault();
        return Guid.TryParse(header, out var guid) ? guid : Guid.Empty;
    }

    private long GetLoggedEnterprise()
    {
        var guidSessionDataRequest = GetGuidSessionDataRequest();
        long loggedEnterpriseId = SessionData.GetLoggedEnterprise(guidSessionDataRequest) ?? 0;

        return loggedEnterpriseId;
    }

    private static EnumMicroservice GetMicroservice(HttpRequestMessage request)
    {
        var header = request.Headers.Contains(RefitClientHeader)
            ? request.Headers.GetValues(RefitClientHeader).FirstOrDefault()
            : null;

        return Enum.TryParse(header, out EnumMicroservice microservice) ? microservice : EnumMicroservice.None;
    }
}