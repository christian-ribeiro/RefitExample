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
        new OutputCredential(1, EnumMicroservice.DrugTrafficking, Guid.NewGuid(), Guid.NewGuid()),
        new OutputCredential(1, EnumMicroservice.Pimp, Guid.NewGuid(), Guid.NewGuid()),
        new OutputCredential(2, EnumMicroservice.DrugTrafficking, Guid.NewGuid(), Guid.NewGuid()),
        new OutputCredential(3, EnumMicroservice.Pimp, Guid.NewGuid(), Guid.NewGuid()),
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