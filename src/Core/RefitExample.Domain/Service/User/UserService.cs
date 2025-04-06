using RefitExample.ApiClient.Refit.Microservice.Endpoint.DrugTrafficking;
using RefitExample.ApiClient.Refit.Microservice.Endpoint.Pimp;
using RefitExample.Domain.Interface.Service.User;

namespace RefitExample.Domain.Service.User;

public class UserService(IMicroserviceDrugTraffickingRefit microserviceDrugTraffickingRefit, IMicroservicePimpRefit microservicePimpRefit) : IUserService
{
    public async Task<List<string>> GetUsers()
    {
        var content = new List<string>();
        var response = await microserviceDrugTraffickingRefit.GetUsers();
        if (!response.IsSuccessStatusCode)
        {
            // Tratar erros
        }

        var response2 = await microservicePimpRefit.GetUsers();
        if (!response2.IsSuccessStatusCode)
        {
            // Tratar erros
        }

        content.Add(response.Content!);
        content.Add(response2.Content!);

        return content;
    }
}