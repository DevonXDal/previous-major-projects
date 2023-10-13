using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Helpers;
using BarebonesRabbitMQImplementationLibrary.Models;
using BarebonesRabbitMQImplementationLibrary.Models.DTO;
using CoreAPIService.Helpers;
using CoreAPIService.Models.DTO;
using CoreAPIService.Models.DTO.StatusUpdate.HostStaff;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CoreAPIService.Controllers.v1
{
    [Route("v{version:apiVersion}/[controller]/[action]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class KitchenStaffController : ControllerBase
    {
        private readonly IMessageProducer _messageProducer;
        private readonly IMessageSubscriber<string> _messageSubscriber;
        private readonly MessageFeedHandler _messageFeedHandler;
        private const string KITCHEN_STAFF = "Kitchen Staff";

        public KitchenStaffController(IMessageProducer messageProducer, IMessageSubscriber<string> messageSubscriber, MessageFeedHandler messageFeedHandler)
        {
            _messageProducer = messageProducer;
            _messageSubscriber = messageSubscriber;
            _messageFeedHandler = messageFeedHandler;
        }

        /// <summary>
        /// Fetches status update information and returns it back to the client.
        /// </summary>
        /// <returns>Status update information if it can be retrieved</returns>
        [HttpGet]
        public async Task<IActionResult> Status()
        {
            // Queue Name
            const string queueName = "StatusUpdate-KitchenStaff";

            // Initial DTO
            DataWithCorrelation dto = new();
            dto.ClientUUID = ControllerAids.SearchRequestForClientUUID(Request.Headers, dto.CorrelationGuid);

            // Send Message
            _messageProducer.SendMessage(queueName, dto);
            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation("Request for a status update of importance to kitchen staff made", dto));

            // Allow time for the data to be handled
            await Task.Delay(2000); // 2 seconds

            // Await Response
            var response = await ControllerAids.TaskListenForEventResponse<KitchenStaffFullStatusDTO>(_messageSubscriber, dto.CorrelationGuid, queueName);

            if (response == null)
            {
                Log.Warning(LoggingHelper.FormatMessageWithCorrelationInformation("No status update received for the kitchen staff",
                    dto));
                return StatusCode(503, "No response from service, the request may or may not have been successful");
            }


            // Handle Response
            return Ok(new Dictionary<string, dynamic>() {
                {"Tables", response.Tables }
            });
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> DeclareOrderIsStarted([FromForm] string orderGuid)
        {
            // Queue Name
            const string queueName = "OrderBeingPreparedForTable";

            // Initial DTO
            SingleStringDTO dto = new();
            dto.ClientUUID = ControllerAids.SearchRequestForClientUUID(Request.Headers, dto.CorrelationGuid);
            dto.SingleStringValue = orderGuid;

            // Send Message
            _messageProducer.SendMessage(queueName, dto);
            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation($"Request to declare order {orderGuid}, as being started, made", dto));

            // Allow time for the data to be handled
            await Task.Delay(2000); // 2 seconds

            // Await Response
            var response = await ControllerAids.TaskListenForEventResponse<EventResponse>(_messageSubscriber, dto.CorrelationGuid, queueName);

            if (response == null)
            {
                Log.Warning(LoggingHelper.FormatMessageWithCorrelationInformation("No response as to whether the order was set as started",
                    dto));
                return StatusCode(503, "No response from service, the request may or may not have been successful");
            }
            if (response.ResponseCode < 0) return StatusCode(400, $"Did not declare the order as started, reason {response.Message}");

            // Handle Response
            _messageFeedHandler.TrackNewEvent(KITCHEN_STAFF, $"The kitchen staff have started on order {orderGuid}");
            return Ok();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> DeclareOrderIsReadyForServing([FromForm] string orderGuid)
        {
            // Queue Name
            const string queueName = "OrderForTableReadyForServing";

            // Initial DTO
            SingleStringDTO dto = new();
            dto.ClientUUID = ControllerAids.SearchRequestForClientUUID(Request.Headers, dto.CorrelationGuid);
            dto.SingleStringValue= orderGuid;

            // Send Message
            _messageProducer.SendMessage(queueName, dto);
            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation($"Request to declare that order {orderGuid} is ready for serving has been made", dto));

            // Allow time for the data to be handled
            await Task.Delay(2000); // 2 seconds

            // Await Response
            var response = await ControllerAids.TaskListenForEventResponse<EventResponse>(_messageSubscriber, dto.CorrelationGuid, queueName);

            if (response == null)
            {
                Log.Warning(LoggingHelper.FormatMessageWithCorrelationInformation("No response on whether the order was declared as ready for serving",
                    dto));
                return StatusCode(503, "No response from service, the request may or may not have been successful");
            }
            if (response.ResponseCode < 0) return StatusCode(400, $"Did not declare the order as ready for serving, reason {response.Message}");

            // Handle Response
            _messageFeedHandler.TrackNewEvent(KITCHEN_STAFF, $"The kitchen staff have finished preparing order ${orderGuid}");
            return Ok();
        }
    }
}
