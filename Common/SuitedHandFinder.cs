using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Hand;
using Models.Hand.SuitedHand;

namespace Common
{
    public class SuitedHandFinder
    {
        public IHand FindBestHand(List<RankEnum> ranks)
        {
            if (ranks.Count < 5)
            {
                return null;
            }

            foreach (var rank in ranks)
            {
                if (ranks.Count(r => r == rank) > 1)
                {
                    throw new InvalidOperationException($"More than 1 {rank}");
                }
            }

            var sortedOnRank = Models.Utils.SortRanks(ranks).Select(t => t.Item1).ToList();
            
            for (int i = 0; i <= sortedOnRank.Count - 5; i++)
            {
                if (sortedOnRank[i] - sortedOnRank[i + 4] == 4)
                {
                    //Straight
                    if (sortedOnRank[i] == RankEnum.Ace)
                    {
                        return new RoyalFlush();
                    }
                    return new StraightFlush(sortedOnRank[i]);
                }
            }
            return new Flush(sortedOnRank[0], sortedOnRank[1], sortedOnRank[2], sortedOnRank[3], sortedOnRank[4]);
        }
    }
}
