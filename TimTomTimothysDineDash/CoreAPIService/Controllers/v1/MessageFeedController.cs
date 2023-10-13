using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Helpers;
using BarebonesRabbitMQImplementationLibrary.Models;
using BarebonesRabbitMQImplementationLibrary.Models.DTO;
using CoreAPIService.Helpers;
using CoreAPIService.Models.DTO;
using CoreAPIService.Models.DTO.StatusUpdate.HostStaff;
using CoreAPIService.Models.DTO.StatusUpdate.MessageFeed;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CoreAPIService.Controllers.v1
{
    [Route("v{version:apiVersion}/[controller]/[action]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class MessageFeedController : ControllerBase
    {
        private readonly MessageFeedHandler _messageFeedHandler;

        public MessageFeedController(MessageFeedHandler messageFeedHandler)
        {
            _messageFeedHandler = messageFeedHandler;
        }

        /// <summary>
        /// Fetches status update information and returns it back to the client.
        /// </summary>
        /// <returns>Status update information if it can be retrieved</returns>
        [HttpGet]
        public async Task<IActionResult> Status()
        {
            // Initial DTO
            DataWithCorrelation dto = new();
            dto.ClientUUID = ControllerAids.SearchRequestForClientUUID(Request.Headers, dto.CorrelationGuid);

            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation("Request for a status update on the message feed made", dto));

            // Retreive the messages (recent 5)
            var messages = _messageFeedHandler.GetRecentMessages().Select(m => new MessageDTO()
            {
                Title = m.ServiceRelation,
                Content = m.Description,
                Created = m.Created
            });


            // Handle Response
            return Ok(new Dictionary<string, dynamic>() {
                {"Messages", messages }
            });
        }
    }
}
