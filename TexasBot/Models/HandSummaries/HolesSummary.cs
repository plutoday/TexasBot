using System.Collections.Generic;
using System.Linq;
using Models;
using TexasBot.Tools;

namespace TexasBot.Models.HandSummaries
{
    public class HolesSummary
    {
        public List<RankEnum> Ranks { get; set; }
        public bool SameColor { get; set; }

        public HolesSummary(IEnumerable<Card> holes)
        {
            Ranks = new List<RankEnum>(holes.Select(c => c.Rank));
            Ranks.Sort();
            SameColor = holes.All(c => c.Suit == holes.First().Suit);
        }

        public override string ToString()
        {
            return string.Join("", Ranks.Select(Tools.Utils.GetStringForRank)) + (SameColor ? "S" : "N");
        }
    }
}
