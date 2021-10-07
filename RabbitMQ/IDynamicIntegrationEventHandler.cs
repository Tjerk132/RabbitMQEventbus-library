using Samarasoft.Common.Events;
using System.Threading.Tasks;

namespace RabbitMQ
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(IntegrationEvent eventData);
    }
}
