using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Connection;
using RabbitMQ.EventBus;
using RabbitMQ.Models;
using RabbitMQ.SubscriptionsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQEventbus.RabbitMQ
{
    public static class Extensions
    {
        /// <summary>
        /// Creates a eventbus instance and adds it as singleton to the specified services
        /// </summary>
        /// <param name="services">The services to configure the eventbus with</param>
        /// <param name="connectionFactory">The connectionFactory to use for establishing a connection to RabbitMQ</param>
        /// <param name="exchange">A exchange to create on the event bus</param>
        /// <param name="queue">A queue to create on the eventbus</param>
        /// <returns>The services with added RabbitMQ configuration</returns>
        public static IServiceCollection AddRabbitMQ(
            this IServiceCollection services, 
            IConnectionFactory connectionFactory, 
            RabbitExchange exchange, 
            RabbitQueue queue
        )
        {
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                return new DefaultRabbitMQPersistentConnection(connectionFactory, logger);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            services.AddSingleton<IEventBus, EventBus>(sp =>
            {
                var serviceBusPersisterConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBus>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBus(
                    serviceBusPersisterConnection,
                    eventBusSubcriptionsManager,
                    logger,
                    exchange, 
                    queue
                );
            });

            return services;
        }
    }
}
