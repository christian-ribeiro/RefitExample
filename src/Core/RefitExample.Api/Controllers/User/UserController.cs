using Microsoft.AspNetCore.Mvc;
using RefitExample.ApiClient.Refit.Microservice.Handler;
using RefitExample.Arguments.Argument.Session;
using RefitExample.Domain.Interface.Service.User;

namespace RefitExample.Api.Controllers.User;

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

        Request.Headers.Append(MicroserviceHandler.GuidSessionDataRequest, _guidSessionDataRequest.ToString());

        var result = await userService.GetUsers();
        return Ok(result);
    }
}