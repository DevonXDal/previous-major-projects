using BarebonesRabbitMQImplementationLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaitStaffService.Models.DTO
{
    /// <summary>
    /// This TableHasBeenSeatedDTO class is sent to indicate what table was seated and how many were seated.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class TableHasBeenSeatedDTO : DataWithCorrelation
    {
        public int TableId { get; set; }

        public int NumSeated { get; set; }
    }
}
