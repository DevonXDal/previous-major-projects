using EarnShardsForCards.Shared.Data.Enumerations;
using EarnShardsForCards.Shared.Data.GenericGameObjects;
using EarnShardsForCards.Shared.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EarnShardsForCards.Shared.Helpers.GeneralAssistanceMethods;

namespace EarnShardsForCards.Shared.Data.GinRummy
{
    /// <summary>
    /// A MeldRun is more of a virtual game support object than a real-world object. 
    /// It represents the three or more cards that a player lays down at the end of each round 
    /// that form a sequence of cards of the same suit that are one rank higher than each other. 
    /// An example of a MeldRun would be the ranks forming 
    /// (‘Ace of Hearts’, ‘Two of Hearts’, ‘Three of Hearts’, ‘Four of Hearts’).
    /// </summary>
    public class MeldRun : IMeld
    {
        private bool _isAroundTheWorld;
        
        public IList<PlayingCard> Cards { get; init; }

        // Creates the meld using the cards provided.
        private MeldRun(IList<PlayingCard> cards, bool isAroundTheWorld)
        {
            Cards = cards;
            _isAroundTheWorld = isAroundTheWorld;
        }

        /// <summary>
        /// Verifies that a new card can be added to the meld run
        /// </summary>
        /// <param name="newCard">The new card to be added to the set</param>
        /// <returns>Whether the new card can be added</returns>
        public bool CanAddNewCard(PlayingCard newCard)
        {
            // If the new card is null or a different suit, then it cannot be added to the meld.
            if (newCard == null || newCard.Suit != Cards[0].Suit)
            {
                return false;
            }

            // If the new card is one rank less than the first card, or one rank higher than the last card, then it can be added to the meld.
            return (newCard.Rank == Cards[0].Rank - 1 || newCard.Rank == Cards[Cards.Count - 1].Rank + 1  // New card is one rank higher than the last or one rank less than the first
                || (newCard.Rank == Rank.Ace && Cards[Cards.Count - 1].Rank == Rank.King));
        }

        /// <summary>
        /// Determines the deadwood that is removed from this run being formed.
        /// </summary>
        /// <returns>The deadwood removed by the set</returns>
        public int DeadwoodRemovedByMeld()
        {
            return Cards.Sum(c => c.Value);

        }

        /// <summary>
        /// Create a meld run from cards of increasing rank
        /// </summary>
        /// <param name="cards">The cards used to create the meld</param>
        /// <returns>Either the meld that was created, or if the meld cannot be formed by the cards, null</returns>
        public static IMeld? GenerateMeldFromCards(IList<PlayingCard> cards, bool isAroundTheWorld = false)
        {
            // if cards is null or empty, return null
            if (cards == null || cards.Count == 0)
            {
                return null;
            }

            // If any of the cards do not have the same suit, return null
            if (cards.Any(c => c.Suit != cards.First().Suit))
            {
                return null;
            }

            var orderedCards = OrderCardsForARun(isAroundTheWorld, cards); // Sort for final determinations
                

            // If any of the cards have a rank that is not one higher than the card before it, return null
            foreach (var current in orderedCards)
            {
                if (orderedCards.IndexOf(current) == 0)
                {
                    continue;
                }
                
                PlayingCard previous = orderedCards.ElementAt(orderedCards.IndexOf(current) - 1);

                if (current.Rank != previous.Rank + 1 && previous.Rank != Rank.King)
                {
                    return null;
                }
            }

            // Create the meld
            return new MeldRun(orderedCards, isAroundTheWorld);

        }

        /// <summary>
        /// Adds a card to the run without verification that the card can be added.
        /// This returns a new run with the card added.
        /// </summary>
        /// <param name="card">The card to be added</param>
        /// <returns>The new run with the card added</returns>
        public IMeld Insert(PlayingCard card)
        {
            return new MeldRun(Cards.Concat(new List<PlayingCard> { card }).OrderBy(c => c.Rank).ToList(), _isAroundTheWorld);
        }

    }
}
