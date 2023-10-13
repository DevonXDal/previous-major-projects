using BarebonesEFCoreRepositoryPatternStarter.Models.MainDb.NotMapped;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaitStaffService.Database.Models
{
    public class Order : EntityBase
    {
        public string OrderGuid { get; set; }

        public string Description { get; set; }

        public bool IsFinished { get; set; }
    }
}
