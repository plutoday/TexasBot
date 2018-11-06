using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace TexasBot.Models.Hands
{
    public class FourOfAKind : IHand
    {
        public RankEnum HighRank { get; set; }

        public FourOfAKind(RankEnum highRank)
        {
            HighRank = highRank;
        }

        public int CompareTo(IHand other)
        {
            var otherFourOfAKind = other as FourOfAKind;
            if (otherFourOfAKind == null)
            {
                throw new InvalidOperationException();
            }

            return HighRank.CompareTo(otherFourOfAKind.HighRank);
        }
    }
}
