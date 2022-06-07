using EarnShardsForCards.Shared.Data.GenericGameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.Interfaces
{
    /// <summary>
    /// This IMeld represents melds (sets/runs) from the game, Gin Rummy. 
    /// It provides the common functionality for the melds and allows them to be stored together in a collection.
    /// Implementing classes should make use of a private constructor.
    /// </summary>
    public interface IMeld
    {
        public IList<PlayingCard> Cards { get; init; }

        /// <summary>
        /// Create a meld set from three or four cards that have the same rank
        /// </summary>
        /// <param name="cards">The cards used to create the meld</param>
        /// <param name="isAroundTheWorld">Whether the meld is for the around the world variation</param>
        /// <returns>Either the meld that was created, or if the meld cannot be formed by the cards, null</returns>
        public static abstract IMeld? GenerateMeldFromCards(IList<PlayingCard> cards, bool isAroundTheWorld = false);

        /// <summary>
        /// Verifies that a new card is able to become part of the meld.
        /// </summary>
        /// <param name="newCard">The new card to add to the meld</param>
        /// <returns></returns>
        public bool CanAddNewCard(PlayingCard newCard);

        /// <summary>
        /// Adds a card to the meld without verification that the card can be added.
        /// </summary>
        /// <param name="card">The card to be added</param>
        public IMeld Insert(PlayingCard card);

        /// <summary>
        /// Determines the deadwood that is removed from this meld being formed.
        /// </summary>
        /// <returns>The deadwood removed by the meld</returns>
        public int DeadwoodRemovedByMeld();
    }
}
