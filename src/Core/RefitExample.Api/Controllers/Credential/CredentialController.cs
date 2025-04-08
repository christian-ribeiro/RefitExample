using Microsoft.AspNetCore.Mvc;
using RefitExample.Arguments.Argument.Credential;
using RefitExample.Arguments.Enum.Microservice;

namespace RefitExample.Api.Controllers.Credential;

[ApiController]
[Route("/api/[controller]")]
public class CredentialController : Controller
{
    /// <summary>
    /// Dados estáticos simulando registros cadastrados para as empresas
    /// </summary>
    public static readonly List<OutputCredential> _credentialList =
    [
        new OutputCredential(1, EnumMicroservice.ACBr, Guid.Parse("51943448-1c1a-43bd-9777-603c423b74ed"), Guid.Parse("58fb96eb-b627-4ce5-b7a3-16d4198989db")),
        new OutputCredential(1, EnumMicroservice.Mercos, Guid.Parse("b8741b61-d623-475c-81d8-344267bdf00b"), Guid.Parse("ecac32b8-0c97-4665-993f-07ca49f4bc02")),
        new OutputCredential(2, EnumMicroservice.ACBr, Guid.Parse("34d5f313-1608-4f0b-97d2-5dea2e29eeb2"), Guid.Parse("28419aba-1178-439d-8c75-d454984891ed")),
        new OutputCredential(3, EnumMicroservice.Mercos, Guid.Parse("29f4a126-6231-4ba6-b401-8e0b0bc95c13"), Guid.Parse("2dbf2c0e-384f-492d-b300-c75304f9e693")),
    ];

    /// <summary>
    /// Endpoint simulando a Área Admin, onde receberá o id da empresa e retornará as credenciais para serem enviadas para o Microservice
    /// </summary>
    /// <param name="credential"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<OutputCredential> GetCredential([FromBody] InputCredential credential)
    {
        try
        {
            var response = _credentialList.FirstOrDefault(x => x.EnterpriseId == credential.EnterpriseId && x.Microservice == credential.Microservice);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}