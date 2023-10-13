using BarebonesEFCoreRepositoryPatternStarter.Data;
using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Helpers;
using BarebonesRabbitMQImplementationLibrary.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Models;
using BusStaffService.Database.Data;
using BusStaffService.Database.Models;
using BusStaffService.Models.DTO.StatusUpdate;
using Serilog;

namespace BusStaffService.EventHandlers
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
        public const string EVENT = "StatusUpdate-BusStaff";


        public StatusUpdateEventHandler(
            IMessageProducer messageProducer, 
            RepositoryBase<ApplicationDbContext, Table> tableRepo)
        {
            _messageProducer = messageProducer;
            _tableRepo = tableRepo;
        }

        /// <summary>
        /// Handles the event and sends a response message indicating a success or failure.
        /// </summary>
        public async Task Handle(DataWithCorrelation dto)
        {
            BusStaffFullStatusDTO status = new();
            var tables = _tableRepo.Get();

            status.Tables = tables.Select(t => new BusStaffTableStatusDTO()
            {
                Id = t.Id,
                HasPaid = t.HasPaid,
                IsNeedingCleaned = t.IsNeedingCleaned
            }).ToList();

            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation("Successfully retrieved an update on the tables", dto));

            _messageProducer.SendMessage($"{EVENT}--{dto.CorrelationGuid}",
                status,
                args: RabbitMQOptionPresets.SHOULD_EXPIRE_AFTER_2_MINUTES
            );
        }
    }
}
