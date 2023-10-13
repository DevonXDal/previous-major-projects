using BarebonesEFCoreRepositoryPatternStarter.Data;
using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Helpers;
using BarebonesRabbitMQImplementationLibrary.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Models;
using HostStaffService.Database.Data;
using HostStaffService.Database.Models;
using HostStaffService.Models.DTO;
using Serilog;

namespace HostStaffService.EventHandlers
{
    /// <summary>
    /// This EnqueueIntoWaitListEventHandler class handles inserted a new group of people who are waiting for seating at a table.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class EnqueueIntoWaitListEventHandler : EventHandlerBase
    {
        private readonly RepositoryBase<ApplicationDbContext, CustomerQueue> _customerQueueRepo;
        public const string EVENT = "EnqueueIntoWaitList";


        public EnqueueIntoWaitListEventHandler(RepositoryBase<ApplicationDbContext, CustomerQueue> customerQueueRepo, IMessageProducer messageProducer)
        {
            _customerQueueRepo = customerQueueRepo;
            _messageProducer = messageProducer;
        }

        /// <summary>
        /// Handles the event and sends a response message indicating a success or failure.
        /// </summary>
        public async Task Handle(NewWaitListGroupDto newWaitListGroup)
        {
            _customerQueueRepo.Insert(new CustomerQueue()
            {
                HasBeenSeated = false,
                NumToSeat = newWaitListGroup.NumberNeedingSeated
            });

            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation("Successfully added a new group to the wait list", newWaitListGroup));

            _messageProducer.SendMessage($"{EVENT}--{newWaitListGroup.CorrelationGuid}",
                new EventResponse()
                {
                    ClientUUID = newWaitListGroup.ClientUUID,
                    CorrelationGuid = newWaitListGroup.CorrelationGuid,
                    InitialCreation = newWaitListGroup.InitialCreation,
                    ResponseCode = 0
                },
                args: RabbitMQOptionPresets.SHOULD_EXPIRE_AFTER_2_MINUTES
            );
        }
    }
}
