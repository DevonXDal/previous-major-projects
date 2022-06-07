using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Data.Enumerations
{
    /// <summary>
    /// GinRummyRoundEndingCase is used to provide the reason that the round ended. 
    /// This is useful for scoring and for displaying the end of round details to the user.
    /// </summary>
    public enum GinRummyRoundEndingCase
    {
        Tie, Knock, Undercut, Gin, BigGin
    }
}
