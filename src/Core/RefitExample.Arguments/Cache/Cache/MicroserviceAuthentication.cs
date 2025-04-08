namespace RefitExample.Arguments.Cache.Cache;

public class MicroserviceAuthentication(string token)
{
    public string Token { get; private set; } = token;
}