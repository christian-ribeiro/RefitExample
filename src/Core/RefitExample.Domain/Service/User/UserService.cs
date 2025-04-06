using RefitExample.ApiClient.Refit.Microservice.Endpoint.DrugTrafficking;
using RefitExample.ApiClient.Refit.Microservice.Endpoint.Pimp;
using RefitExample.Arguments.Argument.Refit.Microservice.Endpoint.Authentication;
using RefitExample.Domain.Interface.Service.User;

namespace RefitExample.Domain.Service.User;

public class UserService(IMicroserviceDrugTraffickingRefit microserviceDrugTraffickingRefit, IMicroservicePimpRefit microservicePimpRefit) : IUserService
{
    public async Task<OutputUserResponse?> GetUsers(int page)
    {
        var response = await microserviceDrugTraffickingRefit.GetUsers(page);
        if (!response.IsSuccessStatusCode)
        {
            // Tratar erros
        }

        var response2 = await microservicePimpRefit.GetUsers(page);
        if (!response2.IsSuccessStatusCode)
        {
            // Tratar erros
        }

        return response.Content;
    }
}