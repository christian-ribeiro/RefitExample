using Microsoft.AspNetCore.Mvc;
using RefitExample.Api.Controllers.Credential;
using RefitExample.Arguments.Argument.Authenticate;

namespace RefitExample.Api.Controllers.Authenticate;

[ApiController]
[Route("/api/[controller]")]
public class AuthenticateController : Controller
{
    /// <summary>
    /// Endpoint simulando o Microservice, onde receberá as chaves e devolverá o Token
    /// </summary>
    /// <param name="inputAuthenticate"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<OutputAuthenticate> Authenticate(InputAuthenticate inputAuthenticate)
    {
        var credential = CredentialController._credentialList.FirstOrDefault(x => x.ApplicationId == inputAuthenticate.ApplicationId && x.ContractId == inputAuthenticate.ContractId);

        if (credential == null)
        {
            return Unauthorized();
        }

        return Ok(new OutputAuthenticate(Guid.NewGuid().ToString()));
    }
}