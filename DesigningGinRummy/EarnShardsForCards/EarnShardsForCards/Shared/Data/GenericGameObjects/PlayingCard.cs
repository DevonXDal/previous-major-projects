using EarnShardsForCards.Shared.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.GenericGameObjects
{
    /// <summary>
    /// A PlayingCard represents the real-world card from a standard 52-card deck. 
    /// It holds information about its rank and suit. 
    /// It can either be face up or face down with the facedown side revealing no rank or suit information. 
    /// All cards have the same face down representation but each face up representation is unique 
    /// from the others.
    /// 
    /// </summary>
    public class PlayingCard : Card
    {
        public Rank Rank { get; init; }
        public Suit Suit { get; init; }
        public int Value { get; init; }

        /// <summary>
        /// Create the playing card, assigning it a rank, suit, and point value.
        /// </summary>
        /// <param name="rank">The alphanumeric character that aids in identification</param>
        /// <param name="suit">The shape that aids in identification</param>
        /// <param name="value">The value that the card has in relation to a game</param>
        public PlayingCard(Rank rank, Suit suit, int value) : base($"img/PlayingCard/{Enum.GetName(rank)}-{Enum.GetName(suit)}.webp", "img/PlayingCard/Back.webp")
        {
            Rank = rank;
            Suit = suit;
            Value = value;
        }
        
        

        /// <summary>
        /// Checks to see if both cards have the same rank and suit combination.
        /// </summary>
        /// <param name="obj">The other object, likely a playing card, to check</param>
        /// <returns>Are both the same card?</returns>
        public override bool Equals(object? obj)
        {
            if (obj is PlayingCard other)
            {
                return Rank == other.Rank && Suit == other.Suit;
            }
            
            return false;
        }
    }
}
