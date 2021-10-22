using RabbitMQ.Models;
using System.Collections.Generic;

namespace RabbitMQ
{
    public class RabbitMQConfiguration
    {
        /// <summary>
        /// The host that will be used to configure the connection with RabbitMQ
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// The exchange that should be declared to the eventbus
        /// </summary>
        public RabbitExchange Exchange { get; set; }

        /// <summary>
        /// The queues that should be binded 
        /// </summary>
        public List<RabbitQueue> Queues { get; set; }
    }
}
