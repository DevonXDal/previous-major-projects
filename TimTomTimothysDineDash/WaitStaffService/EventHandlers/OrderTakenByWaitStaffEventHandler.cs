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

namespace WaitStaffService.EventHandlers
{
    /// <summary>
    /// This OrderTakenByWaitStaffEventHandler class handles storing a new order for a table.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class OrderTakenByWaitStaffEventHandler : EventHandlerBase
    {
        private readonly RepositoryBase<ApplicationDbContext, Table> _tableRepo;
        private readonly RepositoryBase<ApplicationDbContext, Order> _orderRepo;
        public const string EVENT = "OrderTakenByWaitStaff";


        public OrderTakenByWaitStaffEventHandler(
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
        public async Task Handle(NewOrderDTO dto)
        {
            var table = _tableRepo.GetByID(dto.TableId);
            if (table == null)
            {
                SendBasicResponse(EVENT,
                    $"Attempted to add an order for table {dto.TableId} but there is no table with that Id",
                    -1, dto,
                    $"Table {dto.TableId} does not exist");
                return;
            } else if (dto.OrderDescription.Length < 5)
            {
                SendBasicResponse(EVENT,
                    $"Attempted to add an order for table {dto.TableId} but the order was way too short",
                    -2, dto,
                    $"Orders must be at least five characters");
                return;
            } else if (table.OrderId != null)
            {
                SendBasicResponse(EVENT,
                    $"Attempted to add an order for table {dto.TableId} but there is already an order",
                    -3, dto,
                    $"Cannot add an order for a table that has already ordered");
                return;
            } else if (table.SeatsUsed == 0)
            {
                SendBasicResponse(EVENT,
                    $"Attempted to add an order for table {dto.TableId} but there is no one sitting there",
                    -4, dto,
                    $"Cannot add an order for a table that has no used seats");
                return;
            }

            Order order = new()
            {
                Description = dto.OrderDescription,
                IsFinished = false,
                OrderGuid = Guid.NewGuid().ToString(),
            };

            _orderRepo.Insert(order);

            table.OrderId = order.Id;
            _tableRepo.Update(table);

            SendBasicResponse(EVENT, $"Added order {order.OrderGuid} for table {table.Id} successfully", 0, dto);
            _messageProducer.SendMessage($"{EVENT}-KitchenStaff", new NewOrderForKitchenDTO()
            {
                ClientUUID = dto.ClientUUID,
                CorrelationGuid = dto.CorrelationGuid,
                InitialCreation = dto.InitialCreation,
                OrderDescription = dto.OrderDescription,
                OrderGuid = order.OrderGuid,
                TableId = table.Id,
            });
        }
    }
}
