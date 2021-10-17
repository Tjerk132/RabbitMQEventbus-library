using Microsoft.Extensions.DependencyInjection;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.EventBus
{
    public interface IEventBus
    {
        /// <summary>
        /// Publish a message to the eventBus with the given routingKey and event
        /// </summary>
        /// <param name="routingKey"></param>
        /// <param name="event"></param>
        public void Publish(string routingKey, IntegrationEvent @event);

        /// <summary>
        /// Subscribe to the event bus' queue with the given routingKeys
        /// </summary>
        /// <typeparam name="E">The type of event the handler will process</typeparam>
        /// <typeparam name="EH">The type of eventHandler that will process the event</typeparam>
        /// <param name="routingKeys"></param>
        public void Subscribe<E, EH>(List<string> routingKeys)
            where E : IntegrationEvent
            where EH : IIntegrationEventHandler<E>;

        /// <summary>
        /// Unsubscribe from the event bus' queue with the given routingKeys
        /// </summary>
        /// <param name="routingKeys"></param>
        public void Unsubscribe(List<string> routingKeys);
    }
}
