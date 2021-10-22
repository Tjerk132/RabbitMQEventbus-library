namespace RabbitMQ.Events
{
    public interface IRpcIntegrationEventHandler<TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// The method will be invoked to process the message received from the eventbus
        /// </summary>
        /// <param name="event"></param>
        /// <returns>The return message</returns>
        object Process(TIntegrationEvent @event);
    }
}
