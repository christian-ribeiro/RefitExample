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
    public async Task<ActionResult> GetUsers(int page = 1)
    {
        Guid _guidSessionDataRequest = SessionData.Initialize();
        SessionData.SetLoggedUser(_guidSessionDataRequest, 1);

        Request.Headers.Append(MicroserviceHandler.GuidSessionDataRequest, _guidSessionDataRequest.ToString());

        var result = await userService.GetUsers(page);
        return Ok(result);
    }
}