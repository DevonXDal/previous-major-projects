using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Helpers;
using BarebonesRabbitMQImplementationLibrary.Models;
using BarebonesRabbitMQImplementationLibrary.Models.DTO;
using CoreAPIService.Helpers;
using CoreAPIService.Models.DTO;
using CoreAPIService.Models.DTO.HostStaff;
using CoreAPIService.Models.DTO.StatusUpdate.BusStaff;
using CoreAPIService.Models.DTO.StatusUpdate.HostStaff;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CoreAPIService.Controllers.v1
{
    [Route("v{version:apiVersion}/[controller]/[action]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class BusStaffController : ControllerBase
    {
        private readonly IMessageProducer _messageProducer;
        private readonly IMessageSubscriber<string> _messageSubscriber;
        private readonly MessageFeedHandler _messageFeedHandler;
        private const string BUS_STAFF = "Bus Staff";

        public BusStaffController(IMessageProducer messageProducer, IMessageSubscriber<string> messageSubscriber, MessageFeedHandler messageFeedHandler)
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
            const string queueName = "StatusUpdate-BusStaff";

            // Initial DTO
            DataWithCorrelation dto = new();
            dto.ClientUUID = ControllerAids.SearchRequestForClientUUID(Request.Headers, dto.CorrelationGuid);

            // Send Message
            _messageProducer.SendMessage(queueName, dto);
            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation("Request for a status update of importance to bus staff made", dto));

            // Allow time for the data to be handled
            await Task.Delay(2000); // 2 seconds

            // Await Response
            var response = await ControllerAids.TaskListenForEventResponse<BusStaffFullStatusDTO>(_messageSubscriber, dto.CorrelationGuid, queueName);

            if (response == null)
            {
                Log.Warning(LoggingHelper.FormatMessageWithCorrelationInformation("No status update received for the bus staff",
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
        public async Task<IActionResult> MarkTableAsCleanedForTheNextGroup([FromForm] int tableId)
        {
            // Queue Name
            const string queueName = "TableHasBeenCleanedAndAwaitsAnotherGroup";

            // Initial DTO
            SingleStringDTO dto = new();
            dto.ClientUUID = ControllerAids.SearchRequestForClientUUID(Request.Headers, dto.CorrelationGuid);
            dto.SingleStringValue = tableId.ToString();

            // Check for valid data
            if (tableId <= 0)
            {
                Log.Information($"Rejected request to mark table {tableId} as being cleaned, because no table can have that id", dto);
                return BadRequest("Tables do not have ids less than one");
            }

            // Send Message
            _messageProducer.SendMessage(queueName, dto);
            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation($"Request to mark table {tableId} as having been cleaned made", dto));

            // Allow time for the data to be handled
            await Task.Delay(2000); // 2 seconds

            // Await Response
            var response = await ControllerAids.TaskListenForEventResponse<EventResponse>(_messageSubscriber, dto.CorrelationGuid, queueName);

            if (response == null)
            {
                Log.Warning(LoggingHelper.FormatMessageWithCorrelationInformation("No response received when trying to mark a table as having been cleaned.",
                    dto));
                return StatusCode(503, "No response from service, the request may or may not have been successful");
            }
            if (response?.ResponseCode < 0)
            {
                Log.Warning(LoggingHelper.FormatMessageWithCorrelationInformation($"The response for marking a table as cleaned indicates a failure with reason: {response.Message ?? "N/A"}",
                    dto));
                return StatusCode(500, $"Did not mark the specified table as having been cleaned, reason: {response.Message ?? "N/A"}");
            }


            // Handle Response
            _messageFeedHandler.TrackNewEvent(BUS_STAFF, $"The bus staff have marked table {tableId} as having been cleaned. It can now be used again.");
            return Ok();
        }
    }
}
