using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace TexasBot.Models.Hands
{
    public class OnePair : IHand
    {
        public RankEnum PairRank { get; set; }

        public List<RankEnum> SingleRanks { get; set; }

        public OnePair(RankEnum pairRank, List<RankEnum> singleRanks)
        {
            PairRank = pairRank;
            SingleRanks = new List<RankEnum>(singleRanks);
            SingleRanks.Sort((r1, r2) => -r1.CompareTo(r2));
        }

        public int CompareTo(IHand other)
        {
            var otherOnePair = other as OnePair;
            if (otherOnePair == null)
            {
                throw new InvalidOperationException();
            }

            if (PairRank != otherOnePair.PairRank)
            {
                return PairRank.CompareTo(otherOnePair.PairRank);
            }

            for (int i = 0; i < 3; i++)
            {
                if (SingleRanks[i] != otherOnePair.SingleRanks[i])
                {
                    return SingleRanks[i].CompareTo(otherOnePair.SingleRanks[i]);
                }
            }

            return 0;
        }
    }
}
