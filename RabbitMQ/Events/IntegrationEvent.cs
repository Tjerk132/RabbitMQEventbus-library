using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace RabbitMQ.Events
{
    public class IntegrationEvent : IEvent
    {
        /// <summary>
        ///   Default constructor.
        /// </summary>
        protected IntegrationEvent() {}

        /// <summary>
        ///   Constructor that fills the event's Id, Exchange, Routingkey & TimeStamp from its arguments.
        /// </summary>
        /// <param name="args"></param>
        public void SetArgs(BasicDeliverEventArgs args)
        {
            EventId = Guid.NewGuid();
            Exchange = args.Exchange;
            RoutingKey = args.RoutingKey;
            TimeStamp = args.BasicProperties.Timestamp;
        }

        /// <summary>
        /// Event Id.
        /// </summary>
        public Guid EventId { get; private set; }
        /// <summary>
        /// The exchange the message was originally published to.
        /// </summary>
        public string Exchange { get; private set; }
        /// <summary>
        /// The routing key used when the message was originally published.
        /// </summary>
        public string RoutingKey { get; private set; }
        /// <summary>
        /// Message timestamp.
        /// </summary>
        public AmqpTimestamp TimeStamp { get; private set; }
    }

    public interface IEvent
    {
        Guid EventId { get; }
        string Exchange { get; }
        public string RoutingKey { get; }
        AmqpTimestamp TimeStamp { get; }
    }
}
