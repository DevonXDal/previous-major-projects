using BarebonesEFCoreRepositoryPatternStarter.Data;
using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Helpers;
using BarebonesRabbitMQImplementationLibrary.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Models;
using WaitStaffService.Database.Data;
using WaitStaffService.Database.Models;
using WaitStaffService.Models.DTO.StatusUpdate;
using Serilog;
using CoreAPIService.Models.DTO.WaitStaff;
using WaitStaffService.Models.DTO;
using BarebonesRabbitMQImplementationLibrary.Models.DTO;

namespace WaitStaffService.EventHandlers
{
    /// <summary>
    /// This TableHasPaidForOrderAndLeftEventHandler class handles marking a table as having paid and left.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class TableHasPaidForOrderAndLeftEventHandler : EventHandlerBase
    {
        private readonly RepositoryBase<ApplicationDbContext, Table> _tableRepo;
        private readonly RepositoryBase<ApplicationDbContext, Order> _orderRepo;
        public const string EVENT = "TableHasPaidForOrderAndLeft";


        public TableHasPaidForOrderAndLeftEventHandler(
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
        public async Task Handle(SingleStringDTO dto)
        {
            Table table;
            try
            {
                table = _tableRepo.GetByID(int.Parse(dto.SingleStringValue));
            } catch (Exception _)
            {
                SendBasicResponse(EVENT,
                    $"Attempted to mark a table as paid and having left but the id is not an integer as expected",
                    -1, dto,
                    $"The id was not an integer");
                return;
            }

            if (table == null)
            {
                SendBasicResponse(EVENT,
                    $"Attempted to mark a table as paid and having left but there is no table with that id",
                    -2, dto,
                    $"THe table does not exist");
                return;
            }  else if (table.SeatsUsed == 0)
            {
                SendBasicResponse(EVENT,
                    $"Attempted to mark table {table.Id} as paid and having left but there is no one sitting there",
                    -3, dto,
                    $"Cannot mark a table as paid and left, when it has no used seats");
                return;
            }

           table.SeatsUsed = 0;
            _tableRepo.Update(table);

            SendBasicResponse(EVENT, $"Marked table {table.Id} as having paid and left, successfully", 0, dto);
            _messageProducer.SendMessage($"{EVENT}-BusStaff", dto);
        }
    }
}
