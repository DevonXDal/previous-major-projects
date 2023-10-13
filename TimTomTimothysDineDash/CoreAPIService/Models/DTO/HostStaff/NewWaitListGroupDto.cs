using BarebonesRabbitMQImplementationLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAPIService.Models.DTO.HostStaff
{
    /// <summary>
    /// This NewWaitListGroupDto class represents a new group that is getting into the waiting list queue.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class NewWaitListGroupDto : DataWithCorrelation
    {
        /// <summary>
        /// How many people need to be seated in the new group?
        /// </summary>
        public int NumberNeedingSeated { get; set; }

        public NewWaitListGroupDto() : base() { }
    }
}
