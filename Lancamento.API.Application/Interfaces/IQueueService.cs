using Lancamento.API.Domain.Models;
using System.Threading.Tasks;

namespace Lancamento.API.Application.Interfaces
{
    public interface IQueueService
    {
        void PublishMessage(MessageQueue msgQueue);
    }
}
