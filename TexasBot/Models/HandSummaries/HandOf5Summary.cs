using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using TexasBot.Tools;

namespace TexasBot.Models.HandSummaries
{
    public class HandOf5Summary
    {
        public List<RankEnum> Ranks { get; set; }
        public bool SameColor { get; set; }
        public int Score { get; set; }

        public HandOf5Summary(HandOf5 handOf5)
        {
            Ranks = handOf5.Cards.Select(c => c.Rank).ToList();
            Ranks.Sort();
            SameColor = handOf5.Cards.All(c => c.Suit == handOf5.Cards.First().Suit);
            Score = handOf5.Score;
        }

        public override string ToString()
        {
            return string.Join("", Ranks.Select(Tools.Utils.GetStringForRank)) + (SameColor ? "S" : "N");
        }
    }
}
