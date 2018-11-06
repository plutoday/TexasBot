using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace TexasBot.Models.Hands
{
    public class ThreeOfAKind : IHand
    {
        public RankEnum HighRank { get; set; }

        public ThreeOfAKind(RankEnum highRank)
        {
            HighRank = highRank;
        }

        public int CompareTo(IHand other)
        {
            var otherThreeOfAKind = other as ThreeOfAKind;
            if (otherThreeOfAKind == null)
            {
                throw new InvalidOperationException();
            }

            return HighRank.CompareTo(otherThreeOfAKind.HighRank);
        }
    }
}
