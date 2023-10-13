namespace BarebonesRabbitMQImplementation.Helpers.RabbitMQ
{
    /// <summary>
    /// https://code-maze.com/aspnetcore-rabbitmq/ - Guide used for reference
    /// 
    /// A message producer sends the message to its preconfigured target. This message is supplied by the classes that 
    /// </summary>
    public interface IMessageProducer
    {
        /// <summary>
        /// Sends a message through using the target method to a recipient who is listening for the message.
        /// </summary>
        /// <typeparam name="T">The type of data to be sent</typeparam>
        /// <param name="targetQueue">The queue target to send data for a subscriber that should be listening for</param>
        /// <param name="message">The message object to send across</param>
        /// <param name="args">Implementation specific arguments if supported, this may or may not be supported</param>
        void SendMessage<T>(string targetQueue, T message, IDictionary<string, object>? args = null);
    }
}
