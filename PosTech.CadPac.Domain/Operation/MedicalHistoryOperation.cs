using PosTech.CadPac.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PosTech.CadPac.Domain.Shared.Enum.Enum;

namespace PosTech.CadPac.Domain.Operation
{
    public class MedicalHistoryOperation
    {
        public OperationType Operation
        {
            get;
            private set;
        }

        public RegistroMedico MedicalReg
        {
            get;
            private set;
        }

        public string PatientId { get; private set; }

        public MedicalHistoryOperation(OperationType operation, RegistroMedico medicalReg, string patientId)
        {
            Operation = operation;
            MedicalReg = medicalReg;
            PatientId = patientId;
        }

        public override string ToString()=> $"PacienteId {PatientId} | Operação {Operation.ToString()} | Tipo: {MedicalReg.Tipo} - {MedicalReg.Texto}";
        
    }
}
