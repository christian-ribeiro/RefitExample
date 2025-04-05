namespace RefitExample.Arguments.Argument.Refit.Microservice.Endpoint.Authentication;

public class InputAuthenticateUser(string email, string password)
{
    public string Email { get; private set; } = email;
    public string Password { get; private set; } = password;
}