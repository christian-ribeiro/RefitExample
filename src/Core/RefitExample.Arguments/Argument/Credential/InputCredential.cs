using RefitExample.Arguments.Enum.Microservice;
using System.Text.Json.Serialization;

namespace RefitExample.Arguments.Argument.Credential;

[method: JsonConstructor]
public record InputCredential(long EnterpriseId, EnumMicroservice Microservice);