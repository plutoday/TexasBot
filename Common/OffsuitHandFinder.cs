using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Hand;
using Models.Hand.OffsuitHand;

namespace Common
{
    public class OffsuitHandFinder
    {
        public IHand FindBestHand(List<RankEnum> ranks)
        {
            var sortedOnCount = Models.Utils.SortCounts(ranks);
            var sortedOnRank = Models.Utils.SortRanks(ranks);
            if (sortedOnCount[0].Item2 == 4)
            {
                //FourSome
                var kicker = sortedOnRank.First(t => t.Item2 != 4).Item1;
                return new FourSome(kicker);
            }

            if (sortedOnCount[0].Item2 == 3 && sortedOnCount[1].Item2 >= 2)
            {
                //FullHouse
                return new FullHouse(sortedOnCount[0].Item1, sortedOnCount[1].Item1);
            }

            if (sortedOnRank.Count >= 5)
            {
                var reducedRanks = sortedOnRank.Select(t => t.Item1).ToList();
                for (int i = 0; i <= reducedRanks.Count - 5; i++)
                {
                    if (reducedRanks[i] - reducedRanks[i + 4] == 4)
                    {
                        //Straight
                        return new Straight(reducedRanks[i]);
                    }
                }
            }

            if (sortedOnCount[0].Item2 == 3)
            {
                //ThreeSome
                return new ThreeSome(sortedOnCount[0].Item1, sortedOnCount[1].Item1, sortedOnCount[2].Item1);   
            }

            if (sortedOnCount[0].Item2 == 2 && sortedOnCount[1].Item2 == 2)
            {
                //TwoPairs
                return new TwoPairs(sortedOnCount[0].Item1, sortedOnCount[1].Item1, sortedOnCount[2].Item1);
            }

            if (sortedOnCount[0].Item2 == 2)
            {
                //OnePair
                return new OnePair(sortedOnCount[0].Item1, sortedOnCount[1].Item1, sortedOnCount[2].Item1, sortedOnCount[3].Item1);
            }

            return new HighCard(sortedOnCount[0].Item1, sortedOnCount[1].Item1, sortedOnCount[2].Item1, sortedOnCount[3].Item1, sortedOnCount[4].Item1);
        }
    }
}
