using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Events;
using RabbitMQ.Models;
using System;
using System.Text;
using RabbitMQEventbus.RabbitMQ.Models;

namespace RabbitMQ.SubscriptionsManager
{
    public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        private readonly ILogger<InMemoryEventBusSubscriptionsManager> _logger;

        public InMemoryEventBusSubscriptionsManager(ILogger<InMemoryEventBusSubscriptionsManager> logger)
        {
            _logger = logger;
        }

        public void AddSubscription<E, EH>(
            IModel channel,
            object[] args,
            RabbitExchange exchange, 
            RabbitQueue queue
        )
            where E : IntegrationEvent
            where EH : IIntegrationEventHandler<E>
        {
            channel.ExchangeDeclare(exchange.Name, exchange.Type);

            // - a message goes to the queues whose binding key exactly matches the routing key of the message.
            foreach (string routingKey in queue.RoutingKeys)
            {
                channel.QueueBind(queue.Name, exchange.Name, routingKey);
            }

            _logger.LogTrace(" [x] Binded on exchange {0} queue {1} with {2}", exchange, queue, string.Join(", ", queue.RoutingKeys.ToArray()));

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (object sender, BasicDeliverEventArgs eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                E integrationEvent = (E)JsonConvert.DeserializeObject(message, typeof(E));
                integrationEvent.SetArgs(eventArgs);

                EH eventHandler = (EH)Activator.CreateInstance(typeof(EH), args);

                eventHandler.Handle(integrationEvent);
            };
            channel.BasicConsume(queue: queue.Name, autoAck: true, consumer: consumer);
        }

        public void RemoveSubscription(
            IModel channel,
            RabbitExchange exchange,
            RabbitQueue queue
        )
        {
            channel.ExchangeDeclare(exchange.Name, exchange.Type);

            foreach (string routingKey in queue.RoutingKeys)
            {
                channel.QueueUnbind(queue.Name, exchange.Name, routingKey);
            }
        }

        public void CreateRpcServer<E, EH>(IModel channel, RabbitQueue queue)
            where E : IntegrationEvent
            where EH : IRpcIntegrationEventHandler<E>
        {
            _ = new RpcServer<E, EH>(channel, queue);
        }

        public string CallRpcServer(IModel channel, object message, string routingKey)
        {
            var rpcClient = new RpcClient(channel, queueName: routingKey);
            return rpcClient.Call(message, routingKey);
        }
    }
}
