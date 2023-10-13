using BarebonesRabbitMQImplementationLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarebonesRabbitMQImplementationLibrary.Helpers
{
    /// <summary>
    /// This LoggingHelper class provides some methods useful for logging data that is being moved across services.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class LoggingHelper
    {
        /// <summary>
        /// Used to attach correlation information to a log in a consistent format across services.
        /// This should be used whenever an event is being processed across services.
        /// 
        /// Example message:
        /// "Correlation GUID: {GUID}; Initially Created On: {Timestamp (should be UTC)}; Client UUID: {UUID or 'N/A'}; Log Message: {Message}"
        /// </summary>
        /// <param name="message">The message to be logged</param>
        /// <param name="correlationData">Any object in the event process with the correlation data, a DataWithCorrelation object with the needed information</param>
        /// <returns>The formatted log message</returns>
        public static string FormatMessageWithCorrelationInformation(string message, DataWithCorrelation correlationData)
        {
            return $"Correlation GUID: {correlationData.CorrelationGuid}; \nInitially Created On: {correlationData.InitialCreation}; \n" +
                $"Client UUID: {correlationData.ClientUUID ?? "N/A"}; \nLog Message: {message}";
        }
    }
}
