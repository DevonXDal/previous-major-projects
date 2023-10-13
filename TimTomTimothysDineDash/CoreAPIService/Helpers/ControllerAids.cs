using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Helpers.RabbitMQ;
using Serilog;

namespace CoreAPIService.Helpers
{
    /// <summary>
    /// This ControllerAids class provides methods to reduce code duplication from the controllers.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class ControllerAids
    {
        public static string? SearchRequestForClientUUID(IHeaderDictionary headers, string requestCorrelationGuid)
        {
            foreach (var item in headers)
            {
                if (item.Key == "ClientUUID")
                {
                    return item.Value.First();
                }
            }

            Log.Information($"No client UUID was found for request {requestCorrelationGuid}");
            return null;
        }

        /// <summary>
        /// Makes a few attempts to pull a response item of some kind from the expected response queue.
        /// It makes an attempt every second until the attempts are used up.
        /// If data is returned, it is returned to the controller.
        /// Otherwise, null is returned, indicating no response.
        /// 
        /// This will always make at least one attempt.
        /// </summary>
        /// <typeparam name="D">The data that should be expected from the response</typeparam>
        /// <param name="subscriber">The message subscriber to use</param>
        /// <param name="correlationGuid">The GUID that establishes a correlation and identifies the event chain</param>
        /// <param name="originQueueName">Name of the original queue</param>
        /// <param name="attempts">Number of attempts to make at fetching the response before giving up</param>
        /// <returns>The expected response or null if none is found</returns>
        public static async Task<D?> TaskListenForEventResponse<D>(IMessageSubscriber<string> subscriber, string correlationGuid, string originQueueName, int attempts = 5)
        {
            do
            {
                attempts--;
                await Task.Delay(750); // Wait 3/4 of a second before attempting retrieval

                D? possibleResponse = subscriber.FetchAndAcknowledgeNextIfExists<D>($"{originQueueName}--{correlationGuid}", args: RabbitMQOptionPresets.SHOULD_EXPIRE_AFTER_2_MINUTES);

                if (possibleResponse != null) return possibleResponse;
            } while (attempts > 0);

            return default;
        }
    }
}
