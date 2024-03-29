﻿using Microsoft.VisualBasic;
using PosTech.CadPac.Domain.Shared.Entities;
using static PosTech.CadPac.Domain.Shared.Enum.Enum;

namespace PosTech.CadPac.Domain.Entities
{
    public class RegistroMedico : Entity
    {
        public RegistroMedico(string id, DateTime data, string texto, TipoRegistroMedico tipo)
        {
            Id = id;
            Data = data;
            Texto = texto;
            Tipo = tipo;
        }

        public string Id { 
            get; 
            private set; }
        public DateTime Data { 
            get; 
            private set; }

        public string Texto { 
            get; 
            private set; }

        public TipoRegistroMedico Tipo { 
            get; 
            private set; }

        public void SetId(string id)
        {
            this.Id = id;
        }
        public void SetData(DateTime data) { 
            this.Data = data;
        }

        public void SetTexto(string texto)
        {
            this.Texto = texto; 
        }
        public void SetTipo(TipoRegistroMedico tipo)
        {
            this.Tipo = tipo;   
        }

        public override string ToString() =>
         $"Id: {Id} Data: {Data:dd/MM/yyyy} Tipo:{Tipo} Texto: {Texto}";
        
    }
}
