using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace TexasBot.Models.Hands
{
    public class StraightFlush : IHand
    {
        public RankEnum HighRank { get; set; }

        public StraightFlush(RankEnum highRank)
        {
            HighRank = highRank;
        }

        public int CompareTo(IHand other)
        {
            var otherStraightFlush = other as StraightFlush;
            if (otherStraightFlush == null)
            {
                throw new InvalidOperationException();
            }

            return HighRank.CompareTo(otherStraightFlush.HighRank);
        }
    }
}
