using MassTransit;
using PosTech.CadPac.Domain.Operation;
using PosTech.CadPac.Domain.Repositories;
using static PosTech.CadPac.Domain.Shared.Enum.Enum;

namespace PosTech.CadPac.MicroService.Consumer.Eventos
{
    public class PatienteOperationConsumer : IConsumer<PacienteOperation>
    {
        private readonly IPacienteRepository _repository;

        public PatienteOperationConsumer(IPacienteRepository repository)
        {
            _repository = repository;
        }

        public Task Consume(ConsumeContext<PacienteOperation> context)
        {
            Console.WriteLine($"{context?.Message.Operation.ToString()} {context.Message.Paciente?.Email}");

            switch (context.Message.Operation)
            {
                case OperationType.Insert:
                case OperationType.Update:
                    _repository.UpSert(context.Message.Paciente);
                    break;
                case OperationType.Remove:
                    _repository.Delete(context.Message.Paciente.Id);
                    break;
            }
            
            return Task.CompletedTask;
        }
    }
}
