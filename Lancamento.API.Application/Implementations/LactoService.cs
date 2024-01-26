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
            if (EntidadeIsValid(entidade))
            {
                Lacto lacto = _mapper.Map<Lacto>(entidade);
                await _lactoRepository.Add(lacto);

                IQueueMessage messageLacto = GenerateMessage(entidade);
                _queueService.PublishMessage(messageLacto);
            }
        }

        public async Task Delete(int id)
        {
            LactoModel model = await GetById(id);

            Lacto entidade = _mapper.Map<Lacto>(model);

            await _lactoRepository.Delete(entidade);

            IQueueMessage message = GenerateMessageInvertedRecord(model);
            _queueService.PublishMessage(message);
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

            LactoModel model = _mapper.Map<LactoModel>(entidade);

            return model;
        }

        public async Task Update(LactoModel entidade)
        {
            LactoModel lactoAnteriorModel = await GetById(entidade.Id);

            Lacto entity = _mapper.Map<Lacto>(entidade);
            await _lactoRepository.Update(entity);

            IQueueMessage messageLactoAnterior = GenerateMessageInvertedRecord(lactoAnteriorModel);

            IQueueMessage messageLactoAtual = GenerateMessage(entidade);

            _queueService.PublishMessage(messageLactoAnterior);
            _queueService.PublishMessage(messageLactoAtual);
        }

        public async Task<ConsolidadoModel> GetConsolidado(DateTime data)
        {
            List<Lacto> lancamentos = await _lactoRepository.GetAll(t => t.Data.Date == data);

            decimal creditos = 0;
            decimal debitos = 0;

            foreach (Lacto item in lancamentos)
            {
                creditos += (item.Tipo == "C") ? item.Valor : 0;
                debitos += (item.Tipo == "D") ? item.Valor : 0;
            }

            ConsolidadoModel model = new()
            {
                Data = data,
                Creditos = creditos,
                Debitos = debitos
            };

            return model;
        }

        public async Task<ConsolidadoModel> Reprocess(DateTime data)
        {
            ConsolidadoModel model = await GetConsolidado(data);

            _queueService.PublishMessage(GenerateMessage(model));

            return model;
        }

        private static bool EntidadeIsValid(LactoAddModel entidade)
        {
            string camposObrigatorios = string.Empty;

            if (string.IsNullOrEmpty(entidade.Descricao))
            {
                camposObrigatorios += "Descrição deve ser informada!" + Environment.NewLine;
            }

            if (entidade.Valor == 0)
            {
                camposObrigatorios += "Valor deve ser informado!";
            }

            if (camposObrigatorios.Length > 0)
            {
                throw new Exception(camposObrigatorios);
            }

            return true;
        }

        private static IQueueMessage GenerateMessage(ConsolidadoModel model)
        {
            return new QueueMessage
            {
                Data = model.Data,
                Creditos = model.Creditos,
                Debitos = model.Debitos,
                Atualizar = false
            };
        }

        private static IQueueMessage GenerateMessage(ILacto lacto)
        {
            return new QueueMessage
            {
                Data = lacto.Data,
                Creditos = lacto.Tipo == "C" ? lacto.Valor : 0,
                Debitos = lacto.Tipo == "D" ? lacto.Valor : 0,
                Atualizar = true
            };
        }

        private static IQueueMessage GenerateMessageInvertedRecord(ILacto lacto)
        {
            // inverte o lançamento para remover o saldo
            return new QueueMessage
            {
                Data = lacto.Data,
                Creditos = lacto.Tipo == "C" ? 0 : lacto.Valor,
                Debitos = lacto.Tipo == "D" ? 0 : lacto.Valor
            };
        }
    }
}
