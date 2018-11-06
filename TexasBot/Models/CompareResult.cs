using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasBot.Models
{
    public class CompareResult
    {
        public long WinScenarioCount { get; set; }
        public long LoseScenarioCount { get; set; }
        public long TieScenarioCount { get; set; }

        public double WinRatio => (double) WinScenarioCount/(WinScenarioCount + LoseScenarioCount + TieScenarioCount);
        public double LoseRatio => (double)LoseScenarioCount / (WinScenarioCount + LoseScenarioCount + TieScenarioCount);
        public double TieRatio => (double)TieScenarioCount / (WinScenarioCount + LoseScenarioCount + TieScenarioCount);

        public CompareResult(long win, long lose, long tie)
        {
            WinScenarioCount = win;
            LoseScenarioCount = lose;
            TieScenarioCount = tie;
        }

        public void Add(CompareResult result)
        {
            WinScenarioCount += result.WinScenarioCount;
            LoseScenarioCount += result.LoseScenarioCount;
            TieScenarioCount += result.TieScenarioCount;
        }
    }
}
