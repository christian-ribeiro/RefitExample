using Microsoft.AspNetCore.Mvc;

namespace RefitExample.Api.Controllers.Mercos;

[ApiController]
[Route("gateway/Mercos/api/[controller]")]
public class MercosController : Controller
{
    /// <summary>
    /// Endpoint consumido pelo Microservice Mercos
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<string> Mercos()
    {
        if (string.IsNullOrEmpty(Request.Headers.Authorization.ToString()))
            return Unauthorized();

        return Ok("Mercos");
    }
}