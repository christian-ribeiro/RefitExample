namespace RefitMicroserviceAuth.ApiClient.Refit.Microservice.Handler;

public class MicroserviceAuthenticationHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return base.SendAsync(request, cancellationToken);
    }
}