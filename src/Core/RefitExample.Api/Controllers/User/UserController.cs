using Microsoft.AspNetCore.Mvc;
using RefitExample.Domain.Interface.Service.User;

namespace RefitExample.Api.Controllers.User;

[ApiController]
[Route("/api/[controller]")]
public class UserController(IUserService userService) : Controller
{
    [HttpGet]
    public async Task<ActionResult> GetUsers(int page = 1)
    {
        var result = await userService.GetUsers(page);
        return Ok(result);
    }
}