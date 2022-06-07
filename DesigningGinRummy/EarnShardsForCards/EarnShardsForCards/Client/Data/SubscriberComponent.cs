using EarnShardsForCards.Shared.Data.Interfaces;
using Microsoft.AspNetCore.Components;

namespace EarnShardsForCards.Client.Data
{
    public abstract class SubscriberComponent : ComponentBase, ISubscribe
    {
        /// <summary>
        /// React to a notification that the state has changed.
        /// </summary>
        public abstract void Notify();
    }
}
