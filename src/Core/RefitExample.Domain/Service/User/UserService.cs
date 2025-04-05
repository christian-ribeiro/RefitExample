using RefitExampe.ApiClient.Refit.Microservice.Endpoint.User;
using RefitExample.Arguments.Argument.Refit.Microservice.Endpoint.Authentication;
using RefitExample.Domain.Interface.Service.User;

namespace RefitExample.Domain.Service.User;

public class UserService(IMicroserviceUserRefit microserviceUserRefit) : IUserService
{
    public async Task<OutputUserResponse?> GetUsers(int page)
    {
        var response = await microserviceUserRefit.GetUsers(page);
        if (!response.IsSuccessStatusCode)
        {
            // Tratar erros
        }

        return response.Content;
    }
}