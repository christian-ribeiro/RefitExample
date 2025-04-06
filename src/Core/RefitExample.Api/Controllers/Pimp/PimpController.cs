using Microsoft.AspNetCore.Mvc;

namespace RefitExample.Api.Controllers.Pimp;

[ApiController]
[Route("/api/[controller]")]
public class PimpController : Controller
{
    /// <summary>
    /// Endpoint consumido pelo Microservice Pimp
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<string> Pimp()
    {
        if (string.IsNullOrEmpty(Request.Headers.Authorization.ToString()))
            return Unauthorized();

        return Ok("Pimp");
    }
}