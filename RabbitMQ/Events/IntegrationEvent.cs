using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace RabbitMQ.Events
{
    public class IntegrationEvent : IEvent
    {
        /// <summary>
        ///   Default constructor that creates the event's id.
        /// </summary>
        protected IntegrationEvent() {
            EventId = Guid.NewGuid();
        }

        /// <summary>
        /// Sets basic properties from the given eventArgs argument.
        /// </summary>
        /// <param name="args"></param>
        public void SetArgs(BasicDeliverEventArgs args) {  
            BasicProperties = args.BasicProperties;
        }

        /// <summary>
        /// Event Id.
        /// </summary>
        public Guid EventId { get; private set; }

        /// <summary>
        /// Common AMQP Basic headers, spanning the union of the
        /// functionality offered by versions 0-8, 0-8qpid, 0-9 and 0-9-1 of AMQP.
        /// </summary>
        public IBasicProperties BasicProperties { get; private set; }
    }

    public interface IEvent
    {
        /// <summary>
        /// The id of the event.
        /// The eventId is automatically generated on instantiation
        /// </summary>
        Guid EventId { get; }
        /// <summary>
        /// Common AMQP Basic content-class headers interface, spanning the union of the
        /// functionality offered by versions 0-8, 0-8qpid, 0-9 and 0-9-1 of AMQP.
        /// </summary>
        IBasicProperties BasicProperties { get; }
    }
}
