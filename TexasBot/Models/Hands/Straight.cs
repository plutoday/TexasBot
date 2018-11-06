using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace TexasBot.Models.Hands
{
    public class Straight : IHand
    {
        public RankEnum HighRank { get; set; }

        public Straight(RankEnum highRank)
        {
            HighRank = highRank;
        }

        public int CompareTo(IHand other)
        {
            var otherStraight = other as Straight;
            if (otherStraight == null)
            {
                throw new InvalidOperationException();
            }

            return HighRank.CompareTo(otherStraight.HighRank);
        }
    }
}
