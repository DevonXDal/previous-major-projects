using EarnShardsForCards.Shared.Data.Enumerations;
using EarnShardsForCards.Shared.Data.GenericGameObjects;
using EarnShardsForCards.Shared.Data.Interfaces;
using EarnShardsForCards.Shared.Models.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EarnShardsForCards.Shared.Helpers.GeneralAssistanceMethods;

namespace EarnShardsForCards.Shared.Data.GinRummy
{
    /// <summary>
    /// A GinRummyScoreHandler is a virtual game support object that separates the 
    /// handling of deadwood, knocking, Gin, and similar aspects from the main controller class.
    /// </summary>
    public class GinRummyScoreHandler
    {
        private Player<PlayingCard> _humanPlayer;
        private GinRummyComputerPlayer<PlayingCard> _computerPlayer;
        private IGinRummyController _controller;
        private bool _isAroundTheWorld;

        /// <summary>
        /// Create a GinRummyScoreHandler with a reference to the controller, human player, and computer player.
        /// </summary>
        /// <param name="controller">The game controller</param>
        /// <param name="player">The user's player</param>
        /// <param name="computerPlayer">The computer player</param>
        /// <param name="isAroundTheWorld">Whether or not the game is the around the world variation</param>
        public GinRummyScoreHandler(
            IGinRummyController controller, 
            Player<PlayingCard> player, 
            GinRummyComputerPlayer<PlayingCard> computerPlayer,
            bool isAroundTheWorld = false)
        {
            _controller = controller;
            _humanPlayer = player;
            _computerPlayer = computerPlayer;
            _isAroundTheWorld = isAroundTheWorld;
        }

        /// <summary>
        /// Calculates the number of points at the end of around. Once points are calculated
        /// it handles notifying of the score and other information to the user.
        /// </summary>
        /// <param name="isBigGin">Whether the round ended by Big Gin</param>
        public void RewardPoints(bool isBigGin, Player<PlayingCard> roundEndingPlayer)
        {
            EliminateDeadwoodData deadwoodDataForREPlayer; // Round ending player data
            EliminateDeadwoodData deadwoodDataForNREPlayer; // Non-round ending player data
            Player<PlayingCard> nonRoundEndingPlayer = (roundEndingPlayer == _humanPlayer) ? _computerPlayer : _humanPlayer;         

            bool isGinOfAnyForm = isBigGin;

            if (isBigGin)
            {
                deadwoodDataForNREPlayer = EliminateDeadwood(nonRoundEndingPlayer, null, null);
                deadwoodDataForREPlayer = new EliminateDeadwoodData(0, new List<MeldSet>(), new List<MeldRun>()); // Code thinks it is not assigned when handling Gin so an object removes the compilation error
            } else
            {
                deadwoodDataForREPlayer = EliminateDeadwood(roundEndingPlayer, null, null);

                if (deadwoodDataForREPlayer.RemainingDeadwood == 0)
                {
                    isGinOfAnyForm = true;
                    deadwoodDataForNREPlayer = EliminateDeadwood(nonRoundEndingPlayer, null, null);
                } else
                {
                    deadwoodDataForNREPlayer = EliminateDeadwood(
                        nonRoundEndingPlayer, 
                        deadwoodDataForREPlayer.UsedSets, 
                        deadwoodDataForREPlayer.UsedRuns);
                }
            }

            // Check if there was gin of any kind otherwise it was a knock
            if (isGinOfAnyForm)
            {
                // Check if the round ended by Big Gin
                if (isBigGin)
                {
                    DocumentResults(roundEndingPlayer, 31 + deadwoodDataForNREPlayer.RemainingDeadwood, GinRummyRoundEndingCase.BigGin); // Big Gin Bonus! 31!
                }
                else
                {
                    DocumentResults(roundEndingPlayer, 25 + deadwoodDataForNREPlayer.RemainingDeadwood - deadwoodDataForREPlayer.RemainingDeadwood, GinRummyRoundEndingCase.Gin); // Gin Bonus! 25!
                }
            }
            else
            {
                if (deadwoodDataForNREPlayer.RemainingDeadwood <= deadwoodDataForREPlayer.RemainingDeadwood) // Undercut Bonus! 25! But for the non-round ending player
                {
                    DocumentResults(nonRoundEndingPlayer, 25 + deadwoodDataForREPlayer.RemainingDeadwood - deadwoodDataForNREPlayer.RemainingDeadwood, GinRummyRoundEndingCase.Undercut); 
                } else // Normal knock occured
                {
                    DocumentResults(roundEndingPlayer, deadwoodDataForNREPlayer.RemainingDeadwood - deadwoodDataForREPlayer.RemainingDeadwood, GinRummyRoundEndingCase.Knock);
                }
            }

        }

        /// <summary>
        /// Documents the round results by notifying the controller.
        /// </summary>
        /// <param name="winner">A reference to whoever won the round</param>
        /// <param name="points">The points to award that player</param>
        /// <param name="reason">The reason the round ended</param>
        /// <param name="laidOffDeadwoodAmount">The amount of deadwood that was laid off by the non-knocking player if any</param>
        private void DocumentResults(Player<PlayingCard> winner, int points, GinRummyRoundEndingCase reason, int laidOffDeadwoodAmount = 0)
        {
            _controller.DocumentRoundResults(winner, points, reason, laidOffDeadwoodAmount);
        }

        /// <summary>
        /// Determine the largest amount of deadwood that can be eliminated for a player.
        /// Also returns the melds created in order allow laying off deadwood if needed.
        /// </summary>
        /// <param name="player">The player to view the hand of</param>
        /// <param name="setsFromOpponent">The sets the opponent eliminated deadwood using</param>
        /// <param name="runsFromOpponents">The runs the opponent eliminated deadwood using</param>
        /// <returns>Data related to removed deadwood</returns>
        public EliminateDeadwoodData EliminateDeadwood(
            Player<PlayingCard> player, 
            IList<MeldSet>? setsFromOpponent, 
            IList<MeldRun>? runsFromOpponents)
        {
            IList<MeldSet> setsRemovingDeadwood = setsFromOpponent ?? new List<MeldSet>();
            IList<MeldRun> runsRemovingDeadwood = runsFromOpponents ?? new List<MeldRun>();

            // Clone the player's hand
            IList<PlayingCard> hand = player.Hand.ToList();

            FindMeldSets(hand, setsRemovingDeadwood); // Adds sets formed by the hand to the list of sets to remove deadwood from
            FindMeldRuns(hand, runsRemovingDeadwood); // Adds runs formed by the hand to the list of runs to remove deadwood from

            List<List<IMeld>> meldCombinations = new();

            foreach (MeldSet set in setsRemovingDeadwood) // Add the initial sets as single meld combinations
            {
                var meldCombination = new List<IMeld>();
                meldCombination.Add(set);
                meldCombinations.Add(meldCombination);
            }

            foreach (MeldRun run in runsRemovingDeadwood) // Add the initial runs as single meld combinations
            {
                var meldCombination = new List<IMeld>();
                meldCombination.Add(run);
                meldCombinations.Add(meldCombination);
            }
            
            
            //meldCombinations.Add(setsRemovingDeadwood);
            //meldCombinations.Add(runsRemovingDeadwood);

            foreach (List<IMeld> meldCombination in meldCombinations.ToList()) // For each meld combination
            {
                var deadwood = GetRemainingDeadwoodCards(hand, meldCombination); // Get the deadwood cards that can be eliminated
                RecursivelyIdentifyMeldCombinations(deadwood, meldCombinations, meldCombination); // Recursively identify all possible meld combinations
            }

            int bestMeldComboForTheJobIndex = -1; // Which meld combination will remove the most deadwood
            int remainingDeadwoodAmount = int.MaxValue; // The amount of deadwood that will be eliminated

            for (int i = 0; i < meldCombinations.Count; i++) // For each meld combination
            {
                int deadwoodAmount = GetRemainingDeadwoodCards(hand, meldCombinations[i]).Sum(c => c.Value); // Get the amount of deadwood that is left over
                if (deadwoodAmount < remainingDeadwoodAmount) // If the amount of deadwood is less than the current best
                {
                    remainingDeadwoodAmount = deadwoodAmount; // Set the new best amount of deadwood
                    bestMeldComboForTheJobIndex = i; // Set the index of the best meld combination
                }
            }

            List<MeldRun> runsUsed = new();
            List<MeldSet> setsUsed = new();

            if (bestMeldComboForTheJobIndex != -1) // If there was a best meld combination
            {
                foreach (IMeld meld in meldCombinations[bestMeldComboForTheJobIndex]) // For each meld in the best meld combination
                {
                    if (meld is MeldRun) // If the meld is a run
                    {
                        runsUsed.Add(meld as MeldRun); // Add the run to the list of runs used
                    }
                    else // If the meld is a set
                    {
                        setsUsed.Add(meld as MeldSet); // Add the set to the list of sets used
                    }
                }
            } else
            {
                // No meld combination was found
                // This means that the player has every card as deadwood
                // So we will just grab remaining deadwood ourselves
                remainingDeadwoodAmount = hand.Sum(c => c.Value);
            }

            return new EliminateDeadwoodData(remainingDeadwoodAmount, setsUsed, runsUsed); // Return the data around deadwood elimination
        }

        /// <summary>
        /// Determine if the provided player can knock given the cards in their hand.
        /// Remove the card to knock before providing, temporarily, so that accurate information is generated.
        /// </summary>
        /// <param name="player">The player to see if knocking is allowed</param>
        /// <returns>True if the player can knock (10 or less deadwood)</returns>
        public bool CanPlayerKnock(Player<PlayingCard> player)
        {
            return (EliminateDeadwood(player, null, null).RemainingDeadwood <= 10); // Player must have 10 or less deadwood to knock
        }

        /// <summary>
        /// Determine if the provided player has gone Big Gin this turn.
        /// </summary>
        /// <param name="player">The player to see if they have Big Gin</param>
        /// <returns>True if the player has Big Gin</returns>
        public bool DoesPlayerHaveBigGin(Player<PlayingCard> player)
        {
            return (EliminateDeadwood(player, null, null).RemainingDeadwood == 0);
        }

        // Find all the meld sets that can be created from the provided cards
        private void FindMeldSets(IList<PlayingCard> handCards, IList<MeldSet> currentSets)
        {
            // Cards for the hand sorted by rank and then suit
            List<PlayingCard> sortedHandForSetDiscoveries = handCards.OrderBy(c => c.Rank).ThenBy(c => c.Suit).ToList();

            // Index of first card with that rank
            int firstCardWithRank = 0;

            // Index of last card with that rank
            int lastCardWithRank = -1;

            // Previous card's rank
            Rank previousRank = sortedHandForSetDiscoveries[0].Rank;

            for (int i = 1; i < sortedHandForSetDiscoveries.Count; i++) 
            {
                PlayingCard card = sortedHandForSetDiscoveries[i];

                if (i == handCards.Count - 1)
                {
                    // If the current card is the last card in the hand,
                    // then the current card is the last card with that rank
                    if (sortedHandForSetDiscoveries[i].Rank == previousRank)
                    {
                        lastCardWithRank = i;
                    } else
                    {
                        lastCardWithRank = i - 1;
                    }
                    
                } else if (card.Rank != previousRank)
                {
                    // If the current card's rank is different than the previous card's rank,
                    // then the previous card is the last card with that rank
                    lastCardWithRank = i - 1;
                }

                if ((i == sortedHandForSetDiscoveries.Count - 1 || card.Rank != previousRank && lastCardWithRank != -1) && lastCardWithRank - firstCardWithRank >= 2)
                {
                    // If the current card's rank is not the same as the previous card's rank,
                    // and the last card with that rank has been found,
                    // then create a set from the cards with that rank
                    MeldSet set = (MeldSet) MeldSet.GenerateMeldFromCards((sortedHandForSetDiscoveries.GetRange(firstCardWithRank, 3)));

                    if (set != null)
                    {
                        currentSets.Add(set);

                        if (lastCardWithRank != sortedHandForSetDiscoveries.Count - 1 && set.CanAddNewCard(sortedHandForSetDiscoveries[lastCardWithRank + 1]))
                        {
                            // If the set can be extended with the next card,
                            // then extend the set with the next card
                            currentSets.Add((MeldSet)set.Insert(sortedHandForSetDiscoveries[lastCardWithRank]));
                        }
                    }

                    // Reset the first and last card with that rank
                    firstCardWithRank = i;
                    lastCardWithRank = -1;
                } else if (card.Rank != previousRank)
                {
                    // If the previous card's rank was not the same as the current card's rank,
                    // then the current card is the first card with that rank
                    firstCardWithRank = i;
                }

                previousRank = card.Rank;
            }


        }

        // Find meld runs that can be created from the provided cards
        private void FindMeldRuns(IList<PlayingCard> handCards, IList<MeldRun> currentRuns)
        {
            // Cards for the hand sorted by suit and then rank
            List<PlayingCard> sortedHandForRunDiscoveries = handCards.OrderBy(c => c.Suit).ToList();

            var inbetweenList = new List<PlayingCard>();

            foreach (PlayingCard card in sortedHandForRunDiscoveries.Where(c => c.Suit == Suit.Clubs).OrderBy(c => c.Rank).ToList())
            {
                inbetweenList.Add(card);
            }

            foreach (PlayingCard card in sortedHandForRunDiscoveries.Where(c => c.Suit == Suit.Diamonds).OrderBy(c => c.Rank).ToList())
            {
                inbetweenList.Add(card);
            }

            foreach (PlayingCard card in sortedHandForRunDiscoveries.Where(c => c.Suit == Suit.Hearts).OrderBy(c => c.Rank).ToList())
            {
                inbetweenList.Add(card);
            }

            foreach (PlayingCard card in sortedHandForRunDiscoveries.Where(c => c.Suit == Suit.Spades).OrderBy(c => c.Rank).ToList())
            {
                inbetweenList.Add(card);
            }
            sortedHandForRunDiscoveries = inbetweenList.ToList();

            // The previous card's suit
            Suit previousSuit = Suit.Clubs; // This and the rank don't matter as they get overwritten first anyways

            // The previous card's rank
            Rank previousRank = Rank.Ace;

            // Number of cards with the same rank in a row
            int runLength = 0;

            // Index of the first card with the same rank
            int firstCardOfRun = -1;

            // Index of the last card with the same rank
            int lastCardOfRun = -1;

            for (int i = 0; i < sortedHandForRunDiscoveries.Count; i++)
            {
                PlayingCard card = sortedHandForRunDiscoveries[i];

                if (i == 0)
                {
                    previousRank = card.Rank;
                    previousSuit = card.Suit;
                    runLength = 1;
                    firstCardOfRun = i;
                    lastCardOfRun = i;
                }
                else if (!(previousRank == Rank.King) && card.Rank == Enum.GetValues<Rank>()[(int)previousRank + 1] && card.Suit == previousSuit)
                {
                    runLength++;
                    lastCardOfRun = i;
                }
                else if (_isAroundTheWorld && card.Rank == Rank.Ace && card.Suit == previousSuit && previousRank == Rank.King)
                {
                    runLength++;
                    lastCardOfRun = i;
                }
                else
                {
                    if (runLength >= 3)
                    {
                        for (int j = firstCardOfRun; j <= lastCardOfRun - 2; j++) // [1, 2, 3, 4, 5] = {1,2,3,4,5}, {2,3,4,5}, {3,4,5}
                        {
                            currentRuns.Add((MeldRun)MeldRun.GenerateMeldFromCards(sortedHandForRunDiscoveries.GetRange(j, lastCardOfRun - j), _isAroundTheWorld));
                            for (int k = j + 2; k <= lastCardOfRun - 2; k++) // [1, 2, 3, 4, 5] = {1,2,3}, {1, 2, 3, 4}, {1, 2, 3, 4, 5}, {2, 3, 4}, {2, 3, 4, 5}, {3, 4, 5}
                            {
                                currentRuns.Add((MeldRun)MeldRun.GenerateMeldFromCards(sortedHandForRunDiscoveries.GetRange(j, k), _isAroundTheWorld));
                            }
                        }
                    }

                    previousRank = card.Rank;
                    previousSuit = card.Suit;
                    runLength = 1;
                    firstCardOfRun = i;
                    lastCardOfRun = i;
                }

                if (i == sortedHandForRunDiscoveries.Count - 1 && runLength >= 3)
                {

                    for (int j = firstCardOfRun; j <= lastCardOfRun - 2; j++) // [1, 2, 3, 4, 5] = {1,2,3,4,5}, {2,3,4,5}, {3,4,5}
                    {
                        currentRuns.Add((MeldRun)MeldRun.GenerateMeldFromCards(sortedHandForRunDiscoveries.GetRange(j, lastCardOfRun - j), _isAroundTheWorld));
                        for (int k = j + 2; k <= lastCardOfRun - 2; k++) // [1, 2, 3, 4, 5] = {1,2,3}, {1, 2, 3, 4}, {1, 2, 3, 4, 5}, {2, 3, 4}, {2, 3, 4, 5}, {3, 4, 5}
                        {
                            currentRuns.Add((MeldRun)MeldRun.GenerateMeldFromCards(sortedHandForRunDiscoveries.GetRange(j, k), _isAroundTheWorld));
                        }
                    }

                }
            }
        }
            

        // This takes in a list of cards to use to find melds, the combinations list to extend, and the list of current melds to locate
        private void RecursivelyIdentifyMeldCombinations(IList<PlayingCard> cardsRemaining, List<List<IMeld>> meldCombinations, List<IMeld> meldCombinationToFurther)
        {
            if (cardsRemaining.Count < 3)
            {
                // If there are less than three cards left, then there are no more combinations to find
                return;
            }

            // Try to form one or more combinations from meld sets
            List<MeldSet> meldSets = new();
            FindMeldSets(cardsRemaining, meldSets);

            // If any meld sets were found, then add them to the combinations list and recurse more deeply
            if (meldSets.Count > 0)
            {
                foreach (MeldSet meldSet in meldSets)
                {
                    List<IMeld> newMeldCombination = new List<IMeld>(meldCombinationToFurther);
                    newMeldCombination.Add(meldSet);
                    meldCombinations.Add(newMeldCombination);
                    RecursivelyIdentifyMeldCombinations(GetRemainingDeadwoodCards(cardsRemaining, newMeldCombination), meldCombinations, newMeldCombination);
                }
            }

            // Try to form one or more combinations from meld runs
            List<MeldRun> meldRuns = new();
            FindMeldRuns(cardsRemaining, meldRuns);

            // If any meld runs were found, then add them to the combinations list and recurse more deeply
            if (meldRuns.Count > 0)
            {
                foreach (MeldRun meldRun in meldRuns)
                {
                    List<IMeld> newMeldCombination = new List<IMeld>(meldCombinationToFurther);
                    newMeldCombination.Add(meldRun);
                    meldCombinations.Add(newMeldCombination);
                    RecursivelyIdentifyMeldCombinations(GetRemainingDeadwoodCards(cardsRemaining, newMeldCombination), meldCombinations, newMeldCombination);
                }
            }
        }
                   

        // Get remaining deadwood cards
        private List<PlayingCard> GetRemainingDeadwoodCards(IList<PlayingCard> originallyRemainingCards, IList<IMeld> meldCombination)
        {
            List<PlayingCard> deadwood = originallyRemainingCards.ToList(); // Clone the hand again

            for (int i = 0; i < meldCombination.Count; i++) // Remove cards already used in melds
            {
                MeldSet meldSet = meldCombination[i] as MeldSet;
                
                if (meldSet == null) 
                { 
                    meldCombination.RemoveAt(i);
                    i--;
                    continue;
                }

                for (int j = 0; j < meldSet.Cards.Count; j++)
                {
                    if (deadwood.Count == 0)
                    {
                        return deadwood;
                    }

                    for (int k = 0; k < deadwood.Count; k++)
                    {
                        if (deadwood[k] == meldSet.Cards[j])
                        {
                            deadwood.RemoveAt(k);
                            break;
                        }
                    }
                }
            }

            return deadwood;
        }
    }
}
