using Microsoft.AspNetCore.Mvc;
using RefitExample.ApiClient.Refit.Microservice.Handler;
using RefitExample.Arguments.Argument.Session;
using RefitExample.Domain.Interface.Service.User;

namespace RefitExample.Api.Controllers.User;

[ApiController]
[Route("/api/[controller]")]
public class UserController(IUserService userService) : Controller
{
    [HttpGet]
    public async Task<ActionResult<string>> GetUsers()
    {
        Guid _guidSessionDataRequest = SessionData.Initialize();
        SessionData.SetLoggedEnterprise(_guidSessionDataRequest, 1);

        Request.Headers.Append(MicroserviceHandler.GuidSessionDataRequest, _guidSessionDataRequest.ToString());

        var result = await userService.GetUsers();
        return Ok(result);
    }

    [HttpGet("Drug")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<string> Drug()
    {
        if (string.IsNullOrEmpty(Request.Headers.Authorization.ToString()))
            return Unauthorized();
        return Ok("Drug");
    }

    [HttpGet("Pimp")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<string> Pimp()
    {
        if (string.IsNullOrEmpty(Request.Headers.Authorization.ToString()))
            return Unauthorized();

        return Ok("Pimp");
    }

}