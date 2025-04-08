using Microsoft.AspNetCore.Mvc;
using RefitMicroserviceAuth.Arguments.Argument.Session;
using RefitMicroserviceAuth.Arguments.Const;
using RefitMicroserviceAuth.Domain.Interface.Service.MicroserviceGateway;

namespace RefitMicroserviceAuth.Api.Controllers.MicroserviceGateway;

[ApiController]
[Route("/api/[controller]")]
public class MicroserviceGatewayController(IMicroserviceGatewayService microserviceGatewayService) : Controller
{
    /// <summary>
    /// Endpoint da API principal que consumirá os Microservices
    /// </summary>
    /// <param name="enterpriseId"></param>
    /// <returns></returns>
    [HttpGet("{enterpriseId}")]
    public async Task<ActionResult<List<string>>> ConsumeService(long enterpriseId)
    {
        Guid _guidSessionDataRequest = SessionData.Initialize();
        SessionData.SetLoggedEnterprise(_guidSessionDataRequest, enterpriseId);

        Request.Headers.Append(ConfigurationConst.GuidSessionDataRequest, _guidSessionDataRequest.ToString());

        var result = await microserviceGatewayService.ConsumeService();
        return Ok(result);
    }
}