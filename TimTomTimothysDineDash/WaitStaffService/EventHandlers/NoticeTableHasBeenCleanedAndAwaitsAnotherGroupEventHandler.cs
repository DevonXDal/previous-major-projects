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
    /// This NoticeTableHasBeenCleanedAndAwaitsAnotherGroupEventHandler class reacts to the bus staff indicating that the table has been cleaned and can be reused.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class NoticeTableHasBeenCleanedAndAwaitsAnotherGroupEventHandler : EventHandlerBase
    {
        private readonly RepositoryBase<ApplicationDbContext, Table> _tableRepo;
        public const string EVENT = "TableHasBeenCleanedAndAwaitsAnotherGroup-WaitStaff";


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

            table.SeatsUsed = 0;
            table.OrderId = null;
            _tableRepo.Update(table);

            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation("Updated the table to reflect that it is available again", dto));
        }
    }
}
