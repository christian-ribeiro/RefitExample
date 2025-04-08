using RefitMicroserviceAuth.ApiClient.Refit.Microservice.Endpoint.ACBr;
using RefitMicroserviceAuth.ApiClient.Refit.Microservice.Endpoint.Mercos;
using RefitMicroserviceAuth.Domain.Interface.Service.MicroserviceGateway;

namespace RefitMicroserviceAuth.Domain.Service.MicroserviceGateway;

public class MicroserviceGatewayService(IMicroserviceACBrRefit microserviceACBrRefit, IMicroserviceMercosRefit microserviceMercosRefit) : IMicroserviceGatewayService
{
    /// <summary>
    /// Exemplo de um Service que irá consumir um Microservice
    /// </summary>
    /// <returns></returns>
    public async Task<List<string>> ConsumeService()
    {
        var content = new List<string>();
        var response = await microserviceACBrRefit.ACBr();
        if (response.IsSuccessStatusCode)
        {
            content.Add(response.Content!);
        }
        else
        {
            // Tratar erros
        }

        var response2 = await microserviceMercosRefit.Mercos();
        if (response2.IsSuccessStatusCode)
        {
            content.Add(response2.Content!);
        }
        else
        {
            // Tratar erros
        }

        return content;
    }
}