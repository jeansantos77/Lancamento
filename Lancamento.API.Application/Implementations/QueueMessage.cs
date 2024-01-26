using Lancamento.API.Application.Interfaces;
using System;

namespace Lancamento.API.Application.Implementations
{
    public class QueueMessage : IQueueMessage
    {
        public DateTime Data { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
        public bool EhConsolidado { get; set; }

        public QueueMessage()
        {
            EhConsolidado = false;
        }
    }
}
