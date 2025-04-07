using Refit;
using RefitExample.Arguments.Argument.Credential;

namespace RefitExample.ApiClient.Refit.Microservice.MockEndpoint.Credential;

public interface IMicroserviceCredentialRefit
{
    [Post("/api/Credential")]
    Task<ApiResponse<OutputCredential>> GetCredential([Body] InputCredential inputCredential);
}