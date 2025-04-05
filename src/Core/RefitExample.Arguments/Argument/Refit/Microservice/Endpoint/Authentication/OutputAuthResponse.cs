using System.Text.Json.Serialization;

namespace RefitExample.Arguments.Argument.Refit.Microservice.Endpoint.Authentication;

[method: JsonConstructor]
public class OutputAuthResponse(string token)
{
    public string Token { get; private set; } = token;
}