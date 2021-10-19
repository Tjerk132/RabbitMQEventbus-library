using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Models
{
    public class RabbitQueue
    {
        /// <summary>
        /// The name of the queue
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Should this queue survive a broker restart?
        /// </summary>
        /// 
        public bool Durable { get; set; }
        /// <summary>
        /// The routingKeys the queue will be binded with
        /// </summary>
        public List<string> RoutingKeys { get; set; }

        public RabbitQueue(string name = null, bool durable = false, List<string> routingKeys = null)
        {
            Name = name;
            Durable = durable;
            RoutingKeys = routingKeys ?? new List<string>();
        }

        /// <summary>
        /// Default constructor required for configuration
        /// </summary>
        public RabbitQueue() { }

        public void AddRoutingKey(string routingKey) => RoutingKeys.Add(routingKey);
        
        public void RemoveRoutingKey(string routingKey) => RoutingKeys.Remove(routingKey);

        public void AddRoutingKeys(List<string> routingKeys) => RoutingKeys.AddRange(routingKeys);

        public void RemoveRoutingKeys(List<string> routingKeys) => RoutingKeys = RoutingKeys.Except(routingKeys).ToList();

        public void Clear() => RoutingKeys.Clear();
    }
}
