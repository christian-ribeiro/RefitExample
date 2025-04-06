using System.Text.Json.Serialization;

namespace RefitExample.Arguments.Argument.Authenticate;

[method: JsonConstructor]
public class InputAuthenticate(Guid applicationId, Guid contractId)
{
    public Guid ApplicationId { get; private set; } = applicationId;
    public Guid ContractId { get; private set; } = contractId;
}