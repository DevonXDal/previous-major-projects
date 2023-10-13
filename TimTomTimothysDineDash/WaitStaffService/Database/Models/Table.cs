using BarebonesEFCoreRepositoryPatternStarter.Models.MainDb.NotMapped;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaitStaffService.Database.Models
{
    public class Table : EntityBase
    {
        public int SeatsUsed { get; set; }

        public int? OrderId { get; set; }

        [ForeignKey(nameof (OrderId))]
        public virtual Order? Order { get; set; }
    }
}
