using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coaching.Postflop.Betting
{
    public class BettingDecision
    {
        public Dictionary<BettingDecisionEnum, double> BettingDecisionRatios { get; set; }

        public BettingDecision(Dictionary<BettingDecisionEnum, int> decisionCaseCounts)
        {
            int totalCaseCount = decisionCaseCounts.Values.Sum();
            foreach (var decisionCaseCount in decisionCaseCounts)
            {
                BettingDecisionRatios[decisionCaseCount.Key] = ((double) decisionCaseCount.Value)/totalCaseCount;
            }
        }
    }
}
