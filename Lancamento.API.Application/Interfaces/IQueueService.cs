using Lancamento.API.Domain.Entities;
using Lancamento.API.Domain.Models;

namespace Lancamento.API.Application.Interfaces
{
    public interface IQueueService
    {
        void PublishMessage(IQueueMessage message);
    }
}
