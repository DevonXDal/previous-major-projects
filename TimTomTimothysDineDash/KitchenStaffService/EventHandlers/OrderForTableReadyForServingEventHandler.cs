using BarebonesEFCoreRepositoryPatternStarter.Data;
using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Helpers;
using BarebonesRabbitMQImplementationLibrary.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Models;
using BarebonesRabbitMQImplementationLibrary.Models.DTO;
using KitchenStaffService.Database.Data;
using KitchenStaffService.Database.Models;
using KitchenStaffService.Models.DTO.StatusUpdate;
using Serilog;

namespace KitchenStaffService.EventHandlers
{
    /// <summary>
    /// This OrderForTableReadyForServingEventHandler class handles marking an order as having been finished in the kitchen
    /// and that it is ready to be given to the customers.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class OrderForTableReadyForServingEventHandler : EventHandlerBase
    {
        private readonly RepositoryBase<ApplicationDbContext, Order> _orderRepo;
        public const string EVENT = "OrderForTableReadyForServing";


        public OrderForTableReadyForServingEventHandler(
            IMessageProducer messageProducer, 
            RepositoryBase<ApplicationDbContext, Order> orderRepo)
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
            if (order == null)
            {
                SendBasicResponse(EVENT, $"Could not mark an order as finished because no order has the GUID: {dto.SingleStringValue}",
                    -1, dto, $"There is no order with the id: {dto.SingleStringValue}");
                return;
            } else if (order.IsFinished) {
                SendBasicResponse(EVENT, $"Could not mark an order as finished because the order is already finished",
                    -1, dto, $"This order has already been finished");
                return;
            }

            order.IsFinished = true;
            _orderRepo.Update(order);

            SendBasicResponse(EVENT, $"Order {order.OrderGuid} has been finished", 0, dto);
            _messageProducer.SendMessage($"{EVENT}-WaitStaff", dto);
        }
    }
}
