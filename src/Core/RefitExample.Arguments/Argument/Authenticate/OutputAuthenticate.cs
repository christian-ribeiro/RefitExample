namespace RefitExample.Arguments.Argument.Authenticate;

public class OutputAuthenticate(string token)
{
    public string Token { get; private set; } = token;
}