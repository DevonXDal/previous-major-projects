using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.Enumerations
{
    /// <summary>
    /// A TurnState represents whose turn it is to perform an action that will alter the state of the game.
    /// </summary>
    public enum TurnState
    {
        Human,
        Computer
    }
}
