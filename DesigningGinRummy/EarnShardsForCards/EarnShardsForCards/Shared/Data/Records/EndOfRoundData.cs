using EarnShardsForCards.Shared.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.Records
{
    /// <summary>
    /// The data contained in this record is used to display the scoreboard and other 
    /// information at the end of each round or game. 
    /// This includes scores for each player across the rounds played. 
    /// </summary>
    public record EndOfRoundData
    {
        public GinRummyRoundEndingCase RoundEndingReason { get; init; }
        public IList<IList<int>> RoundScores { get; init; } // User and then computer
        public bool WinConditionReached { get; init; }
        public int LaidOffDeadwood { get; init; }
        public int AmountToWin { get; init; }

        /// <summary>
        /// Creates a record for the end of round data to display to the user.
        /// </summary>
        /// <param name="roundEndingReason">What ended the round? Gin? Knock?</param>
        /// <param name="roundScores">Scores from user then the computer for each round</param>
        /// <param name="isGameWon">Has someone won the game</param>
        /// <param name="laidOffDeadwood">How much deadwood was laid off if any (0 if not knock or undercut)</param>
        /// <param name="amountToWin">How much is needed to win the game</param>
        public EndOfRoundData(GinRummyRoundEndingCase roundEndingReason, IList<IList<int>> roundScores, bool isGameWon, int laidOffDeadwood, int amountToWin)
        {
            RoundEndingReason = roundEndingReason;
            RoundScores = roundScores;
            WinConditionReached = isGameWon;
            LaidOffDeadwood = laidOffDeadwood;
            AmountToWin = amountToWin;
        }

    }
}
