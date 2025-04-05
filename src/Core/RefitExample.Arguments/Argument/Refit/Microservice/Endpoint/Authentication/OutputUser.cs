using System.Text.Json.Serialization;

namespace RefitExample.Arguments.Argument.Refit.Microservice.Endpoint.Authentication;

[method: JsonConstructor]
public class OutputUser(int id, string email, string first_Name, string last_Name, string avatar)
{
    public int Id { get; private set; } = id;
    public string Email { get; private set; } = email;
    public string First_Name { get; private set; } = first_Name;
    public string Last_Name { get; private set; } = last_Name;
    public string Avatar { get; private set; } = avatar;
}