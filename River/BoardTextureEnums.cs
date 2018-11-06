namespace River
{
    public enum RiverRankTextureEnum
    {
        Foursome,
        HighTriLowPair,
        LowTriHighPair,
        HighTri,
        MiddleTri,
        LowTri,
        HighTwoPairs,
        HighLowTwoPairs,
        LowTwoPairs,
        HighPair,
        SecondPair,
        ThirdPair,
        LowPair,
        Singles
    }

    public enum FoursomeOutcomeEnum
    {
        FoursomeTopKicker,
        FoursomeGoodKicker,
        FoursomeWeakKicker,
        FoursomeNoneKicker,
    }

    public enum HighTriLowPairOutcomeEnum
    {
        HighFoursome,
        LowFoursome,
        TopPairFullHouse,
        GoodOverPairFullHouse,
        WeakOverPairFullHouse,
        FullHouse,
    }

    public enum LowTriHighPairOutcomeEnum
    {
        HighFoursome,
        LowFoursome,
        HighFullHouse,
        TopPairFullHouse,
        GoodOverPairFullHouse,
        WeakOverPairFullHouse,
        FullHouse,
    }

    public enum HighTriOutcomeEnum
    {
        Foursome,
        TopOverPairFullHouse,
        GoodOverPairFullHouse,
        WeakOverPairFullHouse,
        MiddlePairFullHouse,
        LowPairFullHouse,
        UnderPairFullHouse,
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        TopKicker,
        GoodKicker,
        WeakKicker,
        NoneKicker,
    }

    public enum MiddleTriOutcomeEnum
    {
        Foursome,
        HighFullHouse,
        TopPairFullHouse,
        GoodPairFullHouse,
        WeakPairFullHouse,
        HighPairFullHouse,
        LowPairFullHouse,
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        TopKicker,
        GoodKicker,
        WeakKicker,
        NoneKicker,
    }

    public enum LowTriOutcomeEnum
    {
        Foursome,
        HighFullHouse,
        MiddleFullHouse,
        TopOverPairFullHouse,
        GoodOverPairFullHouse,
        WeakOverPairFullHouse,
        HighPairFullHouse,
        OverMiddlePairFullHouse,
        MiddlePairFullHouse,
        OverNonePairFullHouse,
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        TopKicker,
        GoodKicker,
        WeakKicker,
        NoneKicker,
    }

    public enum HighTwoPairsOutcomeEnum
    {
        HighFoursome,
        MiddleFoursome,
        HighFullHouse,
        MiddleFullHouse,
        LowFullHouse,
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        OverTopTwoPairs,
        OverGoodTwoPairs,
        OverWeakTwoPairs,
        TopKicker,
        GoodKicker,
        WeakKicker,
        NoneKicker,
    }

    public enum HighLowTwoPairsOutcomeEnum
    {
        HighFoursome,
        LowFoursome,
        HighMiddleFullHouse,
        HighFullHouse,
        MiddleFullHouse,
        LowFullHouse,
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        OverTopTwoPairs,
        OverGoodTwoPairs,
        OverWeakTwoPairs,
        OverLowTwoPairs,
        HighTwoPairsTopKicker,
        HighTwoPairsGoodKicker,
        HighTwoPairsWeakKicker,
        HighTwoPairsNoneKicker,
        TopKicker,
        GoodKicker,
        WeakKicker,
        NoneKicker,
    }

    public enum LowTwoPairsOutcomeEnum
    {
        MiddleFoursome,
        LowFoursome,
        HighFullHouse,
        MiddleFullHouse,
        LowFullHouse,
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        OverTopTwoPairs,
        OverGoodTwoPairs,
        OverWeakTwoPairs,
        HighTwoPairs,
        TopKicker,
        GoodKicker,
        WeakKicker,
        NoneKicker,
    }

    public enum HighPairOutcomeEnum
    {
        Foursome,
        HighSecondFullHouse,
        HighThirdFullHouse,
        HighLowFullHouse,
        SecondHighFullHouse,
        ThirdHighFullHouse,
        LowHighFullHouse,
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        ThreesomeTopKicker,
        ThreesomeGoodKicker,
        ThreesomeWeakKicker,
        ThreesomeNoneKicker,
        OverTwoPairs,
        OverSecondTwoPairs,
        OverThirdTwoPairs,
        OverLowTwoPairs,
        UnderTwoPairs,
        SecondTwoPairsTopKicker,
        SecondTwoPairsGoodKicker,
        SecondTwoPairsWeakKicker,
        SecondTwoPairsNoneKicker,
        ThirdTwoPairs,
        LowTwoPairs,
        TopKicker,
        GoodKicker,
        WeakKicker,
        NoneKicker
    }

    public enum SecondPairOutcomeEnum
    {
        Foursome,
        HighSecondFullHouse,
        SecondHighFullHouse,
        SecondThirdFullHouse,
        SecondLowFullHouse,
        ThirdSecondFullHouse,
        LowSecondFullHouse,
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        ThreesomeTopKicker,
        ThreesomeGoodKicker,
        ThreesomeWeakKicker,
        ThreesomeNoneKicker,
        OverTwoPairs,
        OverSecondTwoPairs,
        OverThirdTwoPairs,
        OverLowTwoPairs,
        UnderTwoPairs,
        HighTwoPairsTopKicker,
        HighTwoPairsGoodKicker,
        HighTwoPairsWeakKicker,
        HighTwoPairsNoneKicker,
        ThirdTwoPairs,
        LowTwoPairs,
        TopKicker,
        GoodKicker,
        WeakKicker,
        NoneKicker
    }

    public enum ThirdPairOutcomeEnum
    {
        Foursome,
        HighThirdFullHouse,
        SecondThirdFullHouse,
        ThirdHighFullHouse,
        ThirdSecondFullHouse,
        ThirdLowFullHouse,
        LowThirdFullHouse,
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        ThreesomeTopKicker,
        ThreesomeGoodKicker,
        ThreesomeWeakKicker,
        ThreesomeNoneKicker,
        OverTwoPairs,
        OverSecondTwoPairs,
        OverThirdTwoPairs,
        OverLowTwoPairs,
        UnderTwoPairs,
        HighTwoPairsTopKicker,
        HighTwoPairsGoodKicker,
        HighTwoPairsWeakKicker,
        HighTwoPairsNoneKicker,
        SecondTwoPairs,
        LowTwoPairs,
        TopKicker,
        GoodKicker,
        WeakKicker,
        NoneKicker
    }

    public enum LowPairOutcomeEnum
    {
        Foursome,
        HighLowFullHouse,
        SecondLowFullHouse,
        ThirdLowFullHouse,
        LowHighFullHouse,
        LowSecondFullHouse,
        LowThirdFullHouse,
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        ThreesomeTopKicker,
        ThreesomeGoodKicker,
        ThreesomeWeakKicker,
        ThreesomeNoneKicker,
        OverTwoPairs,
        OverSecondTwoPairs,
        OverThirdTwoPairs,
        OverLowTwoPairs,
        UnderTwoPairs,
        HighTwoPairsTopKicker,
        HighTwoPairsGoodKicker,
        HighTwoPairsWeakKicker,
        HighTwoPairsNoneKicker,
        SecondTwoPairs,
        ThirdTwoPairs,
        TopKicker,
        GoodKicker,
        WeakKicker,
        NoneKicker
    }

    public enum SinglesOutcomeEnum
    {
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        HighSet,
        SecondSet,
        ThirdSet,
        FourthSet,
        LowSet,
        HighSecondTwoPairs,
        HighThirdTwoPairs,
        HighFourthTwoPairs,
        HighLowTwoPairs,
        SecondThirdTwoPairs,
        SecondFourthTwoPairs,
        SecondLowTwoPairs,
        ThirdFourthTwoPairs,
        ThirdLowTwoPairs,
        FourthLowTwoPairs,
        OverTopPair,
        OverGoodPair,
        OverWeakPair,
        OverSecondPair,
        OverThirdPair,
        OverFourthPair,
        OverLowPair,
        UnderPair,
        HighPair,
        SecondPair,
        ThirdPair,
        FourthPair,
        LowPair,
        TopKicker,
        GoodKicker,
        WeakKicker,
        NoneKicker
    }

    public enum RiverSuitTextureEnum
    {
        FiveSuited,
        FourSuited,
        ThreeSuited,
        Offsuit
    }

    public enum FourSuitedOutcomeEnum
    {
        RoyalFlush,
        StraightFlush,
        Flush,
        None
    }

    public enum ThreeSuitedOutcomeEnum
    {
        RoyalFlush,
        StraightFlush,
        Flush,
        None
    }

    public enum OffsuitOutcomeEnum
    {
        None
    }
}
