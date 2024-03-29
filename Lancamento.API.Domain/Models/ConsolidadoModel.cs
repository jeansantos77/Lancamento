﻿using Lancamento.API.Domain.Interfaces;
using System;

namespace Lancamento.API.Domain.Models
{
    public class ConsolidadoModel: IConsolidadoModel
    {
        public DateTime Data { get; set; }
        public decimal Creditos { get; set; }
        public decimal Debitos { get; set; }
    }
}
