using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Helpers;
using BarebonesRabbitMQImplementationLibrary.Models;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenStaffService.EventHandlers
{
    /// <summary>
    /// This EventHandlerBase class serves to make it easier for carrying out basic return messages and logging events.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public abstract class EventHandlerBase
    {
        protected IMessageProducer _messageProducer;

        /// <summary>
        /// Sends a basic response and provides a detailed log about such.
        /// 
        /// Sends it to a return address, specific to that event.
        /// </summary>
        /// <param name="eventName">The name of the event being handled by the handler</param>
        /// <param name="logMessage">The message to log</param>
        /// <param name="responseCode">0 for a normal response, use negative numbers for indicating different failures that occured, positive for atypical good responses</param>
        /// <param name="dto">The DTO data sent to handle the event</param>
        /// <param name="responseMessage">Message to send back to the other service (end-user friendly) - Should specify if the response code is less than 0</param>
        public void SendBasicResponse(string eventName, string logMessage, int responseCode, DataWithCorrelation dto, string? responseMessage = null)
        {
            string formattedLogMessage = LoggingHelper.FormatMessageWithCorrelationInformation(logMessage, dto);
            if (responseCode < 0)
            {
                Log.Warning(formattedLogMessage);
            } else
            {
                Log.Information(formattedLogMessage);
            }
            

            _messageProducer.SendMessage($"{eventName}--{dto.CorrelationGuid}",
                new EventResponse()
                {
                    ClientUUID = dto.ClientUUID,
                    CorrelationGuid = dto.CorrelationGuid,
                    InitialCreation = dto.InitialCreation,
                    ResponseCode = responseCode,
                    Message = responseMessage
                },
                args: RabbitMQOptionPresets.SHOULD_EXPIRE_AFTER_2_MINUTES
            );
        }
    }
}
