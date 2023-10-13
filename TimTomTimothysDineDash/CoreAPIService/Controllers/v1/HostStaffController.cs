using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Helpers;
using BarebonesRabbitMQImplementationLibrary.Models;
using BarebonesRabbitMQImplementationLibrary.Models.DTO;
using CoreAPIService.Helpers;
using CoreAPIService.Models.DTO;
using CoreAPIService.Models.DTO.HostStaff;
using CoreAPIService.Models.DTO.StatusUpdate.HostStaff;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CoreAPIService.Controllers.v1
{
    [Route("v{version:apiVersion}/[controller]/[action]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class HostStaffController : ControllerBase
    {
        private readonly IMessageProducer _messageProducer;
        private readonly IMessageSubscriber<string> _messageSubscriber;
        private readonly MessageFeedHandler _messageFeedHandler;
        private const string HOST_STAFF = "Host Staff";

        public HostStaffController(IMessageProducer messageProducer, IMessageSubscriber<string> messageSubscriber, MessageFeedHandler messageFeedHandler)
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
            const string queueName = "StatusUpdate-HostStaff";

            // Initial DTO
            DataWithCorrelation dto = new();
            dto.ClientUUID = ControllerAids.SearchRequestForClientUUID(Request.Headers, dto.CorrelationGuid);

            // Send Message
            _messageProducer.SendMessage(queueName, dto);
            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation("Request for a status update of importance to host staff made", dto));

            // Allow time for the data to be handled
            await Task.Delay(2000); // 2 seconds

            // Await Response
            var response = await ControllerAids.TaskListenForEventResponse<HostStaffFullStatusDTO>(_messageSubscriber, dto.CorrelationGuid, queueName);

            if (response == null)
            {
                Log.Warning(LoggingHelper.FormatMessageWithCorrelationInformation("No status update received for the host staff",
                    dto));
                return StatusCode(503, "No response from service, the request may or may not have been successful");
            }

            // Handle Response
            return Ok(new Dictionary<string, dynamic>() {
                {"Tables", response.Tables },
                {"QueueParties", response.QueueParties }
            });
        }

        /// <summary>
        /// Adds a new group of customers to the queue for host staff to seat when tables are available.
        /// </summary>
        /// <param name="numOfCustomers">The number of customers needing to be seated</param>
        /// <returns>A message indicating a success or failure</returns>
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> DeclareNewCustomersInQueue([FromForm] int numOfCustomers)
        {
            const string queueName = "EnqueueIntoWaitList";

            NewWaitListGroupDto dto = new NewWaitListGroupDto();
            dto.NumberNeedingSeated = numOfCustomers;
            dto.ClientUUID = ControllerAids.SearchRequestForClientUUID(Request.Headers, dto.CorrelationGuid);

            if (numOfCustomers < 1 || numOfCustomers > 6)
            {
                Log.Information($"Rejected request to add {numOfCustomers} to the queue. Tables seat up to 6", dto);
                return BadRequest("Tables must seat between 1 and 6 people");
            }

            _messageProducer.SendMessage(queueName, dto);
            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation($"Request to add {numOfCustomers} to the waiting list made", dto));

            await Task.Delay(2000); // 2 seconds

            var response = await ControllerAids.TaskListenForEventResponse<EventResponse>(_messageSubscriber, dto.CorrelationGuid, queueName);

            if (response == null)
            {
                Log.Warning(LoggingHelper.FormatMessageWithCorrelationInformation("No response on whether the customers were added to the queue successfully",
                    dto));
                return StatusCode(503, "No response from service, the request may or may not have been successful");
            }

            if (response.ResponseCode < 0)
            {
                return StatusCode(400, "The customers were not added successfully");
            }

            _messageFeedHandler.TrackNewEvent(HOST_STAFF, $"A group of {numOfCustomers} has been added to the queue");
            return Ok("The customers were added to the waiting list successfully");
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SeatNextGroupAtTable([FromForm] int tableId)
        {
            // Queue Name
            const string queueName = "SeatPeopleAtTable";

            // Initial DTO
            SingleStringDTO dto = new();
            dto.ClientUUID = ControllerAids.SearchRequestForClientUUID(Request.Headers, dto.CorrelationGuid);
            dto.SingleStringValue = tableId.ToString();

            // Check for valid data
            if (tableId <= 0)
            {
                Log.Information($"Rejected request to seat the next group at table {tableId}, because no table can have that id", dto);
                return BadRequest("Tables do not have ids less than one");
            }

            // Send Message
            _messageProducer.SendMessage(queueName, dto);
            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation($"Request to seat the next group at table {tableId} made", dto));

            // Allow time for the data to be handled
            await Task.Delay(2000); // 2 seconds

            // Await Response
            var response = await ControllerAids.TaskListenForEventResponse<EventResponse>(_messageSubscriber, dto.CorrelationGuid, queueName);

            if (response == null)
            {
                Log.Warning(LoggingHelper.FormatMessageWithCorrelationInformation("No response to as whether the table was seated",
                    dto));
                return StatusCode(503, "No response from service, the request may or may not have been successful");
            }
            if (response?.ResponseCode < 0)
            {
                Log.Warning(LoggingHelper.FormatMessageWithCorrelationInformation($"Did not successfully seat the next group at the specified table, " +
                    $"reason: {response.Message ?? "N/A"}",
                    dto));
                return StatusCode(400, $"Did not seat group at a the specified table, reason: {response.Message ?? "N/A"}");
            }

            // Handle Response
            _messageFeedHandler.TrackNewEvent(HOST_STAFF, $"A group of customers have been seated at table {tableId}");
            return Ok();
        }
    }
}
