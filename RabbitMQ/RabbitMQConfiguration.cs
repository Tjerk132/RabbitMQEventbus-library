using Newtonsoft.Json;
using RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQEventbus
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
