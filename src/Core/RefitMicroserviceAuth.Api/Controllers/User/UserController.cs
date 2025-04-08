using Microsoft.AspNetCore.Mvc;
using RefitMicroserviceAuth.Arguments.Argument.Session;
using RefitMicroserviceAuth.Arguments.Const;
using RefitMicroserviceAuth.Domain.Interface.Service.User;

namespace RefitMicroserviceAuth.Api.Controllers.User;

[ApiController]
[Route("/api/[controller]")]
public class UserController(IUserService userService) : Controller
{
    /// <summary>
    /// Endpoint da API principal que consumirá os Microservices
    /// </summary>
    /// <param name="enterpriseId"></param>
    /// <returns></returns>
    [HttpGet("{enterpriseId}")]
    public async Task<ActionResult<List<string>>> GetUsers(long enterpriseId)
    {
        Guid _guidSessionDataRequest = SessionData.Initialize();
        SessionData.SetLoggedEnterprise(_guidSessionDataRequest, enterpriseId);

        Request.Headers.Append(ConfigurationConst.GuidSessionDataRequest, _guidSessionDataRequest.ToString());

        var result = await userService.GetUsers();
        return Ok(result);
    }
}