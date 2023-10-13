using BarebonesEFCoreRepositoryPatternStarter.Models.MainDb.NotMapped;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenStaffService.Database.Models
{
    /// <summary>
    /// This Order class represents the database entity relating to the list of items, quantity, etc., describing what a table of customers
    /// wants to be made.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class Order : EntityBase
    {
        public string OrderGuid { get; set; }

        public string Description { get; set; } // What the order requests for

        public bool IsStarted { get; set; } // Whether the kitchen has at least started working on this order

        public bool IsFinished { get; set; }

    }
}
