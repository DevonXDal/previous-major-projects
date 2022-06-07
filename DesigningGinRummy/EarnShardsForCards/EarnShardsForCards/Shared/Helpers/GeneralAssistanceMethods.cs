using EarnShardsForCards.Shared.Data.Enumerations;
using EarnShardsForCards.Shared.Data.GenericGameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Helpers
{
    /// <summary>
    /// This class provides methods that help other classes performs certain operations that are performed by both. 
    /// The operations provided here are not specific to any class. This GeneralAssistanceMethods class exists to promote code reuse.
    /// </summary>
    public static class GeneralAssistanceMethods
    {
        /// <summary>
        /// Handles Around the World variation checks for a run and orders the run based on that information, returning the cards.
        /// Expects only one suit of cards to be provided. Break the cards into groups of 4, and then order the groups based on the suit. Then, provide the cards.
        /// </summary>
        /// <param name="isAroundTheWorld">Is the around the world variation being played</param>
        /// <param name="cards">The cards to reorder</param>
        /// <returns>A new list reordered for meld run calculations</returns>
        public static IList<PlayingCard> OrderCardsForARun(bool isAroundTheWorld, IList<PlayingCard> cards)
        {
            // If not around the world, there is no kings to worry about, or there are 13 cards.
            if (!isAroundTheWorld || !cards.Any(c => c.Rank == Rank.King) || cards.Count == 13)
            {
                return cards.OrderBy(c => c.Suit).ToList();
            }
            else
            {
                var startingOrder = cards.OrderBy(c => c.Rank).ToList(); // Get them ordered by rank
                IList<PlayingCard> toReturn = new List<PlayingCard>(); // Empty list that will be returned with the correct order
                int indexOfFinalPositionFromAceOnwardsRun = 0; // The index (possibly 0) that a run formation ended from the ace.
                Rank previousRank = Rank.Ace; // What the previous index was

                for (int i = 1; i < cards.Count; i++) // For each card after the known Ace, check that the card is one rank higher, until one is not
                {
                    var currentRank = cards.ElementAt(i).Rank;

                    if (currentRank != previousRank + 1)
                    {
                        break;
                    }
                    else
                    {
                        indexOfFinalPositionFromAceOnwardsRun++;
                        previousRank = currentRank;
                    }

                }

                int indexAfterAceRun = indexOfFinalPositionFromAceOnwardsRun + 1;
                while (indexAfterAceRun + 1 != cards.Count) // After the Ace run, build the run leading up to the King
                {
                    toReturn.Add(cards.ElementAt(indexAfterAceRun));
                    cards.RemoveAt(indexAfterAceRun);
                }

                foreach (PlayingCard card in cards) // Add in the remaining cards to the end of the list.
                {
                    toReturn.Add(card);
                }

                return toReturn;
            }
        }
    }
}
