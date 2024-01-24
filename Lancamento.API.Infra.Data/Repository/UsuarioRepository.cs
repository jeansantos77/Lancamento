using Lancamento.API.Domain.Entities;
using Lancamento.API.Domain.Interfaces;
using Lancamento.API.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Lancamento.API.Infra.Data.Repository
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(LancamentoContext context) : base(context)
        {
        }

        public override async Task<Usuario> GetById(int id)
        {
            return await _dbContext.Set<Usuario>().Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
