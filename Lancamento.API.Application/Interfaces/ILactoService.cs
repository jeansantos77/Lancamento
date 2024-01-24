using Lancamento.API.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lancamento.API.Application.Interfaces
{
    public interface ILactoService
    {
        Task Add(LactoAddModel entity);
        Task Update(LactoModel entity);
        Task Delete(int id);
        Task<LactoModel> GetById(int id);
        
        Task<List<LactoModel>> GetAllLancamentos();
    }
}
