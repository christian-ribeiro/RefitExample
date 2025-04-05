using System.Text.Json.Serialization;

namespace RefitExample.Arguments.Argument.Refit.Microservice.Endpoint.Authentication;

[method: JsonConstructor]
public class OutputUserResponse(List<OutputUser> data)
{
    public List<OutputUser> Data { get; private set; } = data;
}