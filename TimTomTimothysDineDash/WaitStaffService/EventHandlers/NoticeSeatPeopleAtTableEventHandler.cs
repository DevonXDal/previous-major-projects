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
    /// This NoticeSeatPeopleAtTableEventHandler class reacts to the host staff indicating that people have been seated at a table.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class NoticeSeatPeopleAtTableEventHandler : EventHandlerBase
    {
        private readonly RepositoryBase<ApplicationDbContext, Table> _tableRepo;
        public const string EVENT = "SeatPeopleAtTable-WaitStaff";


        public NoticeSeatPeopleAtTableEventHandler(
            RepositoryBase<ApplicationDbContext, Table> tableRepo,
            IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
            _tableRepo = tableRepo;
        }

        /// <summary>
        /// Handles the event and sends a response message indicating a success or failure.
        /// </summary>
        public async Task Handle(TableHasBeenSeatedDTO dto)
        {
            var table = _tableRepo.GetByID(dto.TableId);

            table.SeatsUsed = dto.NumSeated;
            _tableRepo.Update(table);

            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation(
                $"Updated table {table.Id} to indicate there are {dto.NumSeated} people sitting and waiting to order", dto));
        }
    }
}
