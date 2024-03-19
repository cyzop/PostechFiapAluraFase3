using Microsoft.AspNetCore.Mvc;
using PosTech.CadPac.Domain.Entities;

namespace PosTech.CadPac.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TesteController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TesteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetPacientes()
        {
            var x = Environment.GetEnvironmentVariable("postechazappconfiguration");

            return Ok($"Enviroment ");
        }
    }
}
