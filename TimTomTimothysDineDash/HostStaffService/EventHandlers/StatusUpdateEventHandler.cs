using BarebonesEFCoreRepositoryPatternStarter.Data;
using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Helpers;
using BarebonesRabbitMQImplementationLibrary.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Models;
using HostStaffService.Database.Data;
using HostStaffService.Database.Models;
using HostStaffService.Models.DTO.StatusUpdate;
using Serilog;

namespace HostStaffService.EventHandlers
{
    /// <summary>
    /// This StatusUpdateEventHandler class handles retreiving the current state of information
    /// needed by this group of staff.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class StatusUpdateEventHandler : EventHandlerBase
    {
        private readonly RepositoryBase<ApplicationDbContext, CustomerQueue> _customerQueueRepo;
        private readonly RepositoryBase<ApplicationDbContext, Table> _tableRepo;
        public const string EVENT = "StatusUpdate-HostStaff";


        public StatusUpdateEventHandler(
            RepositoryBase<ApplicationDbContext, CustomerQueue> customerQueueRepo,
            IMessageProducer messageProducer, 
            RepositoryBase<ApplicationDbContext, Table> tableRepo)
        {
            _customerQueueRepo = customerQueueRepo;
            _messageProducer = messageProducer;
            _tableRepo = tableRepo;
        }

        /// <summary>
        /// Handles the event and sends a response message indicating a success or failure.
        /// </summary>
        public async Task Handle(DataWithCorrelation dto)
        {
            HostStaffFullStatusDTO status = new();
            var tables = _tableRepo.Get();
            var customerQueue = _customerQueueRepo.Get(p => !p.HasBeenSeated); // The ones that are not in the queue do not matter

            status.Tables = tables.Select(t => new HostStaffTableStatusDTO()
            {
                Id = t.Id,
                IsAvailable = t.IsAvailable,
                LastUpdate = t.LastModified
            }).ToList();

            status.QueueParties = customerQueue.Select(p => new HostStaffQueueStatusDTO()
            {
                Id = p.Id,
                NumToSeat = p.NumToSeat,
                LastUpdate = p.LastModified
            }).ToList();

            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation("Successfully retrieved an update on the tables and customer waiting queue", dto));

            _messageProducer.SendMessage($"{EVENT}--{dto.CorrelationGuid}",
                status,
                args: RabbitMQOptionPresets.SHOULD_EXPIRE_AFTER_2_MINUTES
            );
        }
    }
}
