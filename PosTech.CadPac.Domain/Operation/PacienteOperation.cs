using PosTech.CadPac.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PosTech.CadPac.Domain.Shared.Enum.Enum;

namespace PosTech.CadPac.Domain.Operation
{
    public class PacienteOperation
    {
        public OperationType Operation { get;
            private set;
        }

        public Paciente Paciente { get; 
            private set; }

        public PacienteOperation(OperationType operation, Paciente paciente)
        {
            Operation = operation;
            Paciente = paciente;
        }
    }
}
