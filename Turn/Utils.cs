using System;
using Flop;
using Models;

namespace Turn
{
    public static class Utils
    {
        public static RankHandGradeEnum Grade(this FoursomeOutcomeEnum outcome)
        {
            return RankHandGradeEnum.Foursome;
        }

        public static RankHandGradeEnum Grade(this TriOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case TriOutcomeEnum.Foursome:
                    return RankHandGradeEnum.Foursome;
                case TriOutcomeEnum.OverTriFullHouse:
                case TriOutcomeEnum.GoodOverFullHouse:
                case TriOutcomeEnum.OverFullHouse:
                case TriOutcomeEnum.FullHouse:
                case TriOutcomeEnum.UnderFullHouse:
                    return RankHandGradeEnum.FullHouse;
                case TriOutcomeEnum.ThreesomeTopKicker:
                case TriOutcomeEnum.ThreesomeOverGoodKicker:
                case TriOutcomeEnum.ThreesomeOverWeakKicker:
                case TriOutcomeEnum.ThreesomeNoneKicker:
                    return RankHandGradeEnum.Threesome;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static RankHandGradeEnum Grade(this TwoPairsOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case TwoPairsOutcomeEnum.HighFoursome:
                case TwoPairsOutcomeEnum.LowFoursome:
                    return RankHandGradeEnum.Foursome;
                case TwoPairsOutcomeEnum.HighFullHouse:
                case TwoPairsOutcomeEnum.LowFullHouse:
                    return RankHandGradeEnum.FullHouse;
                default:
                    return RankHandGradeEnum.TwoPairs;
            }
        }

        public static RankHandGradeEnum Grade(this HighPairOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case HighPairOutcomeEnum.Foursome:
                    return RankHandGradeEnum.Foursome;
                case HighPairOutcomeEnum.HighFullHouseMiddlePair:
                case HighPairOutcomeEnum.HighFullHouseLowPair:
                case HighPairOutcomeEnum.MiddleFullHouse:
                case HighPairOutcomeEnum.LowFullHouse:
                    return RankHandGradeEnum.FullHouse;
                case HighPairOutcomeEnum.StraightOverTwo:
                case HighPairOutcomeEnum.StraightOverOne:
                case HighPairOutcomeEnum.StraightOverNone:
                    return RankHandGradeEnum.Straight;
                case HighPairOutcomeEnum.ThreesomeTopKicker:
                case HighPairOutcomeEnum.ThreesomeOverGoodKicker:
                case HighPairOutcomeEnum.ThreesomeOverWeakKicker:
                case HighPairOutcomeEnum.ThreesomeNoneKicker:
                    return RankHandGradeEnum.Threesome;
                case HighPairOutcomeEnum.OverTwoPairs:
                case HighPairOutcomeEnum.AboveMiddleTwoPairs:
                case HighPairOutcomeEnum.MiddleTwoPairs:
                case HighPairOutcomeEnum.AboveLowTwoPairs:
                case HighPairOutcomeEnum.LowTwoPairs:
                case HighPairOutcomeEnum.UnderTwoPairs:
                    return RankHandGradeEnum.TwoPairs;
                default:
                    return RankHandGradeEnum.OnePair;
            }
        }

        public static RankHandGradeEnum Grade(this MiddlePairOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case MiddlePairOutcomeEnum.Foursome:
                    return RankHandGradeEnum.Foursome;
                case MiddlePairOutcomeEnum.HighFullHouse:
                case MiddlePairOutcomeEnum.MiddleFullHouseHighPair:
                case MiddlePairOutcomeEnum.MiddleFullHouseLowPair:
                case MiddlePairOutcomeEnum.LowFullHouse:
                    return RankHandGradeEnum.FullHouse;
                case MiddlePairOutcomeEnum.StraightOverTwo:
                case MiddlePairOutcomeEnum.StraightOverOne:
                case MiddlePairOutcomeEnum.StraightOverNone:
                    return RankHandGradeEnum.Straight;
                case MiddlePairOutcomeEnum.ThreesomeTopKicker:
                case MiddlePairOutcomeEnum.ThreesomeOverGoodKicker:
                case MiddlePairOutcomeEnum.ThreesomeOverWeakKicker:
                case MiddlePairOutcomeEnum.ThreesomeNoneKicker:
                    return RankHandGradeEnum.Threesome;
                case MiddlePairOutcomeEnum.OverTwoPairs:
                case MiddlePairOutcomeEnum.HighTwoPairs:
                case MiddlePairOutcomeEnum.AboveMiddleTwoPairs:
                case MiddlePairOutcomeEnum.AboveLowTwoPairs:
                case MiddlePairOutcomeEnum.LowTwoPairs:
                case MiddlePairOutcomeEnum.UnderTwoPairs:
                    return RankHandGradeEnum.TwoPairs;
                default:
                    return RankHandGradeEnum.OnePair;
            }
        }

        public static RankHandGradeEnum Grade(this LowPairOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case LowPairOutcomeEnum.Foursome:
                    return RankHandGradeEnum.Foursome;
                case LowPairOutcomeEnum.HighFullHouse:
                case LowPairOutcomeEnum.MiddleFullHouse:
                case LowPairOutcomeEnum.LowFullHouseHighPair:
                case LowPairOutcomeEnum.LowFullHouseMiddlePair:
                    return RankHandGradeEnum.FullHouse;
                case LowPairOutcomeEnum.StraightOverTwo:
                case LowPairOutcomeEnum.StraightOverOne:
                case LowPairOutcomeEnum.StraightOverNone:
                    return RankHandGradeEnum.Straight;
                case LowPairOutcomeEnum.ThreesomeTopKicker:
                case LowPairOutcomeEnum.ThreesomeOverGoodKicker:
                case LowPairOutcomeEnum.ThreesomeOverWeakKicker:
                case LowPairOutcomeEnum.ThreesomeNoneKicker:
                    return RankHandGradeEnum.Threesome;
                case LowPairOutcomeEnum.OverTwoPairs:
                case LowPairOutcomeEnum.HighTwoPairs:
                case LowPairOutcomeEnum.AboveMiddleTwoPairs:
                case LowPairOutcomeEnum.MiddleTwoPairs:
                case LowPairOutcomeEnum.AboveLowTwoPairs:
                case LowPairOutcomeEnum.UnderTwoPairs:
                    return RankHandGradeEnum.TwoPairs;
                default:
                    return RankHandGradeEnum.OnePair;
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
                case SinglesOutcomeEnum.LowSet:
                    return RankHandGradeEnum.Threesome;
                case SinglesOutcomeEnum.HighSecondTwoPairs:
                case SinglesOutcomeEnum.HighThirdTwoPairs:
                case SinglesOutcomeEnum.HighLowTwoPairs:
                case SinglesOutcomeEnum.SecondThirdTwoPairs:
                case SinglesOutcomeEnum.SecondLowTwoPairs:
                case SinglesOutcomeEnum.ThirdLowTwoPairs:
                    return RankHandGradeEnum.TwoPairs;
                case SinglesOutcomeEnum.OverTopPair:
                case SinglesOutcomeEnum.OverGoodPair:
                case SinglesOutcomeEnum.OverWeakPair:
                case SinglesOutcomeEnum.HighPairTopKicker:
                case SinglesOutcomeEnum.HighPairGoodKicker:
                case SinglesOutcomeEnum.HighPairWeakKicker:
                case SinglesOutcomeEnum.SecondPair:
                case SinglesOutcomeEnum.ThirdPair:
                case SinglesOutcomeEnum.LowPair:
                    return RankHandGradeEnum.OnePair;
                case SinglesOutcomeEnum.HighCard:
                    return RankHandGradeEnum.HighCard;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static SuitHandGradeEnum Grade(this SuitTextureOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case SuitTextureOutcomeEnum.Nothing:
                    return SuitHandGradeEnum.Nothing;
                case SuitTextureOutcomeEnum.FlushDraw:
                    return SuitHandGradeEnum.FlushDraw;
                case SuitTextureOutcomeEnum.FlushWithGoodKicker:
                case SuitTextureOutcomeEnum.FlushWithTopKicker:
                case SuitTextureOutcomeEnum.FlushWithWeakKicker:
                    return SuitHandGradeEnum.Flush;
                case SuitTextureOutcomeEnum.StraightFlush:
                    return SuitHandGradeEnum.StraightFlush;
                case SuitTextureOutcomeEnum.RoyalFlush:
                    return SuitHandGradeEnum.RoyalFlush;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
