using BarebonesEFCoreRepositoryPatternStarter.Models.MainDb.NotMapped;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostStaffService.Database.Models
{
    /// <summary>
    /// This Table class represents the database entity that can seat people. 
    /// The only thing that matters is whether there are currently people there or not.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class Table : EntityBase
    {
        public bool IsAvailable { get; set; } // Whether this table can seat the next set of customers
    }
}
