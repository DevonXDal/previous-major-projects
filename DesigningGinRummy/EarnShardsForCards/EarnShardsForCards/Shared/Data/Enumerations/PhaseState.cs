using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.Enumerations
{
    /// <summary>
    /// A PhaseState represents one of the two possible phases of games like Gin Rummy. 
    /// It is used to tell which step the current player needs to take next.
    /// </summary>
    public enum PhaseState
    {
        Draw,
        Discard
    }
}
