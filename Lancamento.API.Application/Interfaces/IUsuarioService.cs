using Lancamento.API.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lancamento.API.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task Add(UsuarioAddModel entity);
        Task Update(UsuarioModel entity);
        Task Delete(int id);
        Task<UsuarioModel> GetById(int id);
        Task<List<UsuarioModel>> GetAllUsuarios();
    }
}
