using EarnShardsForCards.Shared.Data.GenericGameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.GenericGameObjects
{
    /// <summary>
    /// Interface for a game deck that identifies the common functions of each type of deck.
    /// </summary>
    public abstract class Deck<T> where T : Card
    {
        protected List<T> Cards { get; set; }

        /// <summary>
        /// Creates a new empty deck
        /// </summary>
        public Deck()
        {
            Cards = new List<T>();
        }

        /// <summary>
        /// Creates a new deck with the given cards. The cards will be copied from one deck to another. The actual list itself will not be copied.
        /// </summary>
        /// <param name="cards">The cards to carry over to the new deck</param>
        public Deck(IList<T> cards)
        {
            Cards = new List<T>();
            foreach (T card in cards)
            {
                Cards.Add(card);
            }
        }

        /// <summary>
        /// Returns the number of cards that are currently in the deck.
        /// </summary>
        /// <returns>The number of cards in the deck</returns>
        public int Count()
        {
            return Cards.Count;
        }

        /// <summary>
        /// Remove and return a card from the top of the deck.
        /// </summary>
        /// <returns>The card from the top of the deck</returns>
        public T Draw()
        {
            T card = Cards.Last();
            Cards.Remove(card);
            return card;
        }

        /// <summary>
        /// Randomizes the positions of cards in the deck.
        /// </summary>
        public void Shuffle()
        {
            Random rng = new Random();
            int n = Cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = Cards[k];
                Cards[k] = Cards[n];
                Cards[n] = value;
            }
        }
    }
}
