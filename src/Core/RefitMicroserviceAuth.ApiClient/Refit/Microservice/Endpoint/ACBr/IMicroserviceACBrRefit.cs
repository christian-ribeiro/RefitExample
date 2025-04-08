using Refit;
using RefitMicroserviceAuth.ApiClient.DataAnnotation;
using RefitMicroserviceAuth.ApiClient.Refit.Microservice.Interface;
using RefitMicroserviceAuth.Arguments.Enum.Microservice;

namespace RefitMicroserviceAuth.ApiClient.Refit.Microservice.Endpoint.ACBr;

[MicroserviceRefit(EnumMicroservice.ACBr)]
public interface IMicroserviceACBrRefit : IMicroserviceRefitInterface
{
    [Get("/api/ACBr")]
    Task<ApiResponse<string>> ACBr();
}