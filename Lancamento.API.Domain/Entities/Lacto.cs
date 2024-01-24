using System;

namespace Lancamento.API.Domain.Entities
{
    public class Lacto: Base
    {
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
