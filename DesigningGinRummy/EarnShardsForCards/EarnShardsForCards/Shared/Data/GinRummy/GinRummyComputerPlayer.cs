using EarnShardsForCards.Shared.Data.Enumerations;
using EarnShardsForCards.Shared.Data.GenericGameObjects;
using EarnShardsForCards.Shared.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.GinRummy
{
    /// <summary>
    /// A GinRummyComputerPlayer is a virtual game support object related to a real-world player. 
    /// This player makes decisions in order to beat the human player. 
    /// It represents the other player playing against the one playing the game. 
    /// It maintains a hand and makes better or worse decisions based on its set skill level/difficulty.
    /// </summary>
    /// <typeparam name="T">A playing card or a subtype of playing card</typeparam>
    public class GinRummyComputerPlayer<T> : Player<T> where T : PlayingCard
    {
        public SkillLevel SkillLevel { get; set; }

        private IGinRummyController _controller;
        private IConfiguration _config;

        /// <summary>
        /// Create a new computer player with an empty hand with a skill level of intermediate.
        /// </summary>
        /// <param name="configuration">The configuration file used to supply logic for the decisions made by the computer player</param>
        /// <param name="ginRummyController">The controller used to play Gin Rummy.</param>
        public GinRummyComputerPlayer(IConfiguration configuration, IGinRummyController ginRummyController) : base()
        {
            SkillLevel = SkillLevel.Intermediate;
            _config = configuration;
            _controller = ginRummyController;
        }

        /// <summary>
        /// Check to see if the discard pile card will be drawn by the computer player.
        /// If the discard pile is not drawn, the computer player will choose to draw a card from the deck or pass.
        /// </summary>
        /// <param name="topCardOfDiscardPile">What the card on the discard pile was</param>
        /// <param name="isAroundTheWorld">Whether the around the world variation of Gin Rummy is being played</param>
        /// <returns>Whether the computer chose to draw the discard pile card</returns>
        public bool ShouldDrawFromDiscardPile(T topCardOfDiscardPile, bool isAroundTheWorld)
        {
            IConfigurationSection specificDrawSection = _config.GetSection("GinRummyComputerPlayerDrawPhase").GetSection(Enum.GetName(SkillLevel));

            int percentageOfDrawingThisCard = 0;
            
            if (IsCardRunForming(topCardOfDiscardPile, isAroundTheWorld))
            {
                percentageOfDrawingThisCard += ExtractValueFromConfiguration(specificDrawSection, "FormsRun", 75);
            } else if (DoesCardLeadTowardsRun(topCardOfDiscardPile, isAroundTheWorld))
            {
                percentageOfDrawingThisCard += ExtractValueFromConfiguration(specificDrawSection, "LeadsTowardRun", 20);
            }

            if (IsCardSetForming(topCardOfDiscardPile))
            {
                percentageOfDrawingThisCard += ExtractValueFromConfiguration(specificDrawSection, "FormsSet", 50);
            } else if (DoesCardLeadTowardsSet(topCardOfDiscardPile))
            {
                percentageOfDrawingThisCard += ExtractValueFromConfiguration(specificDrawSection, "LeadsTowardSet", 15);
            }

            foreach (T card in Hand.Where(c => c.Value > topCardOfDiscardPile.Value)) // Check every card that has a value that is greater than the card on the discard pile.
            {
                if (!IsCardRunForming(card, isAroundTheWorld) && !IsCardSetForming(card))
                {
                    percentageOfDrawingThisCard += ExtractValueFromConfiguration(specificDrawSection, "NotLargestDeadwoodCard", 20);
                    break;
                }
            }
            
            switch (percentageOfDrawingThisCard)
            {
                case 0:
                    return false;
                case >= 100: 
                    return true;
                default:
                    return percentageOfDrawingThisCard >= (new Random()).Next(0, 100) + 1; // Use Random like a 100 sided die, return true if the percentage was equal or higher.
            }
        }

        /// <summary>
        /// Return a card that will be removed from the hand as a discard action
        /// </summary>
        /// <param name="isAroundTheWorld">Whether the around the world variation of Gin Rummy is being played</param>
        /// <returns>The card that the computer player removed from their hand to discard</returns>
        public T SelectAndRemoveDiscardedCard(bool isAroundTheWorld)
        {
            
            IConfigurationSection specificDiscardSection = _config.GetSection("GinRummyComputerPlayerDiscardPhase").GetSection(Enum.GetName(SkillLevel));
            List<Tuple<T, int>> handCardRelativeValue = new();
            

            foreach (T currentCard in Hand) // Check every card in the computer player's hand for its hand relative value.
            {
                int currentCardValue = 0;

                if (IsCardRunForming(currentCard, isAroundTheWorld))
                {
                    currentCardValue += ExtractValueFromConfiguration(specificDiscardSection, "FormsRun", 75);
                }
                else if (DoesCardLeadTowardsRun(currentCard, isAroundTheWorld))
                {
                    currentCardValue += ExtractValueFromConfiguration(specificDiscardSection, "LeadsTowardRun", 20);
                }

                if (IsCardSetForming(currentCard))
                {
                    currentCardValue += ExtractValueFromConfiguration(specificDiscardSection, "FormsSet", 50);
                }
                else if (DoesCardLeadTowardsSet(currentCard))
                {
                    currentCardValue += ExtractValueFromConfiguration(specificDiscardSection, "LeadsTowardSet", 25);
                }

                bool shouldCheckCard = true; // This is used because break; is actually breaking something, somewhere.
                foreach (T card in Hand.Where(c => c.Value > currentCard.Value).ToList()) // Check every card that has a value that is greater than the card on the discard pile.
                {
                    if (shouldCheckCard && !(IsCardRunForming(card, isAroundTheWorld) && !IsCardSetForming(card)))
                    {
                        currentCardValue += ExtractValueFromConfiguration(specificDiscardSection, "NotLargestDeadwoodCard", 15);
                        shouldCheckCard = false;
                    }
                }

                handCardRelativeValue.Add(new(currentCard, currentCardValue));
            }

            int numOfWorstCardsToKeep = ExtractValueFromConfiguration(specificDiscardSection, "NumOfWorstCardsForList", 3);

            // Orders from lowest relative value to highest, then it takes the requested number of tuples (card/relative value), then it makes a list of just cards.
            List<T> listOfWorstCards = handCardRelativeValue.OrderBy(tuple => tuple.Item2)
                .Take(numOfWorstCardsToKeep)
                .Select(selected => selected.Item1)
                .ToList();

            T cardToDiscard = listOfWorstCards.ElementAt((new Random()).Next(0, listOfWorstCards.Count)); // Take the card to discard at random.

            Hand.Remove(cardToDiscard); // Remove the discarded card from the hand

            return cardToDiscard; // Return the card that was discarded
        }

        /// <summary>
        /// Determines whether or not to knock or discard. Returns null if a knock is not performed.
        /// </summary>
        /// <returns>Either null or a card from the hand that the computer wishes to knock with</returns>
        public T? DetermineKnockOrDiscardAction(bool isAroundTheWorld)
        {
            IConfigurationSection specificDiscardSection = _config.GetSection("GinRummyComputerPlayerDiscardPhase").GetSection(Enum.GetName(SkillLevel));


            var highestValueCardsFirst = Hand.OrderByDescending(c => c.Value);
            T cardToKnockWith = highestValueCardsFirst.First(); // If a better unmelded card is found then this will be replaced.
            bool shouldCheckCard = true; // This is used because break; is actually breaking something, somewhere.

            foreach (T card in highestValueCardsFirst) // Check for the first unmelded card with the largest value of deadwood. May be the one first selected.
            {
                if (shouldCheckCard && !(IsCardRunForming(card, isAroundTheWorld) || IsCardSetForming(card)))
                {
                    cardToKnockWith = card;
                    shouldCheckCard = false;
                }
            }

            List<PlayingCard> cardsToCheckRemainingDeadwoodWith = new();
            foreach (PlayingCard card in Hand.Where(c => !c.Equals(cardToKnockWith)).ToList()) // Done to handle casting issues around generics
            {
                cardsToCheckRemainingDeadwoodWith.Add(card);
            }
                
            int remainingDeadwood = _controller.CheckComputerPlayerDeadwood(cardsToCheckRemainingDeadwoodWith);
            int chanceToKnock = 0; // Percentage chance to knock

            if (remainingDeadwood > 10) // Too much deadwood still to knock
            {
                return null;
            } else if (remainingDeadwood > 7)
            {
                chanceToKnock = ExtractValueFromConfiguration(specificDiscardSection, "TenDeadwoodKnock", 70);
            } else if (remainingDeadwood > 3)
            {
                chanceToKnock = ExtractValueFromConfiguration(specificDiscardSection, "SevenDeadwoodKnock", 70);
            } else
            {
                chanceToKnock = ExtractValueFromConfiguration(specificDiscardSection, "ThreeDeadwoodKnock", 80);
            }

            if (chanceToKnock >= (new Random()).Next(0, 100) + 1) // If the percent chance was successful, knock with the card.
            {
                Hand.Remove(cardToKnockWith);

                return cardToKnockWith;
            } else
            {
                return null; // Computer player chose to not knock
            }
            
        }

        // Returns true if the new card when added to the hand forms a meld set successfully.
        private bool IsCardSetForming(T card)
        {
            var listSortedOnRank = Hand.Concat(new List<T> { card }).OrderBy(x => x.Rank).ToList();

            // Check for a set of three cards including the new card.
            foreach (PlayingCard secondSetCard in listSortedOnRank)
            {
                if (card.Equals(secondSetCard))
                {
                    continue;
                }

                foreach (PlayingCard thirdSetCard in listSortedOnRank)
                {
                    if (secondSetCard.Equals(thirdSetCard))
                    {
                        continue;
                    }
                    
                    if (MeldSet.GenerateMeldFromCards((IList<PlayingCard>) listSortedOnRank) != null)
                    {
                        return true; // A meld run was formed.
                    }
                    
                }
            }

            return false;
        }

        // Returns true if the new card when added to the hand forms a meld run successfully.
        private bool IsCardRunForming(T card, bool isAroundTheWorld)
        {
            var listSortedOnRank = Hand.Concat(new List<T> { card }).OrderBy(x => x.Suit).ThenBy(x => x.Rank).ToList();

            // Check for a set of three cards including the new card.
            foreach (PlayingCard secondRunCard in listSortedOnRank)
            {
                if (card.Equals(secondRunCard) || card.Suit != secondRunCard.Suit)
                {
                    continue;
                }

                foreach (PlayingCard thirdRunCard in listSortedOnRank)
                {
                    if (secondRunCard.Equals(thirdRunCard) || card.Suit != thirdRunCard.Suit)
                    {
                        continue;
                    }

                    if (MeldRun.GenerateMeldFromCards((IList<PlayingCard>)listSortedOnRank, isAroundTheWorld) != null)
                    {
                        return true; // A meld run was formed.
                    }

                }
            }

            return false;
        }

        // Check to see if the card has the same suit and one more or less rank than any other card in the hand.
        private bool DoesCardLeadTowardsRun(T card, bool isAroundTheWorld)
        {
            if (isAroundTheWorld &&
                Hand.Any(c => c.Suit == card.Suit && // Must be the same suit for a run
                ((card.Rank == Rank.King && c.Rank == Rank.Ace) || (card.Rank == Rank.Ace && c.Rank == Rank.King)))) // Checks for a King/Ace match-up
            {
                return true;
            }
            else if (Hand.Any(c => c.Suit == card.Suit && // Must be the same suit for a run
                Math.Abs(card.Rank - c.Rank) == 1)) // If the difference in rank is only one
            {
                return true;
            } else
            {
                return false;
            }
        }

        // Check to see if the card has the same rank as another card but they are not the same card (when choosing a card to discard).
        private bool DoesCardLeadTowardsSet(T card)
        {
            return Hand.Any(c => !c.Equals(card) && c.Rank == card.Rank);
        }

        // Extract the value if possible from the configuration file. If the value cannot be read the fallback value is used +/- 15%
        // depending of difficulty difference from Intermediate. The fallback amount is usually what the activity diagram had for intermediate difficulty.
        private int ExtractValueFromConfiguration(IConfigurationSection configurationSection, string specificValue, int fallBackAmount)
        {
            if (int.TryParse(configurationSection[specificValue], out int amount))
            {
                return amount;
            }
            else
            {
                return (int)(fallBackAmount + (Math.Round(fallBackAmount * .15) * (SkillLevel - SkillLevel.Intermediate))); // Differ from 15% of the value for each difficulty from the intermediate difficulty
            }
        }
    }
}
