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
    public class LactoService: ILactoService
    {
        private readonly IMapper _mapper;
        private readonly ILactoRepository _lactoRepository;
        private readonly IQueueService _queueService;

        public LactoService(IMapper mapper
                          , ILactoRepository lactoRepository
                          , IQueueService queueService)
        {
            _mapper = mapper;
            _lactoRepository = lactoRepository;
            _queueService = queueService;
        }

        public async Task Add(LactoAddModel entidade)
        {
            string camposObrigatorios = string.Empty;

            if (string.IsNullOrEmpty(entidade.Descricao))
            {
                camposObrigatorios += "Descrição deve ser informada!" + Environment.NewLine;
            }

            if (entidade.Valor == 0)
            {
                camposObrigatorios += "Valor deve ser informado!" + Environment.NewLine;
            }

            if (camposObrigatorios.Length > 0)
            {
                throw new Exception(camposObrigatorios);
            }

            Lacto model = _mapper.Map<Lacto>(entidade);
            await _lactoRepository.Add(model);

            MessageQueue msgQueue = new MessageQueue
            {
                Data = model.Data,
                Credito = (model.Tipo == "C") ? model.Valor : 0,
                Debito = (model.Tipo == "D") ? model.Valor : 0
            };

            _queueService.PublishMessage(msgQueue);
        }

        public async Task Delete(int id)
        {
            LactoModel model = await GetById(id);

            Lacto entidade = _mapper.Map<Lacto>(model);

            await _lactoRepository.Delete(entidade);
        }

        public async Task<List<LactoModel>> GetAllLancamentos()
        {
            var entidades = await _lactoRepository.GetAll(t => t.Id > 0);
            return _mapper.Map<List<LactoModel>>(entidades);
        }

        public async Task<LactoModel> GetById(int id)
        {
            Lacto entidade = await _lactoRepository.GetById(id);

            if (entidade == null)
            {
                throw new Exception($"Lancamento com [ Id = {id}] não encontrado.");
            }

            return _mapper.Map<LactoModel>(entidade);
        }

        public async Task Update(LactoModel entidade)
        {
            Lacto entity = _mapper.Map<Lacto>(entidade);
            await _lactoRepository.Update(entity);
        }
    }
}
