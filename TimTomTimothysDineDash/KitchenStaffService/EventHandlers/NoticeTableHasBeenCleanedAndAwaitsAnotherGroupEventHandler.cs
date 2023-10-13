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
    /// This NoticeTableHasBeenCleanedAndAwaitsAnotherGroupEventHandler class reacts to the bus staff indicating that the table has been cleaned and can be reused.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class NoticeTableHasBeenCleanedAndAwaitsAnotherGroupEventHandler : EventHandlerBase
    {
        private readonly RepositoryBase<ApplicationDbContext, Table> _tableRepo;
        public const string EVENT = "TableHasBeenCleanedAndAwaitsAnotherGroup-KitchenStaff";


        public NoticeTableHasBeenCleanedAndAwaitsAnotherGroupEventHandler(
            RepositoryBase<ApplicationDbContext, Table> tableRepo,
            IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
            _tableRepo = tableRepo;
        }

        /// <summary>
        /// Handles the event and sends a response message indicating a success or failure.
        /// </summary>
        public async Task Handle(SingleStringDTO dto)
        {
            var table = _tableRepo.GetByID(int.Parse(dto.SingleStringValue));

            table.OrderId = null;
            _tableRepo.Update(table);

            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation("Updated the table to reflect that it is available again", dto));
        }
    }
}
