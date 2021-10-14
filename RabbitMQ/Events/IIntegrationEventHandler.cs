namespace RabbitMQ.Events
{
    public interface IIntegrationEventHandler<TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
    {
        void Handle(TIntegrationEvent @event);
    }
}