using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosTech.CadPac.Domain.Services;
using PosTech.CadPac.Producer.Api.Model;

namespace PosTech.CadPac.Producer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly ITokenService tokenService;

        public AuthController(ILogger<AuthController> logger, ITokenService tokenService)
        {
            this.logger = logger;
            this.tokenService = tokenService;
        }

        [HttpPost]
        [Route("Token")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] AuthCredential clientAuth)
        {
            var isValid = ModelState.IsValid;
            logger.LogInformation("Login is {boolean} Worker running at: {time}", isValid, DateTimeOffset.UtcNow);
            
            if (isValid)
                return Ok(tokenService.CreateAuthenticationToken(clientId: clientAuth.clientId));
            else
                return BadRequest("Autenticação inválida");
            
            return Ok();

        }
    }
}
