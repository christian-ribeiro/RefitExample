using Microsoft.AspNetCore.Http;
using RefitExample.ApiClient.Refit.MockEndpoint.Authentication;
using RefitExample.ApiClient.Refit.MockEndpoint.Credential;
using RefitExample.Arguments.Argument.Authenticate;
using RefitExample.Arguments.Argument.Credential;
using RefitExample.Arguments.Argument.Session;
using RefitExample.Arguments.Const;
using RefitExample.Arguments.Enum.Microservice;
using RefitExample.Arguments.Extension;
using System.Net;

namespace RefitExample.ApiClient.Refit.Microservice.Handler;

public class MicroserviceHandler(IMicroserviceCredentialRefit microserviceCredentialRefit, IMicroserviceAuthenticationRefit microserviceAuthenticationRefit, IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        long loggedEnterpriseId = httpContextAccessor.HttpContext.GetLoggedEnterprise();
        var microservice = GetMicroservice(request);
        var token = await GetOrAuthenticateTokenAsync(loggedEnterpriseId, microservice);
        
        request.SetBearerToken(token);

        request.RequestUri = RewriteUri(request.RequestUri!, microservice);

        var response = await base.SendAsync(request, cancellationToken);

        if (!string.IsNullOrEmpty(token) && response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await Authenticate(loggedEnterpriseId, microservice);

            var retryToken = MicroserviceAuthCache.TryGetValidAuth(loggedEnterpriseId, microservice)?.Token;
            if (!string.IsNullOrEmpty(retryToken))
            {
                request.SetBearerToken(retryToken);
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

    private static EnumMicroservice GetMicroservice(HttpRequestMessage request)
    {
        return request.GetHeaderValue<EnumMicroservice>(ConfigurationConst.RefitClientHeader, Enum.TryParse);
    }

    private static Uri RewriteUri(Uri originalUri, EnumMicroservice microservice)
    {
        var scheme = originalUri.Scheme;
        var host = originalUri.Host;
        var port = originalUri.Port;
        var originalPath = originalUri.AbsolutePath;

        var newPath = $"/gateway/{microservice}{originalPath}";

        var newUriBuilder = new UriBuilder
        {
            Scheme = scheme,
            Host = host,
            Port = port,
            Path = newPath,
            Query = originalUri.Query
        };

        return newUriBuilder.Uri;
    }
}