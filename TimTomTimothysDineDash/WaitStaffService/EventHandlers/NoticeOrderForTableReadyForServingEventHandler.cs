using BarebonesEFCoreRepositoryPatternStarter.Data;
using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Helpers;
using BarebonesRabbitMQImplementationLibrary.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Models;
using BarebonesRabbitMQImplementationLibrary.Models.DTO;
using WaitStaffService.Database.Data;
using WaitStaffService.Database.Models;
using WaitStaffService.Models.DTO;
using Serilog;

namespace WaitStaffService.EventHandlers
{
    /// <summary>
    /// This NoticeOrderForTableReadyForServingEventHandler class reacts to the kitchen staff indicating that the meal items from the order are ready to serve.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class NoticeOrderForTableReadyForServingEventHandler : EventHandlerBase
    {
        private readonly RepositoryBase<ApplicationDbContext, Order> _orderRepo;
        public const string EVENT = "OrderForTableReadyForServing-WaitStaff";


        public NoticeOrderForTableReadyForServingEventHandler(
            RepositoryBase<ApplicationDbContext, Order> orderRepo,
            IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
            _orderRepo = orderRepo;
        }

        /// <summary>
        /// Handles the event and sends a response message indicating a success or failure.
        /// </summary>
        public async Task Handle(SingleStringDTO dto)
        {
            var order = _orderRepo.Get(o => o.OrderGuid == dto.SingleStringValue).FirstOrDefault();
            order.IsFinished = true;
            
            _orderRepo.Update(order);

            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation($"Updated order {order.OrderGuid} to reflect that the food has been finished" , dto));
        }
    }
}
