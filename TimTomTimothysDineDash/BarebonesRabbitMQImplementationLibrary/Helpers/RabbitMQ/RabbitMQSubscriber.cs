using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BarebonesRabbitMQImplementation.Helpers.RabbitMQ
{
    /// <summary>
    /// This RabbitMQSubscriber class provides a means for other classes to listen for data coming across various RabbitMQ queues.
    /// It allows action to be taken in response for new data that has come into the queue.
    /// 
    /// References for code snippets: 
    /// - https://www.rabbitmq.com/dotnet-api-guide.html
    /// - https://code-maze.com/aspnetcore-rabbitmq/
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class RabbitMQSubscriber : IMessageSubscriber<string>, IDisposable
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly RabbitMQEnv _env;
        private readonly ILogger<RabbitMQSubscriber> _logger;
        private readonly IConnection _connection;
        private bool _disposedValue;
        private JsonSerializerSettings _serializerSettings;

        /// <summary>
        /// Creates a RabbitMQSubscriber and prepares it for creating future connections
        /// </summary>
        public RabbitMQSubscriber(RabbitMQEnv env, ILogger<RabbitMQSubscriber> logger)
        {
            _env = env;
            _serializerSettings = new JsonSerializerSettings();
            _serializerSettings.MissingMemberHandling = MissingMemberHandling.Error; // Should prevent non-matches from being accepted

            // Declare a factory to provide the connect
            _connectionFactory = new ConnectionFactory { HostName = _env.ContainerName, Port = _env.PortNumber, UseBackgroundThreadsForIO = true };

            // Provide authentication to connect to RabbitMQ
            _connectionFactory.UserName = _env.UserName;
            _connectionFactory.Password = _env.UserPass;

            // Initiate the long lived connection
            _connection = _connectionFactory.CreateConnection();
            _logger = logger;
        }

        /// <summary>
        /// Attempts to grab the next item of data from RabbitMQ using the key specified for targeting a queue.
        /// Returns null if no data is found on the queue.
        /// Always acknowledges
        /// 
        /// Returns null on failed type conversions.
        /// </summary>
        /// <typeparam name="D">The expected type of data to undergo type conversion</typeparam>
        /// <param name="queueKey">The key needed to find the RabbitMQ queue</param>
        /// <param name="args">RabbitMQ arguments</param>
        /// <returns>The next data item if the queue has another item and type conversion is successful</returns>
        public D? FetchAndAcknowledgeNextIfExists<D>(string queueKey, IDictionary<string, object>? args = null)
        {
            // Create the channel on the connection to use to send the data
            using (var channel = _connection.CreateModel())
            {
                D? nextItem = default;

                // Declare the queue that will be sent the data
                if (args != null) channel.QueueDeclare(queueKey, exclusive: false, arguments: args);
                else channel.QueueDeclare(queueKey, exclusive: false);

                var data = channel.BasicGet(queueKey, true);
                if (data == null) // Log an empty queue
                {
                    _logger.LogDebug($"Attempted to retrieve next item for the queue: {queueKey}, but it was null");
                    return nextItem;
                } 

                try
                {

                    // Attempt to get the bytes for the body/message that was recieved
                    byte[] bodyBytes = data.Body.ToArray();

                    nextItem = AttemptDeserialization<D>(bodyBytes);

                    _logger.LogDebug($"Attempted to retrieve next item for the queue: {queueKey}, and was successful");
                }
                catch (Exception e)
                {
                    _logger.LogError($"Attempted to retrieve next item for the queue: {queueKey}, but it was not deserialized correctly", e);
                }

                return nextItem;
            }
        }

        /// <summary>
        /// Subscribes/observes the information coming in from a message broker at a target source specified by the target data.
        /// Acts as an observable and requests onNext which is supplied a value whenever one is successfully recieved and converted.
        /// Always acknowledges
        /// 
        /// onError handles what happens when a value is recieved but is not
        /// </summary>
        /// <typeparam name="D">The expected type of data to undergo type conversion</typeparam>
        /// <param name="queueKey">The key needed to target the specific queue from RabbitMQ</param>
        /// <param name="onNext">The operation to perform when data is successfully recieved and converted</param>
        /// <param name="onError">The operation to perform when data is recieved but an issue arises</param>
        /// <param name="args">RabbitMQ arguments</param>
        public async Task Subscribe<D>(string queueKey, Action<D> onNext, Action<Exception> onError, IDictionary<string, object>? args = null)
        {
            var channel = _connection.CreateModel();

            // Declare the queue that will be sent the data
            if (args != null) channel.QueueDeclare(queueKey, exclusive: false, arguments: args);
            else channel.QueueDeclare(queueKey, exclusive: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                D? nextItem = default;

                try
                {
                    // Attempt to get the bytes for the body/message that was recieved
                    byte[] bodyBytes = eventArgs.Body.ToArray();

                    nextItem = AttemptDeserialization<D>(bodyBytes);
                    if (nextItem == null) throw new InvalidCastException("Did not deserialize correctly");

                    _logger.LogDebug($"An observer listening for the queue: {queueKey}, successfully deserialized the received data");
                    onNext.Invoke(nextItem);
                }
                catch (Exception e)
                {
                    _logger.LogError($"An observer listening for the queue: {queueKey}, failed to deserialize the received data", e);
                    onError.Invoke(e);
                }
            };
            
            // Register the consumer to consume for this queue only, auto acknowledge, and non-exclusively
            channel.BasicConsume(queue: queueKey, autoAck: true, consumer: consumer, exclusive: false);
        }

        // Attempts to deserialize the data and throws an exception if the deserialization fails
        private D? AttemptDeserialization<D>(byte[] dataBytes)
        {
            
            // Convert the byte array to the json string of the message
            string jsonString = Encoding.UTF8.GetString(dataBytes);

            // Deserialize from Json, error for bad objects
            return JsonConvert.DeserializeObject<D>(jsonString, _serializerSettings);
        }

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
