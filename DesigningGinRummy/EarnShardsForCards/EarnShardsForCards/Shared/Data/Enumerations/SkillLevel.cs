using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.Enumerations
{
    /// <summary>
    /// The SkillLevel represents the difficulty rating of the computer player. 
    /// Changing this value will allow the computer player to make smarter decisions about how to play.
    /// </summary>
    public enum SkillLevel
    {
        Beginner,
        Intermediate,
        Advanced,
    }
}
