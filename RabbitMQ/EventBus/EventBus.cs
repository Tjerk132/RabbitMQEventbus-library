using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Connection;
using RabbitMQ.Events;
using RabbitMQ.Models;
using RabbitMQ.SubscriptionsManager;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace RabbitMQ.EventBus
{
    public class EventBus : IEventBus, IDisposable
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private IModel _consumerChannel;
        private readonly ILogger<EventBus> _logger;
        private readonly int _retryCount;

        private readonly RabbitExchange _exchange;
        private readonly List<RabbitQueue> _queues;

        public EventBus(
            IRabbitMQPersistentConnection persistentConnection,
            IEventBusSubscriptionsManager subsManager,
            ILogger<EventBus> logger,
            RabbitExchange exchange,
            List<RabbitQueue> queues,
            int retryCount = 5
        )
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _subsManager = subsManager;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _exchange = exchange ?? new RabbitExchange();
            _queues = queues ?? new List<RabbitQueue>();
            _retryCount = retryCount;

            _consumerChannel = CreateConsumerChannel();
        }

        public void Subscribe<E, EH>(string queueName, List<string> routingKeys, object[] services)
            where E : IntegrationEvent
            where EH : IIntegrationEventHandler<E>
        {
            RabbitQueue queue = _queues.Find(x => x.Name.Equals(queueName));
            queue.AddRoutingKeys(routingKeys);
            _subsManager.AddSubscription<E, EH>(_consumerChannel, args: services, _exchange, queue);
        }

        public void Unsubscribe(string queueName, List<string> routingKeys)
        {
            RabbitQueue queue = _queues.Find(x => x.Name.Equals(queueName));
            queue.RemoveRoutingKeys(routingKeys);
            _subsManager.RemoveSubscription(_consumerChannel, _exchange, queue);
        }

        public void Publish(string routingKey, IntegrationEvent @event)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.EventId, $"{time.TotalSeconds:n1}", ex.Message);
                });

            //TODO: check value
            var eventName = @event.GetType().Name;

            _logger.LogTrace("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", @event.EventId, eventName);

            using var channel = _persistentConnection.CreateModel();

            _logger.LogTrace("Declaring RabbitMQ exchange to publish event: {EventId}", @event.EventId);

            channel.ExchangeDeclare(exchange: _exchange.Name, type: _exchange.Type);
            /////

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            policy.Execute(() =>
            {
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent

                _logger.LogTrace("Publishing event to RabbitMQ: {EventId}", @event.EventId);

                _consumerChannel.BasicPublish(
                            exchange: _exchange.Name,
                            routingKey: routingKey,
                            basicProperties: properties,
                            body: body);
            });
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            _logger.LogTrace("Creating RabbitMQ consumer channel");

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: _exchange.Name, type: _exchange.Type);

            foreach (RabbitQueue queue in _queues)
            {
                channel.QueueDeclare(queue: queue.Name,
                                     durable: queue.Durable,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
            }

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
            };

            return channel;
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }
            _queues.Clear();
        }
    }
}
