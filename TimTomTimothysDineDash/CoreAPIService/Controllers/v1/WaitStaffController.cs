using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Helpers;
using BarebonesRabbitMQImplementationLibrary.Models;
using BarebonesRabbitMQImplementationLibrary.Models.DTO;
using CoreAPIService.Helpers;
using CoreAPIService.Models.DTO;
using CoreAPIService.Models.DTO.StatusUpdate.HostStaff;
using CoreAPIService.Models.DTO.WaitStaff;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CoreAPIService.Controllers.v1
{
    [Route("v{version:apiVersion}/[controller]/[action]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class WaitStaffController : ControllerBase
    {
        private readonly IMessageProducer _messageProducer;
        private readonly IMessageSubscriber<string> _messageSubscriber;
        private readonly MessageFeedHandler _messageFeedHandler;
        private const string WAIT_STAFF = "Wait Staff";

        public WaitStaffController(IMessageProducer messageProducer, IMessageSubscriber<string> messageSubscriber, MessageFeedHandler messageFeedHandler)
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
            const string queueName = "StatusUpdate-WaitStaff";

            // Initial DTO
            DataWithCorrelation dto = new();
            dto.ClientUUID = ControllerAids.SearchRequestForClientUUID(Request.Headers, dto.CorrelationGuid);

            // Send Message
            _messageProducer.SendMessage(queueName, dto);
            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation("Request for a status update of importance to wait staff made", dto));

            // Allow time for the data to be handled
            await Task.Delay(2000); // 2 seconds

            // Await Response
            var response = await ControllerAids.TaskListenForEventResponse<WaitStaffFullStatusDTO>(_messageSubscriber, dto.CorrelationGuid, queueName);

            if (response == null)
            {
                Log.Warning(LoggingHelper.FormatMessageWithCorrelationInformation("No status update received for the wait staff",
                    dto));
                return StatusCode(503, "No response from service, the request may or may not have been successful");
            }


            // Handle Response
            return Ok(new Dictionary<string, dynamic>() {
                {"Tables", response.Tables }
            });
        }

        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> InsertOrderForTable([FromForm] int tableId, [FromForm] string orderDescription)
        {
            // Queue Name
            const string queueName = "OrderTakenByWaitStaff";

            // Initial DTO
            NewOrderDTO dto = new();
            dto.ClientUUID = ControllerAids.SearchRequestForClientUUID(Request.Headers, dto.CorrelationGuid);
            dto.OrderDescription = orderDescription;
            dto.TableId = tableId;


            // Send Message
            _messageProducer.SendMessage(queueName, dto);
            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation("Request to insert an order for a table by wait staff made", dto));

            // Allow time for the data to be handled
            await Task.Delay(2000); // 2 seconds

            // Await Response
            var response = await ControllerAids.TaskListenForEventResponse<EventResponse>(_messageSubscriber, dto.CorrelationGuid, queueName);

            if (response == null)
            {
                Log.Warning(LoggingHelper.FormatMessageWithCorrelationInformation("No notice on whether the order has been added to the table or not",
                    dto));
                return StatusCode(503, "No response from service, the request may or may not have been successful");
            }
            if (response.ResponseCode < 0) return StatusCode(400, $"Did not add the order for the table, reason: {response.Message}");

            // Handle Response
            _messageFeedHandler.TrackNewEvent(WAIT_STAFF, $"The wait staff have taken an order for table {tableId}");
            return Ok();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> MarkTableAsHavingPaidAndLeft([FromForm] int tableId)
        {
            // Queue Name
            const string queueName = "TableHasPaidForOrderAndLeft";

            // Initial DTO
            SingleStringDTO dto = new();
            dto.ClientUUID = ControllerAids.SearchRequestForClientUUID(Request.Headers, dto.CorrelationGuid);
            dto.SingleStringValue = tableId.ToString();

            // Check for valid data
            if (tableId <= 0)
            {
                Log.Information($"Rejected request to mark table {tableId} as paid and having left, because no table can have that id", dto);
                return BadRequest("Tables do not have ids less than one");
            }

            // Send Message
            _messageProducer.SendMessage(queueName, dto);
            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation($"Request to mark table {tableId} as having paid and left made", dto));

            // Allow time for the data to be handled
            await Task.Delay(2000); // 2 seconds

            // Await Response
            var response = await ControllerAids.TaskListenForEventResponse<EventResponse>(_messageSubscriber, dto.CorrelationGuid, queueName);

            if (response == null)
            {
                Log.Warning(LoggingHelper.FormatMessageWithCorrelationInformation("No response received when trying to mark a table as having paid and left",
                    dto));
                return StatusCode(503, "No response from service, the request may or may not have been successful");
            }
            if (response?.ResponseCode < 0)
            {
                Log.Warning(LoggingHelper.FormatMessageWithCorrelationInformation($"The response for marking a table as paid for, indicates a failure with reason: {response.Message ?? "N/A"}",
                    dto));
                return StatusCode(400, $"Did not mark the specified table as having paid, reason: {response.Message ?? "N/A"}");
            }


            // Handle Response
            _messageFeedHandler.TrackNewEvent(WAIT_STAFF, $"The wait staff have marked table {tableId} as having paid and left");
            return Ok();
        }
    }
}
