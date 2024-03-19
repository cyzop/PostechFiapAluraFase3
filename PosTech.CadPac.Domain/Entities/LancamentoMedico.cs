using PosTech.CadPac.Domain.Shared.Entities;

namespace PosTech.CadPac.Domain.Entities
{
    public class LancamentoMedico : Entity
    {
        public string PacienteId { get; private set; }  
        public RegistroMedico RegistroMedico { get; private set; }

        public LancamentoMedico(string paciente, RegistroMedico registroMedico)
        {
            PacienteId = paciente;
            RegistroMedico = registroMedico;
        }
    }
}
