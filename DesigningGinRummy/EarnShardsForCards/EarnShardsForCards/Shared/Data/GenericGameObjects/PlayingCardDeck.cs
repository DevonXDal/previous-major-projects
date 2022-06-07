using EarnShardsForCards.Shared.Data.Enumerations;
using EarnShardsForCards.Shared.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.GenericGameObjects
{
    /// <summary>
    /// A Playing Card Deck represents a real-world deck of cards. It is also used for the draw pile for real-world games. 
    /// It is created usually with 52 playing cards. Cards can be drawn from it and return as requested. 
    /// It also randomizes the position of the cards it holds when requested.
    /// </summary>
    public class PlayingCardDeck<T> : Deck<T> where T : PlayingCard
    {
        /// <summary>
        /// Creates a new empty deck of playing cards.
        /// </summary>
        public PlayingCardDeck() : base()
        {
            
        }

        /// <summary>
        /// Creates a new deck with the given playing cards. The playing cards will be copied from one deck to another. The actual list itself will not be copied.
        /// </summary>
        /// <param name="cards">The cards to carry over to the new deck</param>
        public PlayingCardDeck(IList<T> cards) : base(cards)
        {
            
        }      
    }
}
