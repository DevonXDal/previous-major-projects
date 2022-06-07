using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.Enumerations
{
    /// <summary>
    /// The CardType is used to indicate the type of deck to build using the abstract factory pattern. 
    /// Specifying PlayingCard, for example, will indicate to use a PlayingCardDeck object and build what is needed from there
    /// </summary>
    public enum CardType
    {
        PlayingCard
    }
}
