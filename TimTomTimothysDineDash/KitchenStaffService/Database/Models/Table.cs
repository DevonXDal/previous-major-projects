using BarebonesEFCoreRepositoryPatternStarter.Models.MainDb.NotMapped;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenStaffService.Database.Models
{
    /// <summary>
    /// This Table class represents the database entity related to seating that an order may need to be placed at.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class Table : EntityBase
    {
        public int? OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual Order? Order { get; set; }
    }
}
