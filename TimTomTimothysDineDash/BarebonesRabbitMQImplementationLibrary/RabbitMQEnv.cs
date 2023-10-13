using Microsoft.Extensions.Configuration;

namespace BarebonesRabbitMQImplementation
{
    /// <summary>
    /// This RabbitMQEnv class holds the strongly typed names for configuration values related to RabbitMQ.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class RabbitMQEnv
    {
        private readonly IConfiguration _config;

        public RabbitMQEnv(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// The name of the container used for RabbitMQ.
        /// </summary>
        public string ContainerName => _config["RABBITMQ_CONTAINER_NAME"];

        /// <summary>
        /// The RabbitMQ user name to use.
        /// </summary>
        public string UserName => _config["RABBITMQ_USER_NAME"];

        /// <summary>
        /// The RabbitMQ password for the specified user name.
        /// </summary>
        public string UserPass => _config["RABBITMQ_USER_PASS"];

        /// <summary>
        /// The port number that RabbitMQ is listening on
        /// </summary>
        public int PortNumber => int.Parse(_config["RABBITMQ_PORT"]);
    }
}
