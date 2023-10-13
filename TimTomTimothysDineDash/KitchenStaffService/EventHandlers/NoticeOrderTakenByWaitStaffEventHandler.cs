using BarebonesEFCoreRepositoryPatternStarter.Data;
using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Helpers;
using BarebonesRabbitMQImplementationLibrary.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Models;
using BarebonesRabbitMQImplementationLibrary.Models.DTO;
using KitchenStaffService.Database.Data;
using KitchenStaffService.Database.Models;
using KitchenStaffService.Models.DTO;
using Serilog;

namespace KitchenStaffService.EventHandlers
{
    /// <summary>
    /// This NoticeOrderTakenByWaitStaffEventHandler class reacts to the wait staff inserting an order for a table.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class NoticeOrderTakenByWaitStaffEventHandler : EventHandlerBase
    {
        private readonly RepositoryBase<ApplicationDbContext, Table> _tableRepo;
        private readonly RepositoryBase<ApplicationDbContext, Order> _orderRepo;
        public const string EVENT = "OrderTakenByWaitStaff-KitchenStaff";


        public NoticeOrderTakenByWaitStaffEventHandler(
            RepositoryBase<ApplicationDbContext, Table> tableRepo,
            IMessageProducer messageProducer,
            RepositoryBase<ApplicationDbContext, Order> orderRepo)
        {
            _messageProducer = messageProducer;
            _tableRepo = tableRepo;
            _orderRepo = orderRepo;
        }

        /// <summary>
        /// Handles the event and sends a response message indicating a success or failure.
        /// </summary>
        public async Task Handle(NewOrderForKitchenDTO dto)
        {
            var table = _tableRepo.GetByID(dto.TableId);

            var order = new Order()
            {
                OrderGuid = dto.OrderGuid,
                Description = dto.OrderDescription,
                IsStarted = false,
                IsFinished = false
            };
            _orderRepo.Insert(order);

            table.OrderId = order.Id;
            _tableRepo.Update(table);

            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation("Updated the table with the order's id and created a new order", dto));
        }
    }
}
