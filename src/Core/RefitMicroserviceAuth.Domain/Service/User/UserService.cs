using RefitMicroserviceAuth.ApiClient.Refit.Microservice.Endpoint.ACBr;
using RefitMicroserviceAuth.ApiClient.Refit.Microservice.Endpoint.Mercos;
using RefitMicroserviceAuth.Domain.Interface.Service.User;

namespace RefitMicroserviceAuth.Domain.Service.User;

public class UserService(IMicroserviceACBrRefit microserviceACBrRefit, IMicroserviceMercosRefit microserviceMercosRefit) : IUserService
{
    /// <summary>
    /// Exemplo de um Service que irá consumir um Microservice
    /// </summary>
    /// <returns></returns>
    public async Task<List<string>> GetUsers()
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