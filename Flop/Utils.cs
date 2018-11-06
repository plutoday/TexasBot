using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flop.Strategy;
using Flop.Strategy.Multiway;
using Models;
using Models.Ranging;

namespace Flop
{
    public static class Utils
    {

        public static RankDrawEnum TestRankDrawOnFlop(this RangeGrid grid, List<RankEnum> flopBoardRanks)
        {
            var flopRanksCloned = new List<RankEnum>(flopBoardRanks);
            flopRanksCloned.Sort();

            var lowRank = flopRanksCloned[0];
            var middleRank = flopRanksCloned[1];
            var highRank = flopRanksCloned[2];

            var ranks = new List<RankEnum>();
            ranks.Add(grid.HighRank);
            if (ranks.All(r => r != grid.LowRank)) ranks.Add(grid.LowRank);
            if (ranks.All(r => r != highRank)) ranks.Add(highRank);
            if (ranks.All(r => r != middleRank)) ranks.Add(middleRank);
            if (ranks.All(r => r != lowRank)) ranks.Add(lowRank);

            if (ranks.Count <= 3)
            {
                return RankDrawEnum.Nothing;
            }

            ranks.Sort();

            if (ranks.Count == 4)
            {
                //open draw, close draw, or high card
                List<RankEnum> openDrawRanks = new List<RankEnum>();
                if (ranks[0] + 1 == ranks[1] && ranks[1] + 1 == ranks[2] && ranks[2] + 1 == ranks[3])
                {
                    openDrawRanks.Add(ranks[0]);
                    openDrawRanks.Add(ranks[1]);
                    openDrawRanks.Add(ranks[2]);
                    openDrawRanks.Add(ranks[3]);
                }

                if (openDrawRanks.Any())
                {
                    //open draw
                    if (openDrawRanks.Count(r => (highRank == r || middleRank == r || lowRank == r)) == 2)
                    {
                        //two used
                        if (openDrawRanks[3] == highRank + 2) return RankDrawEnum.TwoUsedOpenDrawTwoOver;
                        if (openDrawRanks[3] == highRank + 1) return RankDrawEnum.TwoUsedOpenDrawOneOver;
                        if (openDrawRanks[3] == highRank) return RankDrawEnum.TwoUsedOpenDrawNoneOver;
                        throw new InvalidOperationException("Impossible to reach here");
                    }
                    if (openDrawRanks.Count(r => (highRank == r || middleRank == r || lowRank == r)) == 3)
                    {
                        if (openDrawRanks[3] == highRank + 1) return RankDrawEnum.OneUsedOpenDrawOneOver;
                        if (openDrawRanks[3] == highRank) return RankDrawEnum.OneUsedOpenDrawNoneOver;
                        throw new InvalidOperationException("Impossible to reach here");
                    }
                    throw new InvalidOperationException("Impossible to reach here");
                }

                //close draw
                List<RankEnum> closeDrawRanks = new List<RankEnum>();
                if (ranks[3] == ranks[0] + 4)
                {
                    closeDrawRanks.Add(ranks[0]);
                    closeDrawRanks.Add(ranks[1]);
                    closeDrawRanks.Add(ranks[2]);
                    closeDrawRanks.Add(ranks[3]);
                }

                if (closeDrawRanks.Any())
                {
                    //close draw
                    if (closeDrawRanks.Count(r => (highRank == r || middleRank == r || lowRank == r)) == 2)
                    {
                        //two used
                        if (closeDrawRanks[3] == highRank + 3) return RankDrawEnum.TwoUsedCloseDrawThreeOver;
                        if (closeDrawRanks[3] == highRank + 2) return RankDrawEnum.TwoUsedCloseDrawTwoOver;
                        if (closeDrawRanks[3] == highRank + 1) return RankDrawEnum.TwoUsedCloseDrawOneOver;
                        if (closeDrawRanks[3] == highRank) return RankDrawEnum.TwoUsedCloseDrawNoneOver;
                        throw new InvalidOperationException("Impossible to reach here");
                    }
                    if (closeDrawRanks.Count(r => (highRank == r || middleRank == r || lowRank == r)) == 3)
                    {
                        if (closeDrawRanks[3] == highRank + 2) return RankDrawEnum.OneUsedCloseDrawTwoOver;
                        if (closeDrawRanks[3] == highRank + 1) return RankDrawEnum.OneUsedCloseDrawOneOver;
                        if (closeDrawRanks[3] == highRank) return RankDrawEnum.OneUsedCloseDrawNoneOver;
                        throw new InvalidOperationException("Impossible to reach here");
                    }
                    throw new InvalidOperationException("Impossible to reach here");
                }

                return RankDrawEnum.Nothing;
            }
            else
            {
                //ranks.Count == 5

                //open draw, close draw, or high card
                List<RankEnum> openDrawRanks = new List<RankEnum>();
                if (ranks[0] + 1 == ranks[1] && ranks[1] + 1 == ranks[2] && ranks[2] + 1 == ranks[3])
                {
                    openDrawRanks.Add(ranks[0]);
                    openDrawRanks.Add(ranks[1]);
                    openDrawRanks.Add(ranks[2]);
                    openDrawRanks.Add(ranks[3]);
                }
                if (ranks[1] + 1 == ranks[2] && ranks[2] + 1 == ranks[3] && ranks[3] + 1 == ranks[4])
                {
                    openDrawRanks.Add(ranks[1]);
                    openDrawRanks.Add(ranks[2]);
                    openDrawRanks.Add(ranks[3]);
                    openDrawRanks.Add(ranks[4]);
                }

                if (openDrawRanks.Any())
                {
                    //open draw
                    if (openDrawRanks.Count(r => (highRank == r || middleRank == r || lowRank == r)) == 2)
                    {
                        //two used
                        if (openDrawRanks[3] == highRank + 2) return RankDrawEnum.TwoUsedOpenDrawTwoOver;
                        if (openDrawRanks[3] == highRank + 1) return RankDrawEnum.TwoUsedOpenDrawOneOver;
                        return RankDrawEnum.TwoUsedOpenDrawNoneOver;
                    }
                    if (openDrawRanks.Count(r => (highRank == r || middleRank == r || lowRank == r)) == 3)
                    {
                        if (openDrawRanks[3] == highRank + 1) return RankDrawEnum.OneUsedOpenDrawOneOver;
                        return RankDrawEnum.OneUsedOpenDrawNoneOver;
                    }
                    throw new InvalidOperationException("Impossible to reach here");
                }

                //close draw or high card
                List<RankEnum> closeDrawRanks = new List<RankEnum>();
                if (ranks.Count == 4 && ranks[4] == ranks[1] + 4)
                {
                    closeDrawRanks.Add(ranks[1]);
                    closeDrawRanks.Add(ranks[2]);
                    closeDrawRanks.Add(ranks[3]);
                    closeDrawRanks.Add(ranks[4]);
                }
                if (ranks[3] == ranks[0] + 4)
                {
                    closeDrawRanks.Add(ranks[0]);
                    closeDrawRanks.Add(ranks[1]);
                    closeDrawRanks.Add(ranks[2]);
                    closeDrawRanks.Add(ranks[3]);
                }

                if (closeDrawRanks.Any())
                {
                    //close draw
                    if (closeDrawRanks.Count(r => (highRank == r || middleRank == r || lowRank == r)) == 2)
                    {
                        //two used
                        if (closeDrawRanks[3] == highRank + 2) return RankDrawEnum.TwoUsedCloseDrawTwoOver;
                        if (closeDrawRanks[3] == highRank + 1) return RankDrawEnum.TwoUsedCloseDrawOneOver;
                        return RankDrawEnum.TwoUsedCloseDrawNoneOver;
                    }
                    if (closeDrawRanks.Count(r => (highRank == r || middleRank == r || lowRank == r)) == 3)
                    {
                        if (closeDrawRanks[3] == highRank + 1) return RankDrawEnum.OneUsedOpenDrawOneOver;
                        return RankDrawEnum.OneUsedOpenDrawNoneOver;
                    }
                    throw new InvalidOperationException("Impossible to reach here");
                }

                return RankDrawEnum.Nothing;
            }
        }

        #region Grade

        public static RankHandGradeEnum Grade(this Flop.SinglesOutcomeEnum singlesOutcome)
        {
            switch (singlesOutcome)
            {
                case Flop.SinglesOutcomeEnum.TwoUsedStraightTwoOver:
                case Flop.SinglesOutcomeEnum.TwoUsedStraightOneOver:
                case Flop.SinglesOutcomeEnum.TwoUsedStraightNoneOver:
                    return RankHandGradeEnum.Straight;
                case Flop.SinglesOutcomeEnum.HighSet:
                case Flop.SinglesOutcomeEnum.MiddleSet:
                case Flop.SinglesOutcomeEnum.LowSet:
                    return RankHandGradeEnum.Threesome;
                case Flop.SinglesOutcomeEnum.TopTwoPairs:
                case Flop.SinglesOutcomeEnum.TopBottomTwoPairs:
                case Flop.SinglesOutcomeEnum.BottomTwoPairs:
                    return RankHandGradeEnum.TwoPairs;
                case Flop.SinglesOutcomeEnum.OverPair:
                case Flop.SinglesOutcomeEnum.TopPair:
                case Flop.SinglesOutcomeEnum.OverMiddlePair:
                case Flop.SinglesOutcomeEnum.MiddlePair:
                case Flop.SinglesOutcomeEnum.UnderMiddlePair:
                case Flop.SinglesOutcomeEnum.LowPair:
                case Flop.SinglesOutcomeEnum.UnderPair:
                    return RankHandGradeEnum.OnePair;
                case Flop.SinglesOutcomeEnum.HighCard:
                case Flop.SinglesOutcomeEnum.TopHighCard:
                case Flop.SinglesOutcomeEnum.OverHighCard:
                    return RankHandGradeEnum.HighCard;

                default:
                    throw new InvalidOperationException();
            }
        }

        public static RankHandGradeEnum Grade(this Flop.LowPairOutcomeEnum singlesOutcome)
        {
            switch (singlesOutcome)
            {
                case Flop.LowPairOutcomeEnum.FourSome:
                    return RankHandGradeEnum.Foursome;
                case Flop.LowPairOutcomeEnum.HighFullHouse:
                case Flop.LowPairOutcomeEnum.LowFullHouse:
                    return RankHandGradeEnum.FullHouse;
                case Flop.LowPairOutcomeEnum.ThreeSomeTopKicker:
                case Flop.LowPairOutcomeEnum.ThreeSomeGoodKicker:
                case Flop.LowPairOutcomeEnum.ThreeSomeWeakKicker:
                    return RankHandGradeEnum.Threesome;
                case Flop.LowPairOutcomeEnum.OverTwoPairs:
                case Flop.LowPairOutcomeEnum.TopTwoPairsTopKicker:
                case Flop.LowPairOutcomeEnum.TopTwoPairsGoodKicker:
                case Flop.LowPairOutcomeEnum.TopTwoPairsWeakKicker:
                case Flop.LowPairOutcomeEnum.BetweenTwoPairs:
                case Flop.LowPairOutcomeEnum.UnderTwoPairs:
                    return RankHandGradeEnum.TwoPairs;
                case Flop.LowPairOutcomeEnum.OnePairTopKicker:
                case Flop.LowPairOutcomeEnum.OnePairGoodKicker:
                case Flop.LowPairOutcomeEnum.OnePairWeakKicker:
                    return RankHandGradeEnum.OnePair;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static RankHandGradeEnum Grade(this Flop.HighPairOutcomeEnum highPairOutcome)
        {
            switch (highPairOutcome)
            {
                case Flop.HighPairOutcomeEnum.FourSome:
                    return RankHandGradeEnum.Foursome;
                case Flop.HighPairOutcomeEnum.HighFullHouse:
                case Flop.HighPairOutcomeEnum.LowFullHouse:
                    return RankHandGradeEnum.FullHouse;
                case Flop.HighPairOutcomeEnum.ThreeSomeTopKicker:
                case Flop.HighPairOutcomeEnum.ThreeSomeGoodKicker:
                case Flop.HighPairOutcomeEnum.ThreeSomeWeakKicker:
                    return RankHandGradeEnum.Threesome;
                case Flop.HighPairOutcomeEnum.OverTwoPairs:
                case Flop.HighPairOutcomeEnum.BetweenTwoPairs:
                case Flop.HighPairOutcomeEnum.BottomTwoPairsTopKicker:
                case Flop.HighPairOutcomeEnum.BottomTwoPairsGoodKicker:
                case Flop.HighPairOutcomeEnum.BottomTwoPairsWeakKicker:
                case Flop.HighPairOutcomeEnum.UnderTwoPairs:
                    return RankHandGradeEnum.TwoPairs;
                case Flop.HighPairOutcomeEnum.OnePairTopKicker:
                case Flop.HighPairOutcomeEnum.OnePairGoodKicker:
                case Flop.HighPairOutcomeEnum.OnePairWeakKicker:
                    return RankHandGradeEnum.OnePair;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static RankHandGradeEnum Grade(this Flop.ThreesomeOutcomeEnum threesomeOutcome)
        {
            switch (threesomeOutcome)
            {
                case ThreesomeOutcomeEnum.FourSome:
                    return RankHandGradeEnum.Foursome;
                case ThreesomeOutcomeEnum.TopFullHouse:
                case ThreesomeOutcomeEnum.GoodFullHouse:
                case ThreesomeOutcomeEnum.WeakFullHouse:
                    return RankHandGradeEnum.FullHouse;
                case ThreesomeOutcomeEnum.ThreeSomeTopKicker:
                case ThreesomeOutcomeEnum.ThreeSomeGoodKicker:
                case ThreesomeOutcomeEnum.ThreeSomeWeakKicker:
                    return RankHandGradeEnum.Threesome;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static SuitHandGradeEnum Grade(this Flop.SuitTextureOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case Flop.SuitTextureOutcomeEnum.Nothing:
                    return SuitHandGradeEnum.Nothing;
                case Flop.SuitTextureOutcomeEnum.FlushDraw:
                    return SuitHandGradeEnum.FlushDraw;
                case Flop.SuitTextureOutcomeEnum.FlushWithGoodKicker:
                case Flop.SuitTextureOutcomeEnum.FlushWithTopKicker:
                case Flop.SuitTextureOutcomeEnum.FlushWithWeakKicker:
                    return SuitHandGradeEnum.Flush;
                case Flop.SuitTextureOutcomeEnum.StraightFlush:
                    return SuitHandGradeEnum.StraightFlush;
                case Flop.SuitTextureOutcomeEnum.RoyalFlush:
                    return SuitHandGradeEnum.RoyalFlush;
                default:
                    throw new InvalidOperationException();
            }
        }

        #endregion
    }
}
