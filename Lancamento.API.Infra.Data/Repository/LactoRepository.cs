using Lancamento.API.Domain.Entities;
using Lancamento.API.Domain.Interfaces;
using Lancamento.API.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Lancamento.API.Infra.Data.Repository
{
    public class LactoRepository : BaseRepository<Lacto>, ILactoRepository
    {
        public LactoRepository(LancamentoContext context) : base(context)
        {

        }

        public override async Task<Lacto> GetById(int id)
        {
            return await _dbContext.Set<Lacto>().Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();

        }
    }
}
