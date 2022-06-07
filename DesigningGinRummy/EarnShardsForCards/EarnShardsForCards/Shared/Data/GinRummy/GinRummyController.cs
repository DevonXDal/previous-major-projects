using EarnShardsForCards.Shared.Data.Enumerations;
using EarnShardsForCards.Shared.Data.GenericGameObjects;
using EarnShardsForCards.Shared.Data.Interfaces;
using EarnShardsForCards.Shared.Data.Records;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.GinRummy
{
    /// <summary>
    /// A GinRummyController is a virtual game support object specifically for Gin Rummy '
    /// that handles the general game loop taken by the game. 
    /// Once created, only one copy of it will exist and many objects will have references 
    /// to that one reference. When an action is performed the controller can be notified 
    /// and the action carried out if able. When the game is started or restarted, the 
    /// controller will handle the setup of the game board.
    /// </summary>
    public class GinRummyController : IGinRummyController
    {
        public int SelectedCardIndex { get; set; }

        private GinRummyBoard? _board;
        private GinRummyScoreHandler? _scoreHandler;
        private GinRummyGameState? _gameState;
        private IConfiguration _config;
        private Notifier _notifier;
        private bool _needDisplayEndOfRound;
        bool wasThereAPreviousPassThisRound;
        private int _laidOffDeadwoodPreviously;
        private GinRummyRoundEndingCase _previousRoundEndingCase;
        private static GinRummyController? _controller;

        /// <summary>
        /// Create a new controller reference to use with the game.
        /// </summary>
        private GinRummyController(IConfiguration config)
        {
            _notifier = new Notifier();
            _config = config;
            wasThereAPreviousPassThisRound = false;

            // The data below should not be read until after being reassinged.
            _previousRoundEndingCase = GinRummyRoundEndingCase.Tie;
            _laidOffDeadwoodPreviously = 0;
        }

        /// <summary>
        /// Gets the singleton instance of the controller and sets one up if one does not already exist.
        /// </summary>
        /// <returns>The singleton of the controller</returns>
        public static GinRummyController GetInstance(IConfiguration config)
        {
            if (_controller == null)
            {
                _controller = new GinRummyController(config);
            }

            return _controller;
        }

        /// <summary>
        /// Set up the models and controller objects necessary to run a game.
        /// </summary>
        public void InitializeGame()
        {
            wasThereAPreviousPassThisRound = false;
            _board = new GinRummyBoard(_config, this);
            _scoreHandler = new GinRummyScoreHandler(this, _board.Player, _board.ComputerPlayer);
            _board.ComputerPlayer.SkillLevel = SkillLevel.Intermediate; // NEEDS CHANGED BEFORE SHOWCASE
            _gameState = new(false, 100, TurnState.Human); // NEEDS CHANGED BEFORE SHOWCASE
            _board.Deck.Shuffle();
            DealCards();

            PlayingCard initialDiscardPileCard = _board.Deck.Draw();
            initialDiscardPileCard.Show();
            _board.DiscardPile.Add(initialDiscardPileCard); // Add the first card to the discard pile     

            _notifier.SendNotice();
        }

        /// <summary>
        /// Restart the game, performs similar to initializing the game. May perform other actions in the future.
        /// </summary>
        public void ReinitializeGame()
        {
            InitializeGame();
        }

        /// <summary>
        /// Try to pass the human player's turn if validation succeeds.
        /// Must be the player's special draw phase.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown with a message why the action was not done when an illegal action occured</exception>
        public async Task RequestPassTurnAsync()
        {
            if (_gameState.CurrentPlayersTurn != TurnState.Human)
            {
                throw new InvalidOperationException("You cannot choose whether the opponent passes their turn");
            }
            else if (!_gameState.IsSpecialDraw)
            {
                throw new InvalidOperationException("You can only pass during the special draw phase");
            }
            else if (_gameState.CurrentTurnPhase == PhaseState.Discard)
            {
                throw new InvalidOperationException("You can only pass during the special draw phase");
            }
            else
            {
                if (wasThereAPreviousPassThisRound)
                {
                    _gameState.IsSpecialDraw = false;
                } else {
                    wasThereAPreviousPassThisRound = true;
                }
                
                _gameState.CurrentPlayersTurn = TurnState.Computer;
                _notifier.SendNotice(); // Update the view
                await HandleComputersTurnAsync();
            }
        }

        /// <summary>
        /// Try to perform a draw from deck action after validation for the human player.
        /// Must be the player's normal draw phase.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown with a message why the action was not done when an illegal action occured</exception>
        public void RequestDrawFromDeck()
        {
            if (_gameState.CurrentPlayersTurn != TurnState.Human)
            {
                throw new InvalidOperationException("You cannot draw cards from the deck during the opponent's turn");
            }
            else if (_gameState.IsSpecialDraw)
            {
                throw new InvalidOperationException("You can only draw from the deck during your normal draw phase");
            }
            else if (_gameState.CurrentTurnPhase == PhaseState.Discard)
            {
                throw new InvalidOperationException("You cannot draw from the deck right now, you must choose a card to discard or knock");
            }
            else
            {
                PlayingCard drawnCard = _board.Deck.Draw();
                drawnCard.Show();
                _board.Player.Insert(_board.Player.Count(), drawnCard);

                _gameState.CurrentTurnPhase = PhaseState.Discard;
                _notifier.SendNotice(); // Update the view

                CheckForBigGin();
            }
        }

        /// <summary>
        /// Try to perform a draw from discard action after validation for the human player.
        /// Must be the player's draw phase.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown with a message why the action was not done when an illegal action occured</exception>
        public void RequestDrawFromDiscard()
        {
            if (_gameState.CurrentPlayersTurn != TurnState.Human)
            {
                throw new InvalidOperationException("You cannot draw cards from the discard pile during the opponent's turn");
            }
            else if (_gameState.CurrentTurnPhase == PhaseState.Discard)
            {
                throw new InvalidOperationException("You cannot draw from the discard pile right now, you must choose a card to discard or knock");
            }
            else
            {
                _board.Player.Insert(_board.Player.Count(), _board.DiscardPile.Draw());

                _gameState.CurrentTurnPhase = PhaseState.Discard;
                _gameState.IsSpecialDraw = false;
                _notifier.SendNotice(); // Update the view

                CheckForBigGin();
            }
        }

        /// <summary>
        /// Try to perform a discard action after validation for the human player.
        /// Must be the player's discard phase.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown with a message why the action was not done when an illegal action occured</exception>
        public async Task RequestDiscardWithCardAtAsync(int index)
        {
            if (_gameState.CurrentPlayersTurn != TurnState.Human)
            {
                throw new InvalidOperationException("You cannot draw cards from the discard pile during the opponent's turn");
            }
            else if (_gameState.CurrentTurnPhase != PhaseState.Discard)
            {
                throw new InvalidOperationException("You cannot discard a card during your draw phase");
            }
            else
            {
                _board.DiscardPile.Add(_board.Player.RemoveAt(index));

                _gameState.CurrentPlayersTurn = TurnState.Computer;
                _gameState.CurrentTurnPhase = PhaseState.Draw;
                
                
                _notifier.SendNotice(); // Update the view
                await HandleComputersTurnAsync();
            }
        }

        /// <summary>
        /// Try to perform a knock action after validation for the human player.
        /// Must be the player's discard phase.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown with a message why the action was not done when an illegal action occured</exception>
        public void RequestKnockWithCardAt(int index)
        {
            if (_gameState.CurrentPlayersTurn != TurnState.Human)
            {
                throw new InvalidOperationException("You cannot knock during the opponent's turn");
            } else if (_gameState.CurrentTurnPhase != PhaseState.Discard)
            {
                throw new InvalidOperationException("You must perform your knocks during the discard phase");
            }

            if (index < 0 || index >= _board.Player.Count())
            {
                throw new InvalidOperationException("You can't knock with a card that doesn't exist");
            }

            PlayingCard cardBeingUsedForAttemptedKnock = _board.Player.RemoveAt(index);

            if (_scoreHandler.CanPlayerKnock(_board.Player))
            {
                cardBeingUsedForAttemptedKnock.Hide();
                _board.DiscardPile.Add(cardBeingUsedForAttemptedKnock);
                foreach (var card in _board.ComputerPlayer.Hand)
                {
                    card.Show(); // Ensure all cards are shown
                }
                _notifier.SendNotice(); // Update the view

                _scoreHandler.RewardPoints(false, _board.Player);
            } else
            {
                _board.Player.Insert(index, cardBeingUsedForAttemptedKnock);
                throw new InvalidOperationException("You can only knock if doing so would leave you with ten or less deadwood");
            }
        }

        /// <summary>
        /// Try to reposition the cards within the human player's hand. 
        /// Must be the player's turn.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown with a message why the action was not done when an illegal action occured</exception>
        public void RequestCardReposition(int initialIndex, int newIndex)
        {
            if (_gameState.CurrentPlayersTurn != TurnState.Human)
            {
                throw new InvalidOperationException("You can only rearrange your hand on your turn");
            }
            else
            {
                // Swap the card from the initial index in the hand to the new index in the hand.
                PlayingCard temp = _board.Player.Hand[initialIndex];
                _board.Player.Hand[initialIndex] = _board.Player.Hand[newIndex];
                _board.Player.Hand[newIndex] = temp;
            }

            _notifier.SendNotice();
        }

        /// <summary>
        /// Get the deadwood remaining for the computer player.
        /// </summary>
        /// <param name="handToCalculateWith">The hand to calculate with. This hand should be either 10 or 11 cards</param>
        /// <returns>The deadwood remaining for the computer player</returns>
        public int CheckComputerPlayerDeadwood(IList<PlayingCard> handToCalculateWith)
        {
            var computerPlayer = _board.ComputerPlayer;

            var currentComputerPlayerHand = computerPlayer.Hand.ToList();

            computerPlayer.Hand.Clear(); // Swap out the hand for the hand to calculate with
            foreach (var card in handToCalculateWith)
            {
                computerPlayer.Hand.Add(card);
            }

            int deadwood = _scoreHandler.EliminateDeadwood(computerPlayer, null, null).RemainingDeadwood;

            computerPlayer.Hand.Clear(); // Reinsert the original hand
            foreach (var card in currentComputerPlayerHand)
            {
                computerPlayer.Hand.Add(card);
            }

            return deadwood;
        }

        /// <summary>
        /// Receive Indication that display of end of round information has begun displaying
        /// </summary>
        public void NotifyThatEndOfRoundIsDisplayed()
        {
            _needDisplayEndOfRound = false;
        }

        /// <summary>
        /// Recieve notice that the end of round display is done displaying and the next round/game should begin.
        /// </summary>
        public void EndOfRoundDisplayIsFinished()
        {
            if (_needDisplayEndOfRound)
            {
                return; // Display never mentioned that it was displaying
            }

            _notifier.SendNotice();

            if (_gameState.CheckIfGameIsWon() != 0) // Game is won
            {
                ReinitializeGame();
            }
            else
            {
                SetupNextRound();
            }
        }

        /// <summary>
        /// Recieve results to document about the end of the round.
        /// </summary>
        /// <param name="winner">The reference to the player that won</param>
        /// <param name="points">The amount of points won by the player</param>
        /// <param name="reason">The round ending reason</param>
        /// <param name="laidOffDeadwood">The amount of deadwood laid off by the non-knocking player. Defaults to 0</param>
        public void DocumentRoundResults(Player<PlayingCard> winner, int points, GinRummyRoundEndingCase reason, int laidOffDeadwood = 0)
        {
            _previousRoundEndingCase = reason;
            _laidOffDeadwoodPreviously = laidOffDeadwood;

            if (winner == _board.Player)
            {
                _gameState.PointsForHumanPlayerPerRound.Add(points); // Reward the human player for their win
                _gameState.PointsForComputerPlayerPerRound.Add(0); // Reward the computer player nothing for their loss
            }
            else
            {
                _gameState.PointsForHumanPlayerPerRound.Add(0); // Reward the human player nothing for their loss
                _gameState.PointsForComputerPlayerPerRound.Add(points); // Reward the computer player for their win
            }

            _needDisplayEndOfRound = true;
            _notifier.SendNotice();
        }

        /// <summary>
        /// Returns data used to render graphical elements to the screen after each state update.
        /// </summary>
        /// <returns>The data the view needs to redisplay itself; null is returned when initialization is not yet done</returns>
        public GinRummyViewData? FetchViewData()
        {
            if (_gameState == null)
            {
                return null;
            }

            return new(_gameState.CurrentPlayersTurn,
                _gameState.CurrentTurnPhase,
                _gameState.IsSpecialDraw,
                _board.Player.Hand.Select(x => x.GetImageFilePath()).ToList(),
                _board.ComputerPlayer.Hand.Select(x => x.GetImageFilePath()).ToList(),
                _board.Player.Hand.Count,
                _board.ComputerPlayer.Hand.Count,
                _board.DiscardPile.GetImageFilePath(),
                _board.Deck.Count(),
                _needDisplayEndOfRound);
        }

        /// <summary>
        /// Returns the data for the end of display to work.
        /// </summary>
        /// <returns>End of round related information</returns>
        public EndOfRoundData FetchEndOfRoundData()
        {
            List<IList<int>> scoreboardData = new();
            scoreboardData.Add(_gameState.PointsForHumanPlayerPerRound);
            scoreboardData.Add(_gameState.PointsForComputerPlayerPerRound);

            return new(_previousRoundEndingCase, scoreboardData, _gameState.CheckIfGameIsWon() != 0, _laidOffDeadwoodPreviously, _gameState.PointsRequiredForWin);
        }

        /// <summary>
        /// Checks to see if Big Gin has just occured.
        /// </summary>
        public void CheckForBigGin()
        {
            if (_gameState.CurrentPlayersTurn == TurnState.Human && _scoreHandler.DoesPlayerHaveBigGin(_board.Player))
            {
                foreach (var card in _board.ComputerPlayer.Hand)
                {
                    card.Show(); // Ensure all cards are shown
                }
                _notifier.SendNotice();
                _scoreHandler.RewardPoints(true, _board.Player);
            }
            else if (_gameState.CurrentPlayersTurn == TurnState.Computer && _scoreHandler.DoesPlayerHaveBigGin(_board.ComputerPlayer))
            {
                foreach (var card in _board.ComputerPlayer.Hand)
                {
                    card.Show(); // Ensure all cards are shown
                }
                _notifier.SendNotice();
                _scoreHandler.RewardPoints(true, _board.ComputerPlayer);
            }
        }

        /// <summary>
        /// Sets up the next round with the player who did not earn points last round going first.
        /// A tie will have the game move to the next player's turn and have them start.
        /// </summary>
        public void SetupNextRound()
        {
            wasThereAPreviousPassThisRound = false;
            _gameState.IsSpecialDraw = true;
           

            _board.Deck = GinRummyBoard.RefreshDeck();
            _board.Deck.Shuffle();
            _board.DiscardPile.Clear();
            
            _board.Player.Clear();
            _board.ComputerPlayer.Clear();

            DealCards();

            if (_gameState.PointsForHumanPlayerPerRound[_gameState.RoundNumber] > 0)
            {
                _gameState.CurrentPlayersTurn = TurnState.Computer;
            } else if (_gameState.PointsForComputerPlayerPerRound[_gameState.RoundNumber] > 0)
            {
                _gameState.CurrentPlayersTurn = TurnState.Human;
            } else // Tie round
            {
                // Whoever is next to play gets to go next
                _gameState.CurrentPlayersTurn = (_gameState.CurrentPlayersTurn == TurnState.Human) ? TurnState.Computer : TurnState.Human;
            }

            _gameState.CurrentTurnPhase = PhaseState.Draw;
            _gameState.IsSpecialDraw = true;
            _gameState.RoundNumber++;
            _notifier.SendNotice();
        }

        /// <summary>
        /// Fetches the notifier for the game.
        /// </summary>
        /// <returns>The observer object used to indicate game state changes</returns>
        public Notifier FetchNotifier()
        {
            return _notifier;
        }

        /// <summary>
        /// Requests that an action similar to many games occurs.
        /// </summary>
        /// <param name="type">Type of general game action to request to happen</param>
        public void RequestGeneralGameAction(GameButtonType type)
        {
            switch (type)
            {
                case GameButtonType.Start:
                    InitializeGame();
                    break;
                case GameButtonType.Restart:
                    ReinitializeGame();
                    break;
                case GameButtonType.Pass:
                    RequestPassTurnAsync();
                    break;
            }
        }

        // Returns true if the next player would start their turn with only two cards left in the deck.
        private void CheckIfTieHasOccured()
        {
            // If the deck has only two cards left, then the round is over.
            if (_board.Deck.Count() == 2)
            {
                _gameState.PointsForHumanPlayerPerRound.Add(0);
                _gameState.PointsForComputerPlayerPerRound.Add(0);
                foreach (var card in _board.ComputerPlayer.Hand)
                {
                    card.Show(); // Ensure all cards are shown
                }
                
                _needDisplayEndOfRound = true;
                _notifier.SendNotice();
            }
        }

        // Handles the actions taken during the computer player's turn.
        private async Task HandleComputersTurnAsync()
        {
            Random rand = new();
            var computerPlayer = _board.ComputerPlayer;

            // Draw phase
            await Task.Delay(rand.Next(2500, 3500)); // Delay for a random amount of time between 2.5 and 3.5 seconds to simulate computer thinking
            if (computerPlayer.ShouldDrawFromDiscardPile(_board.DiscardPile.ViewTop(), _gameState.IsAroundTheWorld))
            {
                // Draw the discard pile card and put it in the computer's hand but ensure the card is face down (hidden first)
                var card = _board.DiscardPile.Draw();
                card.Hide();
                computerPlayer.Hand.Add(card);
                _gameState.IsSpecialDraw = false; // Ensures the next player can draw from the deck
            }
            else
            {
                // Either pass or draw from the deck depending on whether it is a special draw phase or not
                if (_gameState.IsSpecialDraw)
                {
                    if (wasThereAPreviousPassThisRound)
                    {
                        _gameState.IsSpecialDraw = false; // No special draw phase after the both players have had a chance to pass or discard draw
                    }
                    else
                    {
                        // Mark the pass as having occured
                        wasThereAPreviousPassThisRound = true;
                    }

                    _gameState.CurrentPlayersTurn = TurnState.Human;
                    _gameState.CurrentTurnPhase = PhaseState.Draw;
                    _notifier.SendNotice(); // Update the view
                    return; // The computer chose to pass
                }
                else
                {
                    // Draw from the deck
                    var card = _board.Deck.Draw();
                    card.Hide();
                    computerPlayer.Hand.Add(card);
                }
            }

            _gameState.CurrentTurnPhase = PhaseState.Discard;
            _notifier.SendNotice();

            CheckForBigGin();

            // Discard phase
            // Wait another 2.5 to 3.5 seconds to simulate computer thinking
            await Task.Delay(rand.Next(2500, 3500));
            PlayingCard? possibleCardToKnock = computerPlayer.DetermineKnockOrDiscardAction(_gameState.IsAroundTheWorld);
            if (possibleCardToKnock != null)
            {
                // If the computer player has a card that can knock and chooses to knock, then it will be carried out
                computerPlayer.Hand.Remove(possibleCardToKnock); // Remove the card from the computer's hand
                possibleCardToKnock.Hide(); // Ensure the card is hidden so it is face down on the discard pile.
                _board.DiscardPile.Add(possibleCardToKnock); // Add the card to the discard pile
                

                foreach (var card in computerPlayer.Hand)
                {
                    card.Show(); // Ensure all cards are shown
                }
                _notifier.SendNotice();

                _scoreHandler.RewardPoints(false, computerPlayer);
                return; // Round ended by knocking
            }
            else
            {
                // Otherwise, they will discard
                PlayingCard discardedCard = computerPlayer.SelectAndRemoveDiscardedCard(_gameState.IsAroundTheWorld);
                discardedCard.Show(); // Ensure the card is shown so it is face up on the discard pile to the user to indicate the round is still in progress.
                _board.DiscardPile.Add(discardedCard); // Add the card to the discard pile
            }

            _gameState.CurrentPlayersTurn = TurnState.Human;
            _gameState.CurrentTurnPhase = PhaseState.Draw;
            _notifier.SendNotice(); // Update the view

            CheckIfTieHasOccured(); // Next turn may have only two cards left in the deck.
        }

        // Deals ten cards, one at a time, to each of the players - Uses 20 cards
        private void DealCards()
        {
            Player<PlayingCard> humanPlayer = _board.Player;
            GinRummyComputerPlayer<PlayingCard> computerPlayer = _board.ComputerPlayer;

            // Deal the cards to the players
            for (int i = 1; i <= 10; i++)
            {
                PlayingCard cardForHumanPlayer = _board.Deck.Draw();
                cardForHumanPlayer.Show();
                humanPlayer.Hand.Add(cardForHumanPlayer);
                
                computerPlayer.Hand.Add(_board.Deck.Draw());
            }
        }
    }
}
