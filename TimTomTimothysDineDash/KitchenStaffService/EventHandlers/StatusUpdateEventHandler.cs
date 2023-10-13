using BarebonesEFCoreRepositoryPatternStarter.Data;
using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Helpers;
using BarebonesRabbitMQImplementationLibrary.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Models;
using KitchenStaffService.Database.Data;
using KitchenStaffService.Database.Models;
using KitchenStaffService.Models.DTO.StatusUpdate;
using Serilog;

namespace KitchenStaffService.EventHandlers
{
    /// <summary>
    /// This StatusUpdateEventHandler class handles retreiving the current state of information
    /// needed by this group of staff.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class StatusUpdateEventHandler : EventHandlerBase
    {
        private readonly RepositoryBase<ApplicationDbContext, Table> _tableRepo;
        private readonly RepositoryBase<ApplicationDbContext, Order> _orderRepo;
        public const string EVENT = "StatusUpdate-KitchenStaff";


        public StatusUpdateEventHandler(
            IMessageProducer messageProducer, 
            RepositoryBase<ApplicationDbContext, Table> tableRepo,
            RepositoryBase<ApplicationDbContext, Order> orderRepo)
        {
            _messageProducer = messageProducer;
            _tableRepo = tableRepo;
            _orderRepo = orderRepo;
        }

        /// <summary>
        /// Handles the event and sends a response message indicating a success or failure.
        /// </summary>
        public async Task Handle(DataWithCorrelation dto)
        {
            KitchenStaffFullStatusDTO status = new();
            var tables = _tableRepo.Get();
            var orders = _orderRepo.Get();

            status.Tables = tables.Select(t => FormTableStatus(t)).ToList();

            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation("Successfully retrieved an update on the tables and orders", dto));

            _messageProducer.SendMessage($"{EVENT}--{dto.CorrelationGuid}",
                status,
                args: RabbitMQOptionPresets.SHOULD_EXPIRE_AFTER_2_MINUTES
            );

            // Reads from the orders list in the outer method
            KitchenStaffTableStatusDTO FormTableStatus(Table t)
            {
                KitchenStaffTableStatusDTO tableStatus = new()
                {
                    Id = t.Id,
                    LastUpdate = t.LastModified
                };

                if (t.OrderId != null)
                {
                    var order = orders.FirstOrDefault(o => o.Id == t.OrderId);

                    tableStatus.OrderGuid = order.OrderGuid;
                    tableStatus.OrderDescription = order.Description;
                    tableStatus.IsOrderStarted = order.IsStarted;
                    tableStatus.IsOrderFinished = order.IsFinished;
                    
                    if (order.LastModified > t.LastModified) tableStatus.LastUpdate = order.LastModified;
                }

                return tableStatus;
            }
        }
    }
}
