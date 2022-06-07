using EarnShardsForCards.Shared.Data.GenericGameObjects;
using EarnShardsForCards.Shared.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.GinRummy
{
    /// <summary>
    /// A MeldSet is more of a virtual game support object than a real-world object. 
    /// It represents the three or four cards that a player lays down at the end of a round 
    /// that form a three-of-a-kind or four-of-a-kind such that there are three or four cards 
    /// with the same rank but differing suits. Its primary purpose is to assist the scoring 
    /// at the end of a round.
    /// </summary>
    public class MeldSet : IMeld
    {
        public IList<PlayingCard> Cards { get; init; }

        // Creates the meld using the cards provided.
        private MeldSet(IList<PlayingCard> cards)
        {
            Cards = cards;
        }

        /// <summary>
        /// Verifies that a new card can be added to the meld set
        /// </summary>
        /// <param name="newCard">The new card to be added to the set</param>
        /// <returns>Whether the new card can be added</returns>
        public bool CanAddNewCard(PlayingCard newCard)
        {
            // If the meld has three cards and the new card is the same rank as the first card it can be added
            if (Cards.Count == 3 && Cards[0].Rank == newCard.Rank)
            {
                return true;
            } else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines the deadwood that is removed from this set being formed.
        /// </summary>
        /// <returns>The deadwood removed by the set</returns>
        public int DeadwoodRemovedByMeld()
        {
            return Cards.Sum(c => c.Value);
        }

        /// <summary>
        /// Create a meld set from three or four cards that have the same rank
        /// </summary>
        /// <param name="cards">The cards used to create the meld</param>
        /// <returns>Either the meld that was created, or if the meld cannot be formed by the cards, null</returns>
        public static IMeld? GenerateMeldFromCards(IList<PlayingCard> cards, bool isAroundTheWorld = false)
        {
            // If there are not three or four cards, the meld cannot be formed
            if (cards.Count != 3 && cards.Count != 4)
            {
                return null;
            }

            // If the cards are not all the same rank, the meld cannot be formed
            if (cards.Any(c => c.Rank != cards[0].Rank))
            {
                return null;
            }

            // The set can be formed
            return new MeldSet(cards);
        }

        /// <summary>
        /// Adds a card to the meld without verification that the card can be added.
        /// This returns as a new meld set. The current one is not modified.
        /// </summary>
        /// <param name="card">The card to be added</param>
        /// <returns>The new meld set</returns>
        public IMeld Insert(PlayingCard card)
        {
            // Create a new list of cards
            List<PlayingCard> newCards = new List<PlayingCard>(Cards);
            newCards.Add(card);

            // Create the new meld set
            return new MeldSet(newCards);
        }
    }
}
