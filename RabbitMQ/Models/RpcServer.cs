using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Events;
using RabbitMQ.Models;
using System;
using System.Text;

namespace RabbitMQEventbus.RabbitMQ.Models
{
    class RpcServer<E, EH>
        where E : IntegrationEvent
        where EH : IRpcIntegrationEventHandler<E>
    {
        public RpcServer(IModel channel, RabbitQueue queue, object[] args)
        {
            channel.QueueDeclare(queue: queue.Name, durable: queue.Durable,
              exclusive: queue.Exclusive, autoDelete: queue.AutoDelete, arguments: null);

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: queue.Name,
              autoAck: false, consumer: consumer);

            consumer.Received += (sender, eventArgs) =>
            {
                //receive
                var body = eventArgs.Body.ToArray();
                var props = eventArgs.BasicProperties;
                var message = Encoding.UTF8.GetString(body);

                //process
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                E integrationEvent = (E)JsonConvert.DeserializeObject(message, typeof(E));
                integrationEvent.SetArgs(eventArgs);

                EH eventHandler = (EH)Activator.CreateInstance(typeof(EH), args);

                object response = eventHandler.Process(integrationEvent);

                //send
                var responseJson = JsonConvert.SerializeObject(response);

                var responseBytes = Encoding.UTF8.GetBytes(responseJson);

                channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                    basicProperties: replyProps, body: responseBytes);

                channel.BasicAck(deliveryTag: eventArgs.DeliveryTag,
                    multiple: false);

            };
        }
    }
}
