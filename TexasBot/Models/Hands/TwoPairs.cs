using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace TexasBot.Models.Hands
{
    public class TwoPairs : IHand
    {
        public RankEnum HighPairRank { get; set; }
        public RankEnum LowPairRank { get; set; }
        public RankEnum SingleCardRank { get; set; }

        public TwoPairs(RankEnum highPairRank, RankEnum lowPairRank, RankEnum singleCardRank)
        {
            HighPairRank = highPairRank;
            LowPairRank = lowPairRank;
            SingleCardRank = singleCardRank;
        }

        public int CompareTo(IHand other)
        {
            var otherTwoPairs = other as TwoPairs;
            if (otherTwoPairs == null)
            {
                throw new InvalidOperationException();
            }

            if (HighPairRank != otherTwoPairs.HighPairRank)
            {
                return HighPairRank.CompareTo(otherTwoPairs.HighPairRank);
            }

            if (LowPairRank != otherTwoPairs.LowPairRank)
            {
                return LowPairRank.CompareTo(otherTwoPairs.LowPairRank);
            }
            return SingleCardRank.CompareTo(otherTwoPairs.SingleCardRank);
        }
    }
}
