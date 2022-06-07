using EarnShardsForCards.Shared.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.GenericGameObjects
{
    /// <summary>
    /// A notifier is a virtual game support object whose purpose is to notify its changing 
    /// list of subscribers to a change in game state. 
    /// It is mostly used to notify view components to state changes but can be used for other uses. 
    /// It does not specify any action that must be taken.
    /// </summary>
    public class Notifier
    {
        private ICollection<ISubscribe> _subscribers;

        /// <summary>
        /// Create a notifier that has no subscribers.
        /// </summary>
        public Notifier()
        {
            _subscribers = new List<ISubscribe>();
        }

        /// <summary>
        /// Adds a subscribe to the list of subscribers to notify.
        /// </summary>
        /// <param name="subscriber">The subscriber to add</param>
        public void Subscribe(ISubscribe subscriber)
        {
            _subscribers.Add(subscriber);
        }

        /// <summary>
        /// Removes a subscriber from the list of subscribers to notify.
        /// </summary>
        /// <param name="subscriber">The subscriber to remove</param>
        public void Unsubscribe(ISubscribe subscriber)
        {
            _subscribers.Remove(subscriber);
        }

        /// <summary>
        /// Notifies all subscribers that an update or event has occured.
        /// </summary>
        public void SendNotice()
        {
            foreach (ISubscribe subscriber in _subscribers)
            {
                subscriber.Notify();
            }
        }
    }
}
