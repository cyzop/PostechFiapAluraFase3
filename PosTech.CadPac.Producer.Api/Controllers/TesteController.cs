using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosTech.CadPac.Producer.Api.Authentication;

namespace PosTech.CadPac.Producer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TesteController : ControllerBase
    {
        private readonly AuthenticationCredentials _authInfo;

        public TesteController(AuthenticationCredentials authInfo)
        {
            _authInfo = authInfo;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetTeste()
        {
            return Ok($"Acesso autenticado! {_authInfo?.ClientId}");
        }
    }
}
