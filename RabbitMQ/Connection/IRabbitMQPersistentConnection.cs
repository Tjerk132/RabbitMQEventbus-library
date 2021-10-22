using RabbitMQ.Client;
using System;

namespace RabbitMQ.Connection
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        /// <summary>
        /// Determines if the RabbitMQ client is connected to the specified host
        /// </summary>
        /// <returns>true if the connection is established</returns>
        bool IsConnected { get; }

        /// <summary>
        /// Creates a model created by the provided connectionFactory
        /// </summary>
        /// <returns>The model that is created with the connection</returns>
        IModel CreateModel();

        /// <summary>
        /// Attempts to create a connection with the provided connectionFactory
        /// </summary>
        /// <returns>true if the connection was successful</returns>
        bool TryConnect();
    }
}
