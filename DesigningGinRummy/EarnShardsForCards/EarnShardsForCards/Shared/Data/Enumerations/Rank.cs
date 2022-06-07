using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.Enumerations
{
    /// <summary>
    /// Represents one of the thirteen possible alphanumeric characters on a playing card. 
    /// It presents an order to the cards and can be associated with a point value.
    /// </summary>
    public enum Rank
    {
        Ace = 0,
        Two = 1,
        Three = 2,
        Four = 3,
        Five = 4,
        Six = 5,
        Seven = 6,
        Eight = 7,
        Nine = 8,
        Ten = 9,
        Jack = 10,
        Queen = 11,
        King = 12
            
    }
}
