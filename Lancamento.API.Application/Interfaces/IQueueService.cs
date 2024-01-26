using Lancamento.API.Domain.Interfaces;

namespace Lancamento.API.Application.Interfaces
{
    public interface IQueueService
    {
        void PublishMessage(IQueueMessage message);
    }
}
