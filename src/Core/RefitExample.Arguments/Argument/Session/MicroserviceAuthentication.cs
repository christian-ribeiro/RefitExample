namespace RefitExample.Arguments.Argument.Session;

public class MicroserviceAuthentication(string token)
{
    public string Token { get; private set; } = token;
}