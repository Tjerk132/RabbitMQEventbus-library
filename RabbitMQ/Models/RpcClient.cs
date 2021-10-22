using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Text;

namespace RabbitMQEventbus.RabbitMQ.Models
{
    class RpcClient
    {
        private readonly IModel _channel;
        private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();
        private readonly IBasicProperties props;

        public RpcClient(IModel channel, string queueName)
        {
            _channel = channel;

            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

            props = _channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = queueName;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);

                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(response);
                }
            };

            _channel.BasicConsume(
                consumer: consumer,
                queue: queueName,
                autoAck: true);
        }

        public string Call(object message, string routingKey)
        {
            var messageJson = JsonConvert.SerializeObject(message);

            var messageBytes = Encoding.UTF8.GetBytes(messageJson);

            _channel.BasicPublish(
                exchange: "",
                routingKey: routingKey,
                basicProperties: props,
                body: messageBytes);

            return respQueue.Take();
        }
    }
}
