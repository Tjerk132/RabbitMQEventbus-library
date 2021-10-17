namespace RabbitMQ.Events
{
    public interface IIntegrationEventHandler<TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// The method will be invoked to handle the message received from the eventbus
        /// </summary>
        /// <param name="event">The event to process</param>
        void Handle(TIntegrationEvent @event);
    }
}