using System;

namespace Lancamento.API.Domain.Interfaces
{
    public interface IConsolidadoModel
    {
        public DateTime Data { get; set; }
        public decimal Creditos { get; set; }
        public decimal Debitos { get; set; }
    }
}
