using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.Interfaces
{
    /// <summary>
    /// Represents a subscriber object to be used with a notifier to receive updates.
    /// </summary>
    public interface ISubscribe
    {
        /// <summary>
        /// Handle notification from notifier object
        /// </summary>
        public void Notify();
    }
}
