using Refit;
using RefitMicroserviceAuth.Arguments.Argument.Credential;

namespace RefitMicroserviceAuth.ApiClient.Refit.MockEndpoint.Credential;

public interface IMicroserviceCredentialRefit
{
    [Post("/api/Credential")]
    Task<ApiResponse<OutputCredential>> GetCredential([Body] InputCredential inputCredential);
}