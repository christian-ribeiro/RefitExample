using Refit;
using RefitExample.ApiClient.DataAnnotation;
using RefitExample.ApiClient.Refit.Microservice.Interface;
using RefitExample.Arguments.Enum.Microservice;

namespace RefitExample.ApiClient.Refit.Microservice.Endpoint.ACBr;

[MicroserviceRefit(EnumMicroservice.ACBr)]
public interface IMicroserviceACBrRefit : IMicroserviceRefitInterface
{
    [Get("/api/ACBr")]
    Task<ApiResponse<string>> ACBr();
}