using RefitExample.ApiClient.Refit.Microservice.Endpoint.DrugTrafficking;
using RefitExample.ApiClient.Refit.Microservice.Endpoint.Pimp;
using RefitExample.Domain.Interface.Service.User;

namespace RefitExample.Domain.Service.User;

public class UserService(IMicroserviceDrugTraffickingRefit microserviceDrugTraffickingRefit, IMicroservicePimpRefit microservicePimpRefit) : IUserService
{
    /// <summary>
    /// Exemplo de um Service que irá consumir um Microservice
    /// </summary>
    /// <returns></returns>
    public async Task<List<string>> GetUsers()
    {
        var content = new List<string>();
        var response = await microserviceDrugTraffickingRefit.DrugTrafficking();
        if (response.IsSuccessStatusCode)
        {
            content.Add(response.Content!);
        }
        else
        {
            // Tratar erros
        }

        var response2 = await microservicePimpRefit.Pimp();
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