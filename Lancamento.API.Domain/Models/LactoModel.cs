using Lancamento.API.Domain.Interfaces;

namespace Lancamento.API.Domain.Models
{
    public class LactoModel : LactoAddModel, ILacto
    {
        public int Id { get; set; }
    }
}
