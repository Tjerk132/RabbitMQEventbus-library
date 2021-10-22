﻿using RabbitMQ.Client;
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
        /// <param name="channel">The channel that will be used to declare exchanges and bind with the given queue</param>
        /// <param name="args">The services that will be injected into the eventHandler</param>
        /// <param name="exchange">The exchange on which the subscription addition should occur</param>
        /// <param name="queue">The queue with routingKeys to bind</param>
        void AddSubscription<E, EH>(IModel channel, object[] args, RabbitExchange exchange, RabbitQueue queue)
           where E : IntegrationEvent
           where EH : IIntegrationEventHandler<E>;

        /// <summary>
        /// Unbinds the given queue from the event bus' exchange
        /// </summary>
        /// <param name="channel">THe channel that will be used to declare exchanges and unbind with the given queue</param>
        /// <param name="exchange">The exchange on which the subscription removal should occur</param>
        /// <param name="queue">The queue with routingKeys to unbind</param>
        void RemoveSubscription(IModel channel, RabbitExchange exchange, RabbitQueue queue);

        /// <summary>
        /// Creates a rpcServer on the given queue with the given channel
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="queue"></param>
        public void CreateRpcServer<E, EH>(IModel channel, RabbitQueue queue)
              where E : IntegrationEvent
              where EH : IRpcIntegrationEventHandler<E>;

        /// <summary>
        /// Calls the rpcServer if created and sends a message with the given routingKey
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="queue"></param>
        /// <param name="exchange"></param>
        /// <param name="message"></param>
        /// <param name="routingKey"></param>
        /// <returns>The message received from the RPC server</returns>
        public string CallRpcServer(IModel channel, object message, string routingKey);
    }
}
