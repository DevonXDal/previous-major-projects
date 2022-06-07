using EarnShardsForCards.Shared.Data.Enumerations;
using EarnShardsForCards.Shared.Data.GenericGameObjects;
using EarnShardsForCards.Shared.Data.GinRummy;
using EarnShardsForCards.Shared.Data.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.Interfaces
{
    /// <summary>
    /// Represents the open functionality that can be taken a GinRummyController class by other classes.
    /// The main purpose of this interface is to allow a mock controller to be used for testing purposes.
    /// </summary>
    public interface IGinRummyController
    {

        /// <summary>
        /// Set up the models and controller objects necessary to run a game.
        /// </summary>
        public void InitializeGame();
        
        /// <summary>
        /// Restart the game, performs similar to initialize the game but ensures that the notifier is recreated.
        /// </summary>
        public void ReinitializeGame();

        /// <summary>
        /// Try to pass the human player's turn if validation succeeds.
        /// Must be the player's special draw phase.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown with a message why the action was not done when an illegal action occured</exception>
        public Task RequestPassTurnAsync();

        /// <summary>
        /// Try to perform a draw from deck action after validation for the human player.
        /// Must be the player's normal draw phase.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown with a message why the action was not done when an illegal action occured</exception>
        public void RequestDrawFromDeck();

        /// <summary>
        /// Try to perform a draw from discard action after validation for the human player.
        /// Must be the player's draw phase.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown with a message why the action was not done when an illegal action occured</exception>
        public void RequestDrawFromDiscard();

        /// <summary>
        /// Try to perform a discard action after validation for the human player.
        /// Must be the player's discard phase.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown with a message why the action was not done when an illegal action occured</exception>
        public Task RequestDiscardWithCardAtAsync(int index);

        /// <summary>
        /// Try to perform a knock action after validation for the human player.
        /// Must be the player's discard phase.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown with a message why the action was not done when an illegal action occured</exception>
        public void RequestKnockWithCardAt(int index);

        /// <summary>
        /// Try to reposition the cards within the human player's hand. 
        /// Must be the player's turn.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown with a message why the action was not done when an illegal action occured</exception>
        public void RequestCardReposition(int initialIndex, int newIndex);

        /// <summary>
        /// Get the deadwood remaining for the computer player.
        /// </summary>
        /// <param name="handToCalculateWith">The hand to calculate with. This hand should be either 10 or 11 cards</param>
        /// <returns>The deadwood remaining for the computer player</returns>
        public int CheckComputerPlayerDeadwood(IList<PlayingCard> handToCalculateWith);

        /// <summary>
        /// Receive Indication that display of end of round information has begun displaying
        /// </summary>
        public void NotifyThatEndOfRoundIsDisplayed();

        /// <summary>
        /// Recieve notice that the end of round display is done displaying and the next round/game should begin.
        /// </summary>
        public void EndOfRoundDisplayIsFinished();

        /// <summary>
        /// Recieve results to document about the end of the round.
        /// </summary>
        /// <param name="winner">The reference to the player that won</param>
        /// <param name="points">The amount of points won by the player</param>
        /// <param name="reason">The round ending reason</param>
        /// <param name="laidOffDeadwood">The amount of deadwood laid off by the non-knocking player. Defaults to 0</param>
        public void DocumentRoundResults(Player<PlayingCard> winner, int points, GinRummyRoundEndingCase reason, int laidOffDeadwood = 0);

        /// <summary>
        /// Returns data used to render graphical elements to the screen after each state update.
        /// </summary>
        /// <returns>The data the view needs to redisplay itself</returns>
        public GinRummyViewData? FetchViewData();

        /// <summary>
        /// Returns the data for the end of display to work.
        /// </summary>
        /// <returns>End of round related information</returns>
        public EndOfRoundData FetchEndOfRoundData();

        /// <summary>
        /// Fetches the notifier for the game.
        /// </summary>
        /// <returns>The observer object used to indicate game state changes</returns>
        public Notifier FetchNotifier();
    }
}
