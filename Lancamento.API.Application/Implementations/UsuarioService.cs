using AutoMapper;
using Lancamento.API.Application.Interfaces;
using Lancamento.API.Domain.Entities;
using Lancamento.API.Domain.Interfaces;
using Lancamento.API.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lancamento.API.Application.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IMapper _mapper;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IMapper mapper, IUsuarioRepository usuarioRepository)
        {
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
        }

        public async Task Add(UsuarioAddModel entidade)
        {
            if (entidade.Nome == null)
            {
                throw new Exception("Nome deve ser informado!");
            }

            Usuario model = _mapper.Map<Usuario>(entidade);
            await _usuarioRepository.Add(model);
        }

        public async Task Delete(int id)
        {
            UsuarioModel model = await GetById(id);

            Usuario entidade = _mapper.Map<Usuario>(model);

            await _usuarioRepository.Delete(entidade);
        }

        public async Task<List<UsuarioModel>> GetAllUsuarios()
        {
            var entidades = await _usuarioRepository.GetAll(t => t.Id > 0);
            return _mapper.Map<List<UsuarioModel>>(entidades);
        }

        public async Task<UsuarioModel> GetById(int id)
        {
            Usuario entidade = await _usuarioRepository.GetById(id);

            if (entidade == null)
            {
                throw new Exception($"Usuario com [ Id = {id}] não encontrado.");
            }

            return _mapper.Map<UsuarioModel>(entidade);
        }

        public async Task Update(UsuarioModel entidade)
        {
            Usuario entity = _mapper.Map<Usuario>(entidade);
            await _usuarioRepository.Update(entity);
        }
    }
}
