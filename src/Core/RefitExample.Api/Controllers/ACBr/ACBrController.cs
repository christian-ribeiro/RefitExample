using Microsoft.AspNetCore.Mvc;

namespace RefitExample.Api.Controllers.ACBr;

[ApiController]
[Route("gateway/ACBr/api/[controller]")]
public class ACBrController : Controller
{
    /// <summary>
    /// Endpoint consumido pelo Microservice ACBr
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<string> ACBr()
    {
        if (string.IsNullOrEmpty(Request.Headers.Authorization.ToString()))
            return Unauthorized();

        return Ok("ACBr");
    }
}
