using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.GenericGameObjects
{
    /// <summary>
    /// A Player is representative of a real-world player that is playing a card game. 
    /// They have their own hand of cards. 
    /// Players also pick up and rearrange the cards that they have. 
    /// When each round is over, they then discard their hand.
    /// </summary>
    /// <typeparam name="T">Represents some type of card for a card/board game</typeparam>
    public class Player<T> where T : Card?
    {
        public IList<T> Hand { get; init; }

        /// <summary>
        /// Create a new player with an empty hand
        /// </summary>
        public Player()
        {
            Hand = new List<T>();
        }

        /// <summary>
        /// Get the number of cards in the player's hand
        /// </summary>
        /// <returns>How many cards are in the player's hand</returns>
        public int Count()
        {
            return Hand.Count;
        }

        /// <summary>
        /// Remove and return a card from a specific index in the hand. Shifts the index of each card after the removed card to the left by one.
        /// Returns null if the index is out of range.
        /// </summary>
        /// <param name="index">The index that the card can be found.</param>
        /// <returns>The removed card</returns>
        public T? RemoveAt(int index)
        {
            if (index < 0 || index >= Hand.Count)
            {
                return null;
            }

            var card = Hand[index];
            Hand.RemoveAt(index);
            return card;
        }

        /// <summary>
        /// Add a card to a specific index in the hand or the end of the hand
        /// </summary>
        /// <param name="index">The position of which to add the card to the hand</param>
        /// <param name="card">The game card to add to the hand</param>
        public void Insert(int index, T card)
        {
            if (index < 0 || index > Hand.Count)
            {
                throw new IndexOutOfRangeException("Cannot place card at that index");
            }
            else
            {
                Hand.Insert(index, card);
            }
        }

        /// <summary>
        /// Empty the hand of all of its cards without returning them.
        /// </summary>
        public void Clear()
        {
            Hand.Clear();
        }
    }
}
