using Microsoft.Extensions.DependencyInjection;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Events;
using RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.EventsBus
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
        /// <param name="queueName">The name of the queue to subscribe to</param>
        public void Subscribe<E, EH>(string queueName, List<string> routingKeys, object[] services = null)
            where E : IntegrationEvent
            where EH : IIntegrationEventHandler<E>;

        /// <summary>
        /// Unsubscribe from the event bus' queue with the given routingKeys
        /// </summary>
        /// <param name="routingKeys">The routing keys to unsubscribe with</param>
        /// <param name="queueName">The name of the queue to unsubscribe from</param>
        public void Unsubscribe(string queueName, List<string> routingKeys);

        /// <summary>
        /// Creates a rpcServer on the given queue
        /// </summary>
        /// <param name="queue">The queue on which the rpc server will run</param>
        /// <param name="services">The services to inject into the eventHandler</param>
        public void CreateRpcServer<E, EH>(string queue, object[] services = null)
              where E : IntegrationEvent
              where EH : IRpcIntegrationEventHandler<E>;

        /// <summary>
        /// Calls the rpcServer if created and sends a message with the given routingKey
        /// </summary>
        /// <param name="message">The message to publish</param>
        /// <param name="routingKey">The queue the message will be send to</param>
        /// <returns>The message received from the RPC server</returns>
        public string CallRpcServer(object message, string routingKey);
    }
}
