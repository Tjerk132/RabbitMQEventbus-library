using RabbitMQ.Client;
using RabbitMQ.Events;
using RabbitMQ.Models;

namespace RabbitMQ.SubscriptionsManager
{
    public interface IEventBusSubscriptionsManager
    {
        /// <summary>
        /// Binds the given queue to the event bus' exchange
        /// </summary>
        /// <typeparam name="E">The type of event the handler will process</typeparam>
        /// <typeparam name="EH">The type of eventHandler that will process the event</typeparam>
        /// <param name="channel"></param>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        void AddSubscription<E, EH>(IModel channel, RabbitExchange exchange, RabbitQueue queue)
           where E : IntegrationEvent
           where EH : IIntegrationEventHandler<E>;

        /// <summary>
        /// Unbinds the given queue from the event bus' exchange
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        void RemoveSubscription(IModel channel, RabbitExchange exchange, RabbitQueue queue);
    }
}
