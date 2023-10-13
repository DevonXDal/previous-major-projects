namespace BarebonesRabbitMQImplementation.Helpers.RabbitMQ
{
    /// <summary>
    /// A message subscriber is configured to listen and react to specific events sent out by a message broker at source/queue.
    /// A message subscriber can be given instructions on how to handle incoming requests.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    /// <typeparam name="T">Defined by the implementation, the information needed to grab specific information from a message broker</typeparam>
    public interface IMessageSubscriber<T>
    {
        /// <summary>
        /// Subscribes/observes the information coming in from a message broker at a target source specified by the target data.
        /// Acts as an observable and requests onNext which is supplied a value whenever one is successfully recieved and converted.
        /// 
        /// Observes an error if type conversion fails.
        /// </summary>
        /// <typeparam name="D">The expected type of data to undergo type conversion</typeparam>
        /// <param name="targetData">The information/key needed to target the specific source of information from a message broker</param>
        /// <returns>An observable that recieves information from a message broker</returns>
        /// <param name="onNext">The operation to perform when data is successfully recieved and converted</param>
        /// <param name="onError">The operation to perform when data is recieved but an issue arises</param>
        /// <param name="args">Implementation specific arguments if supported, this may or may not be supported</param>
        public Task Subscribe<D>(T targetData, Action<D> onNext, Action<Exception> onError, IDictionary<string, object>? args = null);


        /// <summary>
        /// Attempts to grab the next item of data from a message broker using the data specified for targeting a source/queue.
        /// Returns null if no data is found on the queue.
        /// 
        /// Returns null on failed type conversions.
        /// </summary>
        /// <typeparam name="D">The expected type of data to undergo type conversion</typeparam>
        /// <param name="targetData">The information/key needed to target the specific source of information from a message broker</param>
        /// <param name="args">Implementation specific arguments if supported, this may or may not be supported</param>
        /// <returns>The next data if the source/queue has another item and type conversion is successful</returns>
        public D? FetchAndAcknowledgeNextIfExists<D>(T targetData, IDictionary<string, object>? args = null);
    }
}
