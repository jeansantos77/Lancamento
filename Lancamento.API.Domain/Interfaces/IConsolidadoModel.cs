using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lancamento.API.Domain.Interfaces
{
    public interface IConsolidadoModel
    {
        public DateTime Data { get; set; }
        public decimal Creditos { get; set; }
        public decimal Debitos { get; set; }
    }
}
