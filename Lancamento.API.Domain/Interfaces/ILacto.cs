using System;

namespace Lancamento.API.Domain.Interfaces
{
    public interface ILacto
    {
        public DateTime Data { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
    }
}
