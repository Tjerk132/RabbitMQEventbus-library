using RabbitMQ.Client;

namespace RabbitMQ.Models
{
    public class RabbitExchange
    {
        /// <summary>
        /// The name of the exchange
        /// </summary>
        public string Name { get; set; }

        private string type;
        /// <summary>
        /// The type of the exchange
        /// </summary>
        public string Type
        {
            get => type;
            set => type = ExchangeType.All().Contains(value) ? value : ExchangeType.Fanout;
        }

        /// <summary>
        /// Default constructor required for configuration
        /// </summary>
        public RabbitExchange() { }
    }
}
