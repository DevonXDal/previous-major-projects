using EarnShardsForCards.Shared.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.GinRummy
{
    /// <summary>
    /// A GinRummyGameState is a virtual game support object that maintains overall state information for 
    /// the game. 
    /// It is necessary to have this to separate concerns from other objects and to track conditions for 
    /// actions either player takes. 
    /// This information includes the current round, scores for each player per round, which variations 
    /// of the rules are in use, whose turn it is and the phase, etc. 
    /// Using this state helps the game flow and aids in determining if a player has won the game.
    /// </summary>
    public class GinRummyGameState
    {
        public int PointsRequiredForWin { get; set; }
        public IList<int> PointsForHumanPlayerPerRound { get; set; }
        public IList<int> PointsForComputerPlayerPerRound { get; set; }
        public int RoundNumber { get; set; }
        public bool IsAroundTheWorld { get; set; }
        public TurnState CurrentPlayersTurn { get; set; }
        public PhaseState CurrentTurnPhase { get; set; }
        public bool IsSpecialDraw { get; set; } // First turn(s) of a round where passing is done.

        /// <summary>
        /// I.	Create a GinRummyGameState object using whether around the world is in play, 
        /// the number of points required to win, and a path to the first player to play.
        /// </summary>
        /// <param name="isAroundTheWorld">Whether runs can loop around from King to Ace</param>
        /// <param name="winCondition">How many points is necessary for a player to win the entire game</param>
        /// <param name="firstToPlay">Who is the first player to play the game</param>
        public GinRummyGameState(bool isAroundTheWorld, int winCondition, TurnState firstToPlay)
        {
            PointsRequiredForWin = winCondition;
            IsAroundTheWorld = isAroundTheWorld;
            CurrentPlayersTurn = firstToPlay;

            PointsForHumanPlayerPerRound = new List<int>();
            PointsForComputerPlayerPerRound = new List<int>();

            RoundNumber = 0;
            CurrentTurnPhase = PhaseState.Draw;
            IsSpecialDraw = true;
        }

        /// <summary>
        /// Determines if the game has been won
        /// i.	Returns -1 if the computer won
        /// ii. Returns 0 if no one has won
        /// iii.Returns 1 if the human player won
        /// </summary>
        /// <returns>The integer value that matches the current state of the game information</returns>
        public int CheckIfGameIsWon()
        {
            if (PointsForComputerPlayerPerRound.Sum() >= PointsRequiredForWin)
            {
                return -1;
            }
            else if (PointsForHumanPlayerPerRound.Sum() >= PointsRequiredForWin)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
