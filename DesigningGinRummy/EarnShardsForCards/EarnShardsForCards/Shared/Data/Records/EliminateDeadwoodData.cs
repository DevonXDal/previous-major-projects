using EarnShardsForCards.Shared.Data.GinRummy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarnShardsForCards.Shared.Models.Records
{
    /// <summary>
    /// The EliminateDeadwoodData record is used for returning a group of information from a method call that can be used for scoring and laying off deadwood.
    /// </summary>
    public record EliminateDeadwoodData
    {
        public int RemainingDeadwood { get; init; }
        public IList<MeldSet> UsedSets { get; init; }
        public IList<MeldRun> UsedRuns { get; init; }

        /// <summary>
        /// Create and store data in one location related to deadwood eliminated from a player and by what.
        /// </summary>
        /// <param name="remainingDeadwood">The deadwood the player still has</param>
        /// <param name="usedSets">What sets helped reduce deadwood</param>
        /// <param name="usedRuns">What runs helped reduce deadwood</param>
        public EliminateDeadwoodData(int remainingDeadwood, IList<MeldSet> usedSets, IList<MeldRun> usedRuns)
        {
            RemainingDeadwood = remainingDeadwood;
            UsedSets = usedSets;
            UsedRuns = usedRuns;
        }
    }
}
