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
    /// This OrderBeingPreparedEventHandler class handles marking an order as being prepared.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class OrderBeingPreparedForTableEventHandler : EventHandlerBase
    {
        private readonly RepositoryBase<ApplicationDbContext, Order> _orderRepo;
        public const string EVENT = "OrderBeingPreparedForTable";


        public OrderBeingPreparedForTableEventHandler(
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
                SendBasicResponse(EVENT, $"Could not mark an order as started because no order has the GUID: {dto.SingleStringValue}",
                    -1, dto, $"There is no order with the id: {dto.SingleStringValue}");
                return;
            } else if (order.IsStarted) {
                SendBasicResponse(EVENT, $"Could not mark an order as started because the order is already started",
                    -1, dto, $"This order has already been started");
                return;
            }

            order.IsStarted = true;
            _orderRepo.Update(order);

            SendBasicResponse(EVENT, $"Order {order.OrderGuid} has been started", 0, dto);
        }
    }
}
