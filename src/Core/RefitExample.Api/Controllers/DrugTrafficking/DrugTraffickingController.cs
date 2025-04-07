using Microsoft.AspNetCore.Mvc;

namespace RefitExample.Api.Controllers.DrugTrafficking;

[ApiController]
[Route("gateway/DrugTrafficking/api/[controller]")]
public class DrugTraffickingController : Controller
{
    /// <summary>
    /// Endpoint consumido pelo Microservice DrugTrafficking
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<string> DrugTrafficking()
    {
        if (string.IsNullOrEmpty(Request.Headers.Authorization.ToString()))
            return Unauthorized();

        return Ok("DrugTrafficking");
    }
}
