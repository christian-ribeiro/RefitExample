using RefitExample.Arguments.Enum.Microservice;
using System.Text.Json.Serialization;

namespace RefitExample.Arguments.Argument.Credential;

[method: JsonConstructor]
public class OutputCredential(long enterpriseId, EnumMicroservice microservice, Guid applicationId, Guid contractId)
{
    public long EnterpriseId { get; private set; } = enterpriseId;
    public EnumMicroservice Microservice { get; private set; } = microservice;
    public Guid ApplicationId { get; private set; } = applicationId;
    public Guid ContractId { get; private set; } = contractId;
}