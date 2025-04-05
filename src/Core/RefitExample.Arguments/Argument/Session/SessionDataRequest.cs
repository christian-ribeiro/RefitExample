namespace RefitExample.Arguments.Argument.Session;

public class SessionDataRequest
{
    public Guid GuidSessionDataRequest { get; } = Guid.NewGuid();
    public long? LoggerUserId { get; set; }
}