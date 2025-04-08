namespace RefitMicroserviceAuth.Domain.Interface.Service.User;

public interface IUserService
{
    Task<List<string>> GetUsers();
}