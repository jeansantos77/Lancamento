using AutoMapper;
using Lancamento.API.Application.Implementations;
using Lancamento.API.Application.Interfaces;
using Lancamento.API.Domain.Entities;
using Lancamento.API.Domain.Interfaces;
using Lancamento.API.Domain.Models;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Lancamento.API.Tests
{
    public class LancamentoServiceTests
    {
        private readonly LactoService _lactoService;

        public LancamentoServiceTests()
        {
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<LactoAddModel, Lacto>(It.IsAny<LactoAddModel>())).Returns(GetLancamento());
            mockMapper.Setup(m => m.Map<Lacto, LactoModel>(It.IsAny<Lacto>())).Returns(GetLactoModel());
            mockMapper.Setup(m => m.Map<LactoModel, Lacto>(It.IsAny<LactoModel>())).Returns(GetLancamento());

            var mockLactoRepository = new Mock<ILactoRepository>();
            //mockLactoRepository.Setup(s=> s.GetById(It.IsAny<int>())).ReturnsAsync(GetLancamento());

            var mockQueueService = new Mock<IQueueService>();
            mockQueueService.Setup(s => s.PublishMessage(It.IsAny<IQueueMessage>())).Callback(() => Console.WriteLine("Publish Message"));

            _lactoService = new LactoService(mockMapper.Object, mockLactoRepository.Object, mockQueueService.Object);
        }

        [Fact]
        public void LactoService_Should_Not_Return_Exception_If_Try_Add()
        {
            //Arrange
            LactoAddModel model = GetLactoAddModel();

            //Assert
            var exception = Record.ExceptionAsync(() => _lactoService.Add(model));
            Assert.Null(exception.Result);
        }

        [Fact]
        public async Task LactoService_Should_Return_Exception_If_Try_Delete_Id_Not_Found()
        {
            //Arrange
            LactoModel model = new()
            {
                Id = 1
            };

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _lactoService.Delete(model.Id));
            Assert.Equal($"Lancamento com [ Id = {model.Id}] não encontrado.", exception.Message);
        }

        [Fact]
        public async Task LactoService_Should_Return_Exception_If_Entidade_Is_Invalid()
        {
            //Arrange
            LactoAddModel model = new();

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _lactoService.Add(model));
            Assert.Equal("Descrição deve ser informada!" + Environment.NewLine + "Valor deve ser informado!", exception.Message);
        }

        [Fact]
        public async Task LactoService_Should_Return_Exception_If_Try_Update_Id_NotFound()
        {
            //Arrange
            LactoModel model = GetLactoModel();

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _lactoService.Update(model));
            Assert.Equal($"Lancamento com [ Id = {model.Id}] não encontrado.", exception.Message);
        }


        private LactoAddModel GetLactoAddModel()
        {
            LactoAddModel model = new LactoAddModel
            {
                Data = DateTime.Today,
                Descricao = "Lançamento Teste Model",
                Tipo = "C",
                Valor = 100,
                UsuarioId = 1
            };

            return model;
        }

        private LactoModel GetLactoModel()
        {
            LactoModel model = new LactoModel
            {
                Id = 1,
                Data = DateTime.Today,
                Descricao = "Lançamento Teste Model",
                Tipo = "C",
                Valor = 100,
                UsuarioId = 1
            };

            return model;
        }

        private Lacto GetLancamento()
        {
            Lacto lacto = new()
            {
                Id = 1,
                Data = DateTime.Today,
                Descricao = "Lançamento Teste",
                Tipo = "C",
                Valor = 100,
                UsuarioId = 1
            };

            return lacto;

        }

        private QueueMessage GetQueueMessage()
        {
            return new QueueMessage
            {
                Data = DateTime.Today,
                Creditos = 100,
                Debitos = 0
            };
        }
    }
}
