using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Connection;
using RabbitMQ.EventsBus;
using RabbitMQ.SubscriptionsManager;
using RabbitMQEventbus.RabbitMQ.Models;

namespace RabbitMQ
{
    public static class Extensions
    {
        /// <summary>
        /// Creates a eventbus instance and adds it as singleton to the specified services
        /// </summary>
        /// <param name="services">The services to configure the eventbus with</param>
        /// <returns>The services with added RabbitMQ configuration</returns>
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            var options = sp.GetService<IOptions<RabbitMQConfiguration>>();

            RabbitMQConfiguration configuration = options.Value;

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                RabbitHost host = configuration.Host;

                IConnectionFactory connectionFactory = new ConnectionFactory() { 
                    HostName = host.Hostname, 
                    UserName = host.Username, 
                    Password = host.Password 
                };

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
                    configuration
                );
            });
            return services;
        }
    }
}
