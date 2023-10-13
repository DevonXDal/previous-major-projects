using BarebonesRabbitMQImplementationLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaitStaffService.Models.DTO
{
    /// <summary>
    /// This NewOrderForKitchenDTO class holds information needed to pass an order along to the kitchen and keep it in sync
    /// with the information on the order that the wait staff has.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class NewOrderForKitchenDTO : DataWithCorrelation
    {
        /// <summary>
        /// The id of the table the order is for
        /// </summary>
        public int TableId { get; set; }

        /// <summary>
        /// The description of what is being ordered by the user.
        /// </summary>
        public string OrderDescription { get; set; }

        /// <summary>
        /// The GUID identifier of the new order
        /// </summary>
        public string OrderGuid { get; set; }
    }
}
