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
    /// This TableHasBeenCleanedAndAwaitsAnotherGroup class handles calling for every
    /// set of data, on all tables to be reset, upon hearing a table has been cleaned.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class TableHasBeenCleanedAndAwaitsAnotherGroup : EventHandlerBase
    {
        private readonly RepositoryBase<ApplicationDbContext, Table> _tableRepo;
        public const string EVENT = "TableHasBeenCleanedAndAwaitsAnotherGroup";


        public TableHasBeenCleanedAndAwaitsAnotherGroup(
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
            Table table;

            try
            {
                table = _tableRepo.GetByID(int.Parse(dto.SingleStringValue));
            } catch (Exception _)
            {
                SendBasicResponse(EVENT, $"Could not mark a table as cleaned since the id was not a number", -1, dto, "The provided table id must be an integer");
                return;
            }

            if (table == null)
            {
                SendBasicResponse(EVENT, "Could not mark a table as cleaned since no table has that id", -2, dto, "No table found with that id");
                return;
            } else if (!table.IsNeedingCleaned || !table.HasPaid)
            {
                SendBasicResponse(EVENT, "Could not mark a table as cleaned since the table needs to be paid for and marked for cleaning",
                    -2, dto, 
                    "The table must have paid its order and be marked for cleaning");
                return;
            }

            table.HasPaid = false;
            table.IsNeedingCleaned = false;
            _tableRepo.Update(table);

            SendBasicResponse(EVENT, $"Table {table.Id} has been cleaned and is now ready for the next customers", 0, dto);

            // This one goes to every other main staff service
            _messageProducer.SendMessage($"{EVENT}-HostStaff", dto);
            _messageProducer.SendMessage($"{EVENT}-WaitStaff", dto);
            _messageProducer.SendMessage($"{EVENT}-KitchenStaff", dto);
        }
    }
}
