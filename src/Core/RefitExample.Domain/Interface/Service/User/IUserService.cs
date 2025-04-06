namespace RefitExample.Domain.Interface.Service.User;

public interface IUserService
{
    Task<string?> GetUsers();
}