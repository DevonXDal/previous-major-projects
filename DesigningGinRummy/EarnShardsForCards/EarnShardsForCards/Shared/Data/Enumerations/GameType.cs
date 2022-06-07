using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.Enumerations
{
    /// <summary>
    /// GameType is used for building objects based on the game being played. 
    /// In the case of a PlayingCardDeck, even though many games use it, some games use only some of the cards, some use more than the typical 52. 
    /// GameType allows improved control of the deck provided for each game.
    /// </summary>
    public enum GameType
    {
        GinRummy
    }
}
