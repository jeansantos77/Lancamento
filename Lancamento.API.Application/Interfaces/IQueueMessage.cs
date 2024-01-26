using System;

namespace Lancamento.API.Application.Interfaces
{
    public interface IQueueMessage
    {
        public DateTime Data { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
        public bool EhConsolidado { get; set; }
    }
}
