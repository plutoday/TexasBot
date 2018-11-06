using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasBot.Models
{
    public class HolesRecord
    {
        public HolesRecord(IEnumerable<Card> holes)
        {
            Holes = new List<Card>(holes);
            WinScenarioCount = 0;
            LoseScenarioCount = 0;
            TieScenarioCount = 0;
        }

        public List<Card> Holes { get; set; }
        public int WinScenarioCount { get; set; }
        public int LoseScenarioCount { get; set; }
        public int TieScenarioCount { get; set; }
    }
}
