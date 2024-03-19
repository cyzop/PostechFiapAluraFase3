using MassTransit;
using MassTransit.Initializers;
using Microsoft.Extensions.Configuration;
using PosTech.CadPac.Domain.Entities;
using PosTech.CadPac.Domain.Operation;
using PosTech.CadPac.Domain.Services;
using static PosTech.CadPac.Domain.Shared.Enum.Enum;

namespace PosTech.CadPac.ServiceProducer
{
    public class PacienteServiceProducer : IPatientServiceWriter
    {
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;

        public PacienteServiceProducer(IBus bus, IConfiguration configuration)
        {
            _bus = bus;
            _configuration = configuration;
        }

        public void RemovePaciente(string id)
        {
            var paciente = new Paciente(id, string.Empty, DateTime.Now, string.Empty, string.Empty);
            var operationmessage = new PacienteOperation(OperationType.Remove, paciente);

            //var fila = _configuration.

        }

        public async Task<Paciente> SavePacienteAsync(Paciente paciente)
        {
            var queueName = _configuration.GetSection("PatientAzureBus")["QueueName"] ?? string.Empty;
            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{queueName}"));
            await endpoint.Send(paciente);
            return paciente;
        }

        public Paciente UpdatePacienteData(Paciente paciente)
        {
            throw new NotImplementedException();
        }
    }
}