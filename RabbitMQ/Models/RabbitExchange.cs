using RabbitMQ.Client;

namespace RabbitMQ.Models
{
    public class RabbitExchange
    {
        /// <summary>
        /// The name of the exchange
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The type of the exchange
        /// </summary>
        public string Type { get; set; }

        public RabbitExchange(string name = null, string type = ExchangeType.Fanout)
        {
            Name = name;
            Type = ExchangeType.All().Contains(type) ? type : ExchangeType.Fanout;
        }

        /// <summary>
        /// Default constructor required for configuration
        /// </summary>
        public RabbitExchange(){ }
    }
}
