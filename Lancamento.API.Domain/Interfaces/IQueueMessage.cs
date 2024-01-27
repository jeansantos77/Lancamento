﻿using System;

namespace Lancamento.API.Domain.Interfaces
{
    public interface IQueueMessage
    {
        public DateTime Data { get; set; }
        public decimal Creditos { get; set; }
        public decimal Debitos { get; set; }
        public bool Atualizar { get; set; }
    }
}