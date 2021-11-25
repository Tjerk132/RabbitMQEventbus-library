using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQEventbus.RabbitMQ.Models
{
    public class RabbitHost
    {
        /// <summary>
        /// The hostname to connect to
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// The username to login with
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password to login with
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Default constructor required for configuration
        /// </summary>
        public RabbitHost() { }
    }
}
