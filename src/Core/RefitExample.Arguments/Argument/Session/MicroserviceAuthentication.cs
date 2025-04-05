using RefitExample.Arguments.Enum.Microservice;

namespace RefitExample.Arguments.Argument.Session;

public class MicroserviceAuthentication(EnumMicroservice microservice, long entepriseId, string token)
{
    public EnumMicroservice Microservice { get; private set; } = microservice;
    public long EntepriseId { get; private set; } = entepriseId;
    public string Token { get; private set; } = token;
}