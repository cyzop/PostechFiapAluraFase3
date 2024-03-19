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
    public class MedicalHistoryController : ControllerBase
    {
        private IBus _bus;
        private readonly ILogger<MedicalHistoryController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _medicalHistoryQueueName;

        public MedicalHistoryController(IBus bus, ILogger<MedicalHistoryController> logger, IConfiguration configuration)
        {
            _bus = bus;
            _logger = logger;
            _configuration = configuration;

            var _azureAppConfigurationValue = _configuration["postechcadpac:masstransit:azurebus"];

            _medicalHistoryQueueName = _configuration.GetSection("MedicalHistoryAzureBus")["QueueName"] ?? string.Empty;
        }

        /// <summary>
        /// Excluir o lançamento médico do histórico médico do paciente
        /// </summary>
        /// <param name="idPaciente">Identificador do Paciente</param>
        /// <param name="id">Identificador do Lançamento Médico</param>
        /// <response code="200">Exclusão realizada com sucesso</response>
        /// <response code="404">Lançamento médico não encontrado para o Paciente</response>
        [HttpDelete]
        [Route("{idPaciente}/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteLancamento(string idPaciente, string id)
        {
            try
            {
                if (ModelState.IsValid) {

                    _logger.LogInformation($"DeleteLancamento idPaciente: {idPaciente} idLancamento: {id}");
                    
                    var lancamento = new RegistroMedico(
                        id, 
                        DateTime.Now, 
                        string.Empty, 
                        TipoRegistroMedico.Tratamento);

                    var operacao = new MedicalHistoryOperation(
                        OperationType.Remove, 
                        lancamento,
                        idPaciente);

                    var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{_medicalHistoryQueueName}"));
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
                _logger.LogError(ex, "DeleteLancamento {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Incluir lançamento médico no histórico médico do paciente
        /// </summary>
        /// <param name="idPaciente">Identificador do Paciente</param>
        /// <param name="lancamento">Lançamento métido para o histórico do paciente</param>
        /// <response code="200">Exclusão realizada com sucesso</response>
        /// <response code="404">Lançamento médico não encontrado para o Paciente</response>
        [HttpPost]
        [Route("idPaciente")]
        [Authorize]
        public async Task<IActionResult> PostLancamento(string idPaciente, [FromBody] RegistroMedico lancamento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation($"PostLancamento idPaciente: {idPaciente} RegistroMedico: {lancamento}");

                    var operacao = new MedicalHistoryOperation(
                        OperationType.Insert,
                        lancamento,
                        idPaciente);

                    var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{_medicalHistoryQueueName}"));
                    await endpoint.Send(operacao);

                    return Ok();
                }
                else
                {
                    var erros = ModelState.Values
                        .Where(x => x.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                        .Select(x => x.Errors?.FirstOrDefault()?.ErrorMessage).ToList();
                    return BadRequest(new
                    {
                        PayloadErros = erros
                    });
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "PostLancamento {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
