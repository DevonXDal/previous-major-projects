using BarebonesEFCoreRepositoryPatternStarter.Data;
using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Helpers;
using BarebonesRabbitMQImplementationLibrary.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Models;
using BarebonesRabbitMQImplementationLibrary.Models.DTO;
using HostStaffService.Database.Data;
using HostStaffService.Database.Models;
using HostStaffService.Models.DTO;
using Serilog;

namespace HostStaffService.EventHandlers
{
    /// <summary>
    /// This SeatPeopleAtTableEventHandler class handles seating the next group that is in the queue at a table.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class SeatPeopleAtTableEventHandler : EventHandlerBase
    {
        private readonly RepositoryBase<ApplicationDbContext, CustomerQueue> _customerQueueRepo;
        private readonly RepositoryBase<ApplicationDbContext, Table> _tableRepo;
        public const string EVENT = "SeatPeopleAtTable";


        public SeatPeopleAtTableEventHandler(
            RepositoryBase<ApplicationDbContext, CustomerQueue> customerQueueRepo,
            RepositoryBase<ApplicationDbContext, Table> tableRepo,
            IMessageProducer messageProducer)
        {
            _customerQueueRepo = customerQueueRepo;
            _messageProducer = messageProducer;
            _tableRepo = tableRepo;
        }

        /// <summary>
        /// Handles the event and sends a response message indicating a success or failure.
        /// </summary>
        public async Task Handle(SingleStringDTO dto)
        {
            int tableId = 0;
            try
            {
                tableId = int.Parse(dto.SingleStringValue);
            } catch (Exception _)
            {
                SendBasicResponse(EVENT, "The table id was not a number", -1, dto, "The provided id was not a number");
                return;
            }

            Table? table = _tableRepo.GetByID(tableId);
            if (table == null)
            {
                SendBasicResponse(EVENT, $"There is no table {tableId} to seat people at", -2, dto, "There is no table with that id");
                return;
            } else if (!table.IsAvailable)
            {
                SendBasicResponse(EVENT, $"Table {tableId} is currently in use and cannot be used for seating people", -3, dto, "That table is already being used");
                return;
            }

            CustomerQueue? nextParty = _customerQueueRepo.Get(p => !p.HasBeenSeated).OrderBy(p => p.Id).FirstOrDefault();
            if (nextParty == null)
            {
                SendBasicResponse(EVENT, $"Attempt made to seat people at table {tableId} but there is no party to seat", -3, dto, "There is no party in the waiting queue");
                return;
            }

            nextParty.HasBeenSeated = true;
            table.IsAvailable = false;
            
            _customerQueueRepo.Update(nextParty);
            _tableRepo.Update(table);

            SendBasicResponse(EVENT, $"Seated {nextParty.NumToSeat} at table {table.Id}", 0, dto);

            TableHasBeenSeatedDTO updateDTO = new()
            {
                TableId = tableId,
                NumSeated = nextParty.NumToSeat,
                ClientUUID = dto.ClientUUID,
                CorrelationGuid = dto.CorrelationGuid,
                InitialCreation = dto.InitialCreation,
            };
            _messageProducer.SendMessage($"{EVENT}-WaitStaff", updateDTO);
        }
    }
}
