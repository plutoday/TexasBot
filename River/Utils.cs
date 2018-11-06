using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace River
{
    public static class Utils
    {
        /// <summary>
        /// Over-Used
        /// </summary>
        /// <param name="boardRanks"></param>
        /// <param name="playerRanks"></param>
        /// <returns></returns>
        public static Tuple<int, int> FindStraight(this List<RankEnum> boardRanks, List<RankEnum> playerRanks)
        {
            var boardHighRank = boardRanks.Max();
            var ranks = new List<RankEnum>();

            foreach (var playerRank in playerRanks)
            {
                if (ranks.All(r => r != playerRank))
                {
                    ranks.Add(playerRank);
                }
            }

            ranks.Sort();

            RankEnum highRank = RankEnum.Undefined;
            RankEnum lowRank = RankEnum.Undefined;

            if (ranks.Count >= 7 && ranks[6] == ranks[2] + 4)
            {
                //23456
                highRank = ranks[6];
                lowRank = ranks[5];
            }
            else if (ranks.Count >= 6 && ranks[5] == ranks[1] + 4)
            {
                //12345
                highRank = ranks[5];
                lowRank = ranks[1];
            }
            else if (ranks.Count >= 5 && ranks[4] == ranks[0] + 4)
            {
                //01234
                highRank = ranks[4];
                lowRank = ranks[0];
            }

            if (highRank != RankEnum.Undefined)
            {
                int used = 5 - boardRanks.Count(r => r >= lowRank && r <= highRank);
                if (highRank == boardHighRank + 2) return new Tuple<int, int>(2, used);
                if (highRank == boardHighRank + 1) return new Tuple<int, int>(1, used);
                return new Tuple<int, int>(0, used);
            }

            return null;
        }

        public static RankHandGradeEnum Grade(this FoursomeOutcomeEnum outcome)
        {
            return RankHandGradeEnum.Foursome;
        }

        public static RankHandGradeEnum Grade(this HighTriLowPairOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case HighTriLowPairOutcomeEnum.HighFoursome:
                case HighTriLowPairOutcomeEnum.LowFoursome:
                    return RankHandGradeEnum.Foursome;
                default:
                    return RankHandGradeEnum.FullHouse;
            }
        }

        public static RankHandGradeEnum Grade(this LowTriHighPairOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case LowTriHighPairOutcomeEnum.HighFoursome:
                case LowTriHighPairOutcomeEnum.LowFoursome:
                    return RankHandGradeEnum.Foursome;
                default:
                    return RankHandGradeEnum.FullHouse;
            }
        }

        public static RankHandGradeEnum Grade(this HighTriOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case HighTriOutcomeEnum.Foursome:
                    return RankHandGradeEnum.Foursome;
                case HighTriOutcomeEnum.TopOverPairFullHouse:
                case HighTriOutcomeEnum.GoodOverPairFullHouse:
                case HighTriOutcomeEnum.WeakOverPairFullHouse:
                case HighTriOutcomeEnum.MiddlePairFullHouse:
                case HighTriOutcomeEnum.LowPairFullHouse:
                case HighTriOutcomeEnum.UnderPairFullHouse:
                    return RankHandGradeEnum.FullHouse;
                case HighTriOutcomeEnum.StraightOverTwo:
                case HighTriOutcomeEnum.StraightOverOne:
                case HighTriOutcomeEnum.StraightOverNone:
                    return RankHandGradeEnum.Straight;
                default:
                    return RankHandGradeEnum.Threesome;
            }
        }

        public static RankHandGradeEnum Grade(this MiddleTriOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case MiddleTriOutcomeEnum.Foursome:
                    return RankHandGradeEnum.Foursome;
                case MiddleTriOutcomeEnum.HighFullHouse:
                case MiddleTriOutcomeEnum.TopPairFullHouse:
                case MiddleTriOutcomeEnum.GoodPairFullHouse:
                case MiddleTriOutcomeEnum.WeakPairFullHouse:
                case MiddleTriOutcomeEnum.HighPairFullHouse:
                case MiddleTriOutcomeEnum.LowPairFullHouse:
                    return RankHandGradeEnum.FullHouse;
                case MiddleTriOutcomeEnum.StraightOverTwo:
                case MiddleTriOutcomeEnum.StraightOverOne:
                case MiddleTriOutcomeEnum.StraightOverNone:
                    return RankHandGradeEnum.Straight;
                case MiddleTriOutcomeEnum.TopKicker:
                case MiddleTriOutcomeEnum.GoodKicker:
                case MiddleTriOutcomeEnum.WeakKicker:
                case MiddleTriOutcomeEnum.NoneKicker:
                    return RankHandGradeEnum.Threesome;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static RankHandGradeEnum Grade(this LowTriOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case LowTriOutcomeEnum.Foursome:
                    return RankHandGradeEnum.Foursome;
                case LowTriOutcomeEnum.HighFullHouse:
                case LowTriOutcomeEnum.MiddleFullHouse:
                case LowTriOutcomeEnum.TopOverPairFullHouse:
                case LowTriOutcomeEnum.GoodOverPairFullHouse:
                case LowTriOutcomeEnum.WeakOverPairFullHouse:
                case LowTriOutcomeEnum.HighPairFullHouse:
                case LowTriOutcomeEnum.OverMiddlePairFullHouse:
                case LowTriOutcomeEnum.MiddlePairFullHouse:
                case LowTriOutcomeEnum.OverNonePairFullHouse:
                    return RankHandGradeEnum.FullHouse;
                case LowTriOutcomeEnum.StraightOverTwo:
                case LowTriOutcomeEnum.StraightOverOne:
                case LowTriOutcomeEnum.StraightOverNone:
                    return RankHandGradeEnum.Straight;
                case LowTriOutcomeEnum.TopKicker:
                case LowTriOutcomeEnum.GoodKicker:
                case LowTriOutcomeEnum.WeakKicker:
                case LowTriOutcomeEnum.NoneKicker:
                    return RankHandGradeEnum.Threesome;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static RankHandGradeEnum Grade(this HighTwoPairsOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case HighTwoPairsOutcomeEnum.HighFoursome:
                case HighTwoPairsOutcomeEnum.MiddleFoursome:
                    return RankHandGradeEnum.Foursome;
                case HighTwoPairsOutcomeEnum.HighFullHouse:
                case HighTwoPairsOutcomeEnum.MiddleFullHouse:
                case HighTwoPairsOutcomeEnum.LowFullHouse:
                    return RankHandGradeEnum.FullHouse;
                case HighTwoPairsOutcomeEnum.StraightOverTwo:
                case HighTwoPairsOutcomeEnum.StraightOverOne:
                case HighTwoPairsOutcomeEnum.StraightOverNone:
                    return RankHandGradeEnum.Straight;
                case HighTwoPairsOutcomeEnum.OverTopTwoPairs:
                case HighTwoPairsOutcomeEnum.OverGoodTwoPairs:
                case HighTwoPairsOutcomeEnum.OverWeakTwoPairs:
                case HighTwoPairsOutcomeEnum.TopKicker:
                case HighTwoPairsOutcomeEnum.GoodKicker:
                case HighTwoPairsOutcomeEnum.WeakKicker:
                case HighTwoPairsOutcomeEnum.NoneKicker:
                    return RankHandGradeEnum.TwoPairs;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static RankHandGradeEnum Grade(this HighLowTwoPairsOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case HighLowTwoPairsOutcomeEnum.HighFoursome:
                case HighLowTwoPairsOutcomeEnum.LowFoursome:
                    return RankHandGradeEnum.Foursome;
                case HighLowTwoPairsOutcomeEnum.HighMiddleFullHouse:
                case HighLowTwoPairsOutcomeEnum.HighFullHouse:
                case HighLowTwoPairsOutcomeEnum.MiddleFullHouse:
                case HighLowTwoPairsOutcomeEnum.LowFullHouse:
                    return RankHandGradeEnum.FullHouse;
                case HighLowTwoPairsOutcomeEnum.StraightOverTwo:
                case HighLowTwoPairsOutcomeEnum.StraightOverOne:
                case HighLowTwoPairsOutcomeEnum.StraightOverNone:
                    return RankHandGradeEnum.Straight;
                case HighLowTwoPairsOutcomeEnum.OverTopTwoPairs:
                case HighLowTwoPairsOutcomeEnum.OverGoodTwoPairs:
                case HighLowTwoPairsOutcomeEnum.OverWeakTwoPairs:
                case HighLowTwoPairsOutcomeEnum.OverLowTwoPairs:
                case HighLowTwoPairsOutcomeEnum.HighTwoPairsTopKicker:
                case HighLowTwoPairsOutcomeEnum.HighTwoPairsGoodKicker:
                case HighLowTwoPairsOutcomeEnum.HighTwoPairsWeakKicker:
                case HighLowTwoPairsOutcomeEnum.HighTwoPairsNoneKicker:
                case HighLowTwoPairsOutcomeEnum.TopKicker:
                case HighLowTwoPairsOutcomeEnum.GoodKicker:
                case HighLowTwoPairsOutcomeEnum.WeakKicker:
                case HighLowTwoPairsOutcomeEnum.NoneKicker:
                    return RankHandGradeEnum.TwoPairs;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static RankHandGradeEnum Grade(this LowTwoPairsOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case LowTwoPairsOutcomeEnum.MiddleFoursome:
                case LowTwoPairsOutcomeEnum.LowFoursome:
                    return RankHandGradeEnum.Foursome;
                case LowTwoPairsOutcomeEnum.HighFullHouse:
                case LowTwoPairsOutcomeEnum.MiddleFullHouse:
                case LowTwoPairsOutcomeEnum.LowFullHouse:
                    return RankHandGradeEnum.FullHouse;
                case LowTwoPairsOutcomeEnum.StraightOverTwo:
                case LowTwoPairsOutcomeEnum.StraightOverOne:
                case LowTwoPairsOutcomeEnum.StraightOverNone:
                    return RankHandGradeEnum.Straight;
                case LowTwoPairsOutcomeEnum.OverTopTwoPairs:
                case LowTwoPairsOutcomeEnum.OverGoodTwoPairs:
                case LowTwoPairsOutcomeEnum.OverWeakTwoPairs:
                case LowTwoPairsOutcomeEnum.HighTwoPairs:
                case LowTwoPairsOutcomeEnum.TopKicker:
                case LowTwoPairsOutcomeEnum.GoodKicker:
                case LowTwoPairsOutcomeEnum.WeakKicker:
                case LowTwoPairsOutcomeEnum.NoneKicker:
                    return RankHandGradeEnum.TwoPairs;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static RankHandGradeEnum Grade(this HighPairOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case HighPairOutcomeEnum.Foursome:
                    return RankHandGradeEnum.Foursome;
                case HighPairOutcomeEnum.HighSecondFullHouse:
                case HighPairOutcomeEnum.HighThirdFullHouse:
                case HighPairOutcomeEnum.HighLowFullHouse:
                case HighPairOutcomeEnum.SecondHighFullHouse:
                case HighPairOutcomeEnum.ThirdHighFullHouse:
                case HighPairOutcomeEnum.LowHighFullHouse:
                    return RankHandGradeEnum.FullHouse;
                case HighPairOutcomeEnum.StraightOverTwo:
                case HighPairOutcomeEnum.StraightOverOne:
                case HighPairOutcomeEnum.StraightOverNone:
                    return RankHandGradeEnum.Straight;
                case HighPairOutcomeEnum.ThreesomeTopKicker:
                case HighPairOutcomeEnum.ThreesomeGoodKicker:
                case HighPairOutcomeEnum.ThreesomeWeakKicker:
                case HighPairOutcomeEnum.ThreesomeNoneKicker:
                    return RankHandGradeEnum.Threesome;
                case HighPairOutcomeEnum.OverTwoPairs:
                case HighPairOutcomeEnum.OverSecondTwoPairs:
                case HighPairOutcomeEnum.OverThirdTwoPairs:
                case HighPairOutcomeEnum.OverLowTwoPairs:
                case HighPairOutcomeEnum.UnderTwoPairs:
                case HighPairOutcomeEnum.SecondTwoPairsTopKicker:
                case HighPairOutcomeEnum.SecondTwoPairsGoodKicker:
                case HighPairOutcomeEnum.SecondTwoPairsWeakKicker:
                case HighPairOutcomeEnum.SecondTwoPairsNoneKicker:
                case HighPairOutcomeEnum.ThirdTwoPairs:
                case HighPairOutcomeEnum.LowTwoPairs:
                    return RankHandGradeEnum.TwoPairs;
                case HighPairOutcomeEnum.TopKicker:
                case HighPairOutcomeEnum.GoodKicker:
                case HighPairOutcomeEnum.WeakKicker:
                case HighPairOutcomeEnum.NoneKicker:
                    return RankHandGradeEnum.OnePair;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static RankHandGradeEnum Grade(this SecondPairOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case SecondPairOutcomeEnum.Foursome:
                    return RankHandGradeEnum.Foursome;
                case SecondPairOutcomeEnum.HighSecondFullHouse:
                case SecondPairOutcomeEnum.SecondHighFullHouse:
                case SecondPairOutcomeEnum.SecondThirdFullHouse:
                case SecondPairOutcomeEnum.SecondLowFullHouse:
                case SecondPairOutcomeEnum.ThirdSecondFullHouse:
                case SecondPairOutcomeEnum.LowSecondFullHouse:
                    return RankHandGradeEnum.FullHouse;
                case SecondPairOutcomeEnum.StraightOverTwo:
                case SecondPairOutcomeEnum.StraightOverOne:
                case SecondPairOutcomeEnum.StraightOverNone:
                    return RankHandGradeEnum.Straight;
                case SecondPairOutcomeEnum.ThreesomeTopKicker:
                case SecondPairOutcomeEnum.ThreesomeGoodKicker:
                case SecondPairOutcomeEnum.ThreesomeWeakKicker:
                case SecondPairOutcomeEnum.ThreesomeNoneKicker:
                    return RankHandGradeEnum.Threesome;
                case SecondPairOutcomeEnum.OverTwoPairs:
                case SecondPairOutcomeEnum.OverSecondTwoPairs:
                case SecondPairOutcomeEnum.OverThirdTwoPairs:
                case SecondPairOutcomeEnum.OverLowTwoPairs:
                case SecondPairOutcomeEnum.UnderTwoPairs:
                case SecondPairOutcomeEnum.HighTwoPairsTopKicker:
                case SecondPairOutcomeEnum.HighTwoPairsGoodKicker:
                case SecondPairOutcomeEnum.HighTwoPairsWeakKicker:
                case SecondPairOutcomeEnum.HighTwoPairsNoneKicker:
                case SecondPairOutcomeEnum.ThirdTwoPairs:
                case SecondPairOutcomeEnum.LowTwoPairs:
                    return RankHandGradeEnum.TwoPairs;
                case SecondPairOutcomeEnum.TopKicker:
                case SecondPairOutcomeEnum.GoodKicker:
                case SecondPairOutcomeEnum.WeakKicker:
                case SecondPairOutcomeEnum.NoneKicker:
                    return RankHandGradeEnum.OnePair;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static RankHandGradeEnum Grade(this ThirdPairOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case ThirdPairOutcomeEnum.Foursome:
                    return RankHandGradeEnum.Foursome;
                case ThirdPairOutcomeEnum.HighThirdFullHouse:
                case ThirdPairOutcomeEnum.SecondThirdFullHouse:
                case ThirdPairOutcomeEnum.ThirdHighFullHouse:
                case ThirdPairOutcomeEnum.ThirdSecondFullHouse:
                case ThirdPairOutcomeEnum.ThirdLowFullHouse:
                case ThirdPairOutcomeEnum.LowThirdFullHouse:
                    return RankHandGradeEnum.FullHouse;
                case ThirdPairOutcomeEnum.StraightOverTwo:
                case ThirdPairOutcomeEnum.StraightOverOne:
                case ThirdPairOutcomeEnum.StraightOverNone:
                    return RankHandGradeEnum.Straight;
                case ThirdPairOutcomeEnum.ThreesomeTopKicker:
                case ThirdPairOutcomeEnum.ThreesomeGoodKicker:
                case ThirdPairOutcomeEnum.ThreesomeWeakKicker:
                case ThirdPairOutcomeEnum.ThreesomeNoneKicker:
                    return RankHandGradeEnum.Threesome;
                case ThirdPairOutcomeEnum.OverTwoPairs:
                case ThirdPairOutcomeEnum.OverSecondTwoPairs:
                case ThirdPairOutcomeEnum.OverThirdTwoPairs:
                case ThirdPairOutcomeEnum.OverLowTwoPairs:
                case ThirdPairOutcomeEnum.UnderTwoPairs:
                case ThirdPairOutcomeEnum.HighTwoPairsTopKicker:
                case ThirdPairOutcomeEnum.HighTwoPairsGoodKicker:
                case ThirdPairOutcomeEnum.HighTwoPairsWeakKicker:
                case ThirdPairOutcomeEnum.HighTwoPairsNoneKicker:
                case ThirdPairOutcomeEnum.SecondTwoPairs:
                case ThirdPairOutcomeEnum.LowTwoPairs:
                    return RankHandGradeEnum.TwoPairs;
                case ThirdPairOutcomeEnum.TopKicker:
                case ThirdPairOutcomeEnum.GoodKicker:
                case ThirdPairOutcomeEnum.WeakKicker:
                case ThirdPairOutcomeEnum.NoneKicker:
                    return RankHandGradeEnum.OnePair;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static RankHandGradeEnum Grade(this LowPairOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case LowPairOutcomeEnum.Foursome:
                    return RankHandGradeEnum.Foursome;
                case LowPairOutcomeEnum.HighLowFullHouse:
                case LowPairOutcomeEnum.SecondLowFullHouse:
                case LowPairOutcomeEnum.ThirdLowFullHouse:
                case LowPairOutcomeEnum.LowHighFullHouse:
                case LowPairOutcomeEnum.LowSecondFullHouse:
                case LowPairOutcomeEnum.LowThirdFullHouse:
                    return RankHandGradeEnum.FullHouse;
                case LowPairOutcomeEnum.StraightOverTwo:
                case LowPairOutcomeEnum.StraightOverOne:
                case LowPairOutcomeEnum.StraightOverNone:
                    return RankHandGradeEnum.Straight;
                case LowPairOutcomeEnum.ThreesomeTopKicker:
                case LowPairOutcomeEnum.ThreesomeGoodKicker:
                case LowPairOutcomeEnum.ThreesomeWeakKicker:
                case LowPairOutcomeEnum.ThreesomeNoneKicker:
                    return RankHandGradeEnum.Threesome;
                case LowPairOutcomeEnum.OverTwoPairs:
                case LowPairOutcomeEnum.OverSecondTwoPairs:
                case LowPairOutcomeEnum.OverThirdTwoPairs:
                case LowPairOutcomeEnum.OverLowTwoPairs:
                case LowPairOutcomeEnum.UnderTwoPairs:
                case LowPairOutcomeEnum.HighTwoPairsTopKicker:
                case LowPairOutcomeEnum.HighTwoPairsGoodKicker:
                case LowPairOutcomeEnum.HighTwoPairsWeakKicker:
                case LowPairOutcomeEnum.HighTwoPairsNoneKicker:
                case LowPairOutcomeEnum.SecondTwoPairs:
                case LowPairOutcomeEnum.ThirdTwoPairs:
                    return RankHandGradeEnum.TwoPairs;
                case LowPairOutcomeEnum.TopKicker:
                case LowPairOutcomeEnum.GoodKicker:
                case LowPairOutcomeEnum.WeakKicker:
                case LowPairOutcomeEnum.NoneKicker:
                    return RankHandGradeEnum.OnePair;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static RankHandGradeEnum Grade(this SinglesOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case SinglesOutcomeEnum.StraightOverTwo:
                case SinglesOutcomeEnum.StraightOverOne:
                case SinglesOutcomeEnum.StraightOverNone:
                    return RankHandGradeEnum.Straight;
                case SinglesOutcomeEnum.HighSet:
                case SinglesOutcomeEnum.SecondSet:
                case SinglesOutcomeEnum.ThirdSet:
                case SinglesOutcomeEnum.FourthSet:
                case SinglesOutcomeEnum.LowSet:
                    return RankHandGradeEnum.Threesome;
                case SinglesOutcomeEnum.HighSecondTwoPairs:
                case SinglesOutcomeEnum.HighThirdTwoPairs:
                case SinglesOutcomeEnum.HighFourthTwoPairs:
                case SinglesOutcomeEnum.HighLowTwoPairs:
                case SinglesOutcomeEnum.SecondThirdTwoPairs:
                case SinglesOutcomeEnum.SecondFourthTwoPairs:
                case SinglesOutcomeEnum.SecondLowTwoPairs:
                case SinglesOutcomeEnum.ThirdFourthTwoPairs:
                case SinglesOutcomeEnum.ThirdLowTwoPairs:
                case SinglesOutcomeEnum.FourthLowTwoPairs:
                    return RankHandGradeEnum.TwoPairs;
                case SinglesOutcomeEnum.OverTopPair:
                case SinglesOutcomeEnum.OverGoodPair:
                case SinglesOutcomeEnum.OverWeakPair:
                case SinglesOutcomeEnum.OverSecondPair:
                case SinglesOutcomeEnum.OverThirdPair:
                case SinglesOutcomeEnum.OverFourthPair:
                case SinglesOutcomeEnum.OverLowPair:
                case SinglesOutcomeEnum.UnderPair:
                case SinglesOutcomeEnum.HighPair:
                case SinglesOutcomeEnum.SecondPair:
                case SinglesOutcomeEnum.ThirdPair:
                case SinglesOutcomeEnum.FourthPair:
                case SinglesOutcomeEnum.LowPair:
                    return RankHandGradeEnum.OnePair;
                case SinglesOutcomeEnum.TopKicker:
                case SinglesOutcomeEnum.GoodKicker:
                case SinglesOutcomeEnum.WeakKicker:
                case SinglesOutcomeEnum.NoneKicker:
                    return RankHandGradeEnum.HighCard;
                default:
                    throw new InvalidOperationException();
            }
        }



        public static SuitHandGradeEnum Grade(this River.SuitTextureOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case SuitTextureOutcomeEnum.RoyalFlush:
                    return SuitHandGradeEnum.RoyalFlush;
                case SuitTextureOutcomeEnum.StraightFlush:
                    return SuitHandGradeEnum.StraightFlush;
                case SuitTextureOutcomeEnum.FlushWithTopKicker:
                case SuitTextureOutcomeEnum.FlushWithGoodKicker:
                case SuitTextureOutcomeEnum.FlushWithWeakKicker:
                case SuitTextureOutcomeEnum.FlushWithNoneKicker:
                    return SuitHandGradeEnum.Flush;
                case SuitTextureOutcomeEnum.Nothing:
                    return SuitHandGradeEnum.Nothing;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
