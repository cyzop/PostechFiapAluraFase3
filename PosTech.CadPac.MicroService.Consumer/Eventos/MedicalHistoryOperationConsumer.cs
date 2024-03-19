using MassTransit;
using PosTech.CadPac.Domain.Operation;
using PosTech.CadPac.Domain.Repositories;
using static PosTech.CadPac.Domain.Shared.Enum.Enum;

namespace PosTech.CadPac.MicroService.Consumer.Eventos
{
    public class MedicalHistoryOperationConsumer : IConsumer<MedicalHistoryOperation>
    {
        private readonly IPacienteRepository _repository;

        public MedicalHistoryOperationConsumer(IPacienteRepository repository)
        {
            _repository = repository;
        }

        public Task Consume(ConsumeContext<MedicalHistoryOperation> context)
        {
            Console.WriteLine(context?.Message);

            switch (context.Message.Operation)
            {
                case OperationType.Insert:
                case OperationType.Update:
                    
                    if (string.IsNullOrEmpty(context.Message.MedicalReg.Id) || 
                        !Guid.TryParse(context.Message.MedicalReg.Id, out _))
                        context.Message.MedicalReg.SetId(Guid.NewGuid().ToString());

                    var pacienteUp = _repository.GetById(context.Message.PatientId);
                    pacienteUp.AddRegistroMedico(context.Message.MedicalReg);
                    _repository.UpSert(pacienteUp);

                    break;
                case OperationType.Remove:
                    
                    var paciente = _repository.GetById(context.Message.PatientId);
                    var lancamento = paciente.HistoricoMedico.Where(h => h.Id == context.Message.MedicalReg.Id).First();
                    if (lancamento != null)
                    {
                        paciente.DeleteRegistroMedico(lancamento);
                        _repository.UpSert(paciente);
                    }
                    break;
            }


            return Task.CompletedTask;  
        }
    }
}
