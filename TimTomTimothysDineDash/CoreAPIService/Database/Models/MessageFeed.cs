using BarebonesEFCoreRepositoryPatternStarter.Models.MainDb.NotMapped;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAPIService.Database.Models
{
    /// <summary>
    /// This MessageFeed class tracks the recent actions taken on the system.
    /// It is read to provide updates to all users of the system.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class MessageFeed : EntityBase
    {
        /// <summary>
        /// The service that this message in the feed was stored because of.
        /// </summary>
        public string ServiceRelation { get; set; }

        /// <summary>
        /// The description of what action happened recently.
        /// </summary>
        public string Description { get; set; }
    }
}
