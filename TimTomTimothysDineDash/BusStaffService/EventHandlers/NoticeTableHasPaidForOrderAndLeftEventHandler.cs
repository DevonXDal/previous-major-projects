using BarebonesEFCoreRepositoryPatternStarter.Data;
using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Helpers;
using BarebonesRabbitMQImplementationLibrary.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Models;
using BarebonesRabbitMQImplementationLibrary.Models.DTO;
using BusStaffService.Database.Data;
using BusStaffService.Database.Models;
using BusStaffService.Models.DTO.StatusUpdate;
using Serilog;

namespace BusStaffService.EventHandlers
{
    /// <summary>
    /// This NoticeTableHasPaidForOrderAndLeftEventHandler class reacts to the wait staff
    /// declaring that a table has paid and left.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class NoticeTableHasPaidForOrderAndLeftEventHandler : EventHandlerBase
    {
        private readonly RepositoryBase<ApplicationDbContext, Table> _tableRepo;
        public const string EVENT = "TableHasPaidForOrderAndLeft-BusStaff";


        public NoticeTableHasPaidForOrderAndLeftEventHandler(
            IMessageProducer messageProducer, 
            RepositoryBase<ApplicationDbContext, Table> tableRepo)
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

            table.HasPaid = true;
            table.IsNeedingCleaned = true;
            _tableRepo.Update(table);

            Log.Information($"Marked table {table.Id} as having paid and needing cleaned");
        }
    }
}
