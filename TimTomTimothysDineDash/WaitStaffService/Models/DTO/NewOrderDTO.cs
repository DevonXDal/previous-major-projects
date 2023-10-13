using BarebonesRabbitMQImplementationLibrary.Models;

namespace CoreAPIService.Models.DTO.WaitStaff
{
    /// <summary>
    /// This NewOrderDTO class is used to hold the data needed to insert a new order taken by wait staff
    /// into the system.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class NewOrderDTO : DataWithCorrelation
    {
        /// <summary>
        /// The id of the table the order is for
        /// </summary>
        public int TableId { get; set; }

        /// <summary>
        /// The description of what is being ordered by the user.
        /// </summary>
        public string OrderDescription { get; set; }
    }
}
