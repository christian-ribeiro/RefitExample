using RefitExample.Arguments.Argument.Refit.Microservice.Endpoint.Authentication;

namespace RefitExample.Domain.Interface.Service.User;

public interface IUserService
{
    Task<OutputUserResponse?> GetUsers(int page);
}