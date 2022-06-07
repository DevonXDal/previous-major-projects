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
    /// A GinRummyBoard represents the real-world table that Gin Rummy is played on. 
    /// In the aspect of the virtual game, it is used as an object to hold the other objects. 
    /// The one game support capability it includes not typical of a normal table is knowing 
    /// whether the pass button is being shown.
    /// </summary>
    public class GinRummyBoard
    {
        public GinRummyComputerPlayer<PlayingCard> ComputerPlayer { get; private set; }
        public Player<PlayingCard> Player { get; private set; }
        public PlayingCardDeck<PlayingCard> Deck { get; set; }
        public DiscardPile<PlayingCard> DiscardPile { get; private set; }

        /// <summary>
        /// Create a new GinRummyBoard holding information related to the board and its players
        /// </summary>
        public GinRummyBoard(IConfiguration config, IGinRummyController controller)
        {
            ComputerPlayer = new GinRummyComputerPlayer<PlayingCard>(config, controller);
            Player = new Player<PlayingCard>();

            Deck = RefreshDeck();
            DiscardPile = new DiscardPile<PlayingCard>();
        }

        /// <summary>
        /// Loads the deck with a fresh set of cards, will be removed with a deck factory when able
        /// </summary>
        public static PlayingCardDeck<PlayingCard> RefreshDeck()
        {
            List<PlayingCard> cards = new();
            
            foreach (Rank rank in Enum.GetValues(typeof(Rank))) {
                foreach (Suit suit in Enum.GetValues(typeof(Suit))) {
                    int value = (int)rank + 1;

                    if (value >= 10)
                    {
                        value = 10;
                    }
                    cards.Add(new PlayingCard(rank, suit, value));
                }
            }

            return new PlayingCardDeck<PlayingCard>(cards);
        }
    }
}
