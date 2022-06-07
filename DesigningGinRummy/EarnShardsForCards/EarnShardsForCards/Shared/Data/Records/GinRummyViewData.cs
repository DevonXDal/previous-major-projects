using EarnShardsForCards.Shared.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.Records
{
    /// <summary>
    /// The GinRummyViewData record is used for sending information to the view in order to be displayed 
    /// to the user. Each view component can take what it needs to display the information to the user.
    /// </summary>
    public record GinRummyViewData
    {
        public TurnState CurrentTurn { get; init; }
        public PhaseState CurrentPhase { get; init; }
        public bool IsSpecialDraw { get; init; }
        public IList<string> HumanPlayerHandImages { get; init; }
        public IList<string> ComputerPlayerHandImages { get; init; }
        public int HumanPlayerHandSize { get; init; }
        public int ComputerPlayerHandSize { get; init; }
        public string DiscardPileImageFilePath { get; init; }
        public int RemainingCardsInDeck { get; init; }

        public bool IsRoundOver { get; init; }

        /// <summary>
        /// Set up the view data with information to display to the screen.
        /// </summary>
        /// <param name="currentTurn">Whose turn it is to play</param>
        /// <param name="currentPhase">What phase of the current turn is it</param>
        /// <param name="isSpecialDraw">Is drawing from the deck allowed or only passing?</param>
        /// <param name="humanPlayerHandImages">Images in order for the cards in the user's hand</param>
        /// <param name="computerPlayerHandImages">Images in order for the cards in the computer's hand</param>
        /// <param name="humanPlayerHandSize">Number of cards in the user's hand</param>
        /// <param name="computerPlayerHandSize">Number of cards in the computer's hand</param>
        /// <param name="discardPileImageFilePath">Image representation for the discard pile</param>
        /// <param name="remainingCardsInDeck">Number of cards in the deck currently</param>
        /// <param name="isRoundOver">Is the round over?</param>
        public GinRummyViewData(
            TurnState currentTurn, 
            PhaseState currentPhase, bool isSpecialDraw, 
            IList<string> humanPlayerHandImages, 
            IList<string> computerPlayerHandImages, 
            int humanPlayerHandSize, 
            int computerPlayerHandSize, 
            string discardPileImageFilePath, 
            int remainingCardsInDeck,
            bool isRoundOver)
        {
            CurrentTurn = currentTurn;
            CurrentPhase = currentPhase;
            IsSpecialDraw = isSpecialDraw;
            HumanPlayerHandImages = humanPlayerHandImages;
            ComputerPlayerHandImages = computerPlayerHandImages;
            HumanPlayerHandSize = humanPlayerHandSize;
            ComputerPlayerHandSize = computerPlayerHandSize;
            DiscardPileImageFilePath = discardPileImageFilePath;
            RemainingCardsInDeck = remainingCardsInDeck;
            IsRoundOver = isRoundOver;
        }

    }
}
