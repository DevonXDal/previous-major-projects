using BarebonesEFCoreRepositoryPatternStarter.Models.MainDb.NotMapped;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusStaffService.Database.Models
{
    /// <summary>
    /// This represents a table that may or may not need to be cleaned by bus staff.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class Table : EntityBase
    {
        public bool IsNeedingCleaned { get; set; } // Whether the table needs to be cleaned

        public bool HasPaid { get; set; } // Whether the current customers have paid or not
    }
}
