using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PosTech.CadPac.Domain.Entities;
using PosTech.CadPac.Domain.Operation;
using static PosTech.CadPac.Domain.Shared.Enum.Enum;

namespace PosTech.CadPac.Producer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private IBus _bus;
        private readonly ILogger<PatientController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _patientQueueName;

        public PatientController(ILogger<PatientController> logger, IConfiguration configuration, IBus bus)
        {
            _logger = logger;
            _configuration = configuration;
            _bus = bus;

            _patientQueueName = _configuration.GetSection("PatientAzureBus")["QueueName"] ?? string.Empty;
        }

        /// <summary>
        /// Atualiza as informações do Paciente
        /// </summary>
        /// <param name="paciente">Json representando as informações do Paciente</param>
        /// <response code="200">Paciente atualizado com sucesso</response>
        /// <response code="404">Paciente não encontrado</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Paciente))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> PutPaciente(Paciente paciente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //update
                    _logger.LogInformation("PutPaciente {Nome}", paciente.Nome);
                    
                    var operacao = new PacienteOperation(
                        OperationType.Update, 
                        paciente);

                    var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{_patientQueueName}"));
                    await endpoint.Send(operacao);

                    return Ok();
                }
                else
                {
                    var erros = ModelState.Values
                        .Where(x => x.ValidationState == ModelValidationState.Invalid)
                        .Select(x => x.Errors?.FirstOrDefault()?.ErrorMessage).ToList();
                    return BadRequest(new
                    {
                        PayloadErros = erros
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Inclusão de um novo Paciente
        /// </summary>
        /// <param name="paciente">Json representando as informações do Paciente</param>
        /// <response code="200">Paciente atualizado com sucesso</response>
        /// <response code="400">Falha na inclusão do Paciente</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Paciente))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> PostPaciente(Paciente paciente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //include
                    _logger.LogInformation("PostPaciente {Nome}", paciente.Nome);
                    
                    var operacao = new PacienteOperation(
                        OperationType.Insert, 
                        paciente);

                    var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{_patientQueueName}"));
                    await endpoint.Send(operacao);

                    return Ok();
                }
                else
                {
                    var erros = ModelState.Values
                        .Where(x => x.ValidationState == ModelValidationState.Invalid)
                        .Select(x => x.Errors?.FirstOrDefault()?.ErrorMessage).ToList();
                    return BadRequest(new
                    {
                        PayloadErros = erros
                    });
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exclusão de um paciente
        /// </summary>
        /// <param name="id">Identificador do Paciente</param>
        /// <response code="200">Paciente excluído com sucesso</response>
        /// <response code="400">Falha na exclusão do Paciente</response>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePaciente(string id)
        {
            try
            {
                var paciente = new Paciente(id, string.Empty, DateTime.Now, string.Empty, string.Empty);
                   
                //remove
                _logger.LogInformation("DeletePaciente {Id}", id);
                    
                var operacao = new PacienteOperation(
                    OperationType.Remove, 
                    paciente);

                var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{_patientQueueName}"));
                await endpoint.Send(operacao);

                return Ok();
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
