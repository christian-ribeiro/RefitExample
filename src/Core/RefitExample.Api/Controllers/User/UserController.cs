using Microsoft.AspNetCore.Mvc;
using RefitExample.Domain.Interface.Service.User;

namespace RefitExample.Api.Controllers.User;

[ApiController]
[Route("/api/[controller]")]
public class UserController(IUserService userService) : Controller
{
    private readonly Guid _guidSessionDataRequest = Guid.NewGuid();

    [HttpGet]
    public async Task<ActionResult> GetUsers(int page = 1)
    {
        Request.Headers.Append("GuidSessionDataRequest", _guidSessionDataRequest.ToString());

        var result = await userService.GetUsers(page);
        return Ok(result);
    }
}