using RefitMicroserviceAuth.Arguments.Enum.Microservice;
using System.Text.Json.Serialization;

namespace RefitMicroserviceAuth.Arguments.Argument.Credential;

[method: JsonConstructor]
public record InputCredential(long EnterpriseId, EnumMicroservice Microservice);