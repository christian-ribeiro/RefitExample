using Microsoft.AspNetCore.Http;
using RefitExample.ApiClient.Refit.MockEndpoint.Authentication;
using RefitExample.ApiClient.Refit.MockEndpoint.Credential;
using RefitExample.Arguments.Argument.Authenticate;
using RefitExample.Arguments.Argument.Credential;
using RefitExample.Arguments.Cache.Cache;
using RefitExample.Arguments.Cache.Interface;
using RefitExample.Arguments.Const;
using RefitExample.Arguments.Enum.Microservice;
using RefitExample.Arguments.Extension;
using System.Net;

namespace RefitExample.ApiClient.Refit.Microservice.Handler;

public class MicroserviceHandler(IMicroserviceCredentialRefit microserviceCredentialRefit, IMicroserviceAuthenticationRefit microserviceAuthenticationRefit, IMicroserviceAuthCache microserviceAuthCache, IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        long enterpriseId = httpContextAccessor.HttpContext.GetLoggedEnterprise();
        var microservice = GetMicroservice(request);
        var token = await GetOrAuthenticateTokenAsync(enterpriseId, microservice);

        request.SetBearerToken(token);

        request.RequestUri = RewriteUri(request.RequestUri!, microservice);

        var response = await base.SendAsync(request, cancellationToken);

        // Se a resposta indicar que o token atual não tem mais permissão, tenta reautenticar e reenviar a requisição.
        if (!string.IsNullOrWhiteSpace(token) && response.StatusCode.In(HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden))
        {
            var retryToken = await TryAuthenticate(enterpriseId, microservice);

            if (!string.IsNullOrWhiteSpace(retryToken))
            {
                request.SetBearerToken(retryToken);
                return await base.SendAsync(request, cancellationToken);
            }
        }

        return response;
    }

    private async Task<string?> GetOrAuthenticateTokenAsync(long enterpriseId, EnumMicroservice microservice)
    {
        var auth = microserviceAuthCache.TryGetValidAuth(enterpriseId, microservice);
        if (auth != null)
            return auth.Token;

        return await TryAuthenticate(enterpriseId, microservice);
    }

    private async Task<string?> TryAuthenticate(long enterpriseId, EnumMicroservice microservice)
    {
        var credentialResponse = await microserviceCredentialRefit.GetCredential(new InputCredential(enterpriseId, microservice));
        if (!credentialResponse.IsSuccessStatusCode || credentialResponse.Content == null)
            return null;

        var authResponse = await microserviceAuthenticationRefit.Login(new InputAuthenticate(credentialResponse.Content.ApplicationId, credentialResponse.Content.ContractId));
        if (!authResponse.IsSuccessStatusCode || authResponse.Content == null)
            return null;

        var token = authResponse.Content.Token;
        microserviceAuthCache.AddOrUpdateAuth(enterpriseId, microservice, new MicroserviceAuthentication(token));
        return token;
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