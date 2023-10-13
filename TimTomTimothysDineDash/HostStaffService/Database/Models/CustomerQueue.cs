using BarebonesEFCoreRepositoryPatternStarter.Models.MainDb.NotMapped;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostStaffService.Database.Models
{
    /// <summary>
    /// This CustomerQueue class represents the database entity that is used to track incoming customers. 
    /// This needs to know whether they have been seated, and how many 'they' refers to.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class CustomerQueue : EntityBase
    {
        public int NumToSeat { get; set; } // How many customers are to be seated?

        public bool HasBeenSeated { get; set; } // Has this set of customers been seated?
    }
}
