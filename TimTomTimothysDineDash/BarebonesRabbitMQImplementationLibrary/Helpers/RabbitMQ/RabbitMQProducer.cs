using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace BarebonesRabbitMQImplementation.Helpers.RabbitMQ
{
    /// <summary>
    /// Sends an object as a message to another endpoint using RabbitMQ as the message broker.
    /// 
    /// References for code snippets: 
    /// - https://code-maze.com/aspnetcore-rabbitmq/
    /// - https://medium.com/trimble-maps-engineering-blog/getting-started-with-net-core-docker-and-rabbitmq-part-3-66305dc50ccf 
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class RabbitMQProducer : IMessageProducer, IDisposable
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly RabbitMQEnv _env;
        private readonly ILogger<RabbitMQProducer> _logger;
        private bool _disposedValue;

        /// <summary>
        /// Creates a RabbitMQProducer and prepares it for creating future connections
        /// </summary>
        public RabbitMQProducer(RabbitMQEnv env, ILogger<RabbitMQProducer> logger)
        {
            _env = env;

            // Declare a factory to provide the connect
            _connectionFactory = new ConnectionFactory { HostName = _env.ContainerName, Port = _env.PortNumber };

            // Provide authentication to connect to RabbitMQ
            _connectionFactory.UserName = _env.UserName;
            _connectionFactory.Password = _env.UserPass;

            // Initiate the long lived connection
            _connection = _connectionFactory.CreateConnection();
            _logger = logger;
        }

        /// <summary>
        /// Sends an object as a message through RabbitMQ to be picked up by the intended recipient.
        /// </summary>
        /// <typeparam name="T">The type of data to send</typeparam>
        /// <param name="targetQueue">The queue target to send data for a subscriber that should be listening for</param>
        /// <param name="message">The message/object to send</param>
        /// <param name="args">RabbitMQ arguments</param>
        public void SendMessage<T>(string targetQueue, T message, IDictionary<string, object>? args = null)
        {
            // Create the channel on the connection to use to send the data
            using (var channel = _connection.CreateModel())
            {
                
                // Declare the queue that will be sent the data. All declarations must be not be exclusive
                if (args != null) channel.QueueDeclare(targetQueue, exclusive: false, arguments: args);
                else channel.QueueDeclare(targetQueue, exclusive: false);

                // Serialize the object as JSON and convert it to an array of bytes that will be accepted by RabbitMQ.
                byte[] serializedData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                // Publish the data using the targetQueue key to RabbitMQ
                channel.BasicPublish(exchange: "", routingKey: targetQueue, body: serializedData);
                _logger.LogDebug($"Sent a message to the queue: {targetQueue}");

                // Do needed cleanup before disposal to prevent a memory leak
                channel.Close();
            }
        }

        /// <summary>
        /// Frees resources from the RabbitMQ connection
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _connection.Close();
                    _connection.Dispose();
                }


                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
