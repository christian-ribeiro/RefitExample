namespace RefitMicroserviceAuth.Arguments.Argument.Session;

public class SessionDataRequest
{
    public Guid GuidSessionDataRequest { get; } = Guid.NewGuid();
    public long? EnterpriseId { get; set; }
}