using System;

namespace Lancamento.API.Domain.Models
{
    public class MessageQueue
    {
        public DateTime Data { get; set; }
        public decimal Credito { get; set; }
        public decimal Debito { get; set; }
        public bool EhConsolidado { get; set; }

    }
}
