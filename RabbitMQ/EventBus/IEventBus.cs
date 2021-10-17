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
        /// <param name="routingKey">The routing key to publish the event with</param>
        /// <param name="event">The event to publish</param>
        public void Publish(string routingKey, IntegrationEvent @event);

        /// <summary>
        /// Subscribe to the event bus' queue with the given routingKeys
        /// </summary>
        /// <typeparam name="E">The type of event the handler will process</typeparam>
        /// <typeparam name="EH">The type of eventHandler that will process the event</typeparam>
        /// <param name="routingKeys">The routing keys to subscribe with</param>
        /// <param name="services">The services to inject into the eventHandler</param>
        public void Subscribe<E, EH>(List<string> routingKeys, object[] services)
            where E : IntegrationEvent
            where EH : IIntegrationEventHandler<E>;

        /// <summary>
        /// Unsubscribe from the event bus' queue with the given routingKeys
        /// </summary>
        /// <param name="routingKeys">The routing keys to unsubscribe with</param>
        public void Unsubscribe(List<string> routingKeys);
    }
}
