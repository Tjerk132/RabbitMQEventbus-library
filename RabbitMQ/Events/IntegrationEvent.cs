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
            BasicProperties = args.BasicProperties;
        }

        /// <summary>
        /// Event Id.
        /// </summary>
        public Guid EventId { get; private set; }

        /// <summary>
        /// Application correlation identifier
        /// </summary>
        public IBasicProperties BasicProperties { get; set; }
    }

    public interface IEvent
    {
        Guid EventId { get; }
        IBasicProperties BasicProperties { get; }
    }
}
