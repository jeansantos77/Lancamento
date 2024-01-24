using System;

namespace Lancamento.API.Domain.Models
{
    public class LactoAddModel
    {
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
        public int UsuarioId { get; set; }
    }
}
