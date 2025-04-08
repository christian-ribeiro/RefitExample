namespace RefitMicroserviceAuth.Domain.Interface.Service.MicroserviceGateway;

public interface IMicroserviceGatewayService
{
    Task<List<string>> ConsumeService();
}