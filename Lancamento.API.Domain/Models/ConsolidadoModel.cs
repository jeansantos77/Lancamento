using Lancamento.API.Domain.Interfaces;
using System;

namespace Lancamento.API.Domain.Models
{
    public class ConsolidadoModel: ILacto
    {
        public DateTime Data { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
    }
}
