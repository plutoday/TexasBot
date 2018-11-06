namespace Turn
{
    public enum TurnBoardRankTextureEnum
    {
        Undefined,
        //straight, straght draw, set, two pairs, one pair, high card
        Singles,

        //straight, straight draw, high full house, middle full house, low full house, set, two pairs, one pair, high card
        HighPair,

        //straight, straight draw, high full house, middle full house, low full house, set, two pairs, one pair, high card
        MiddlePair,

        //straight, straight draw, high full house, middle full house, low full house, set, two pairs, one pair, high card
        LowPair,

        //high foursome, low foursome, high full house, low full house, set, two pairs, straight draw, one pair, high card
        TwoPairs,

        //foursome, high full house with over pair kicker, high full house with low pair kicker, high full house with under pair kicker
        //set with top kicker, set with good kicker, set with weak kicker, straight draw
        HighTri,

        //foursome, low full house with over pair kicker, low full house with high pair kicker, low full house with under pair kicker
        //set with top kicker, set with good kicker, set with weak kicker, straight draw
        LowTri,

        //top kicker, good kicker, weak kicker
        Foursome,
    }

    public enum FoursomeOutcomeEnum
    {
        FoursomeTopKicker,
        FoursomeGoodKicker,
        FoursomeWeakKicker,
    }

    public enum TriOutcomeEnum
    {
        Foursome,
        OverTriFullHouse,
        GoodOverFullHouse,
        OverFullHouse,
        FullHouse,
        UnderFullHouse,
        ThreesomeTopKicker,
        ThreesomeOverGoodKicker,
        ThreesomeOverWeakKicker,
        ThreesomeNoneKicker
    }

    public enum TwoPairsOutcomeEnum
    {
        HighFoursome,
        LowFoursome,
        HighFullHouse,
        LowFullHouse,
        OverTwoPairs,
        HighBetweenTwoPairs,
        TopKicker,
        GoodKicker,
        WeakKicker,
    }

    public enum HighPairOutcomeEnum
    {
        Foursome,
        HighFullHouseMiddlePair,
        HighFullHouseLowPair,
        MiddleFullHouse,
        LowFullHouse,
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        ThreesomeTopKicker,
        ThreesomeOverGoodKicker,
        ThreesomeOverWeakKicker,
        ThreesomeNoneKicker,
        OverTwoPairs,
        AboveMiddleTwoPairs,
        MiddleTwoPairs,
        AboveLowTwoPairs,
        LowTwoPairs,
        UnderTwoPairs,
        TopKicker,
        OverGoodKicker,
        OverWeakKicker,
        NoneKicker,
    }

    public enum MiddlePairOutcomeEnum
    {
        Foursome,
        HighFullHouse,
        MiddleFullHouseHighPair,
        MiddleFullHouseLowPair,
        LowFullHouse,
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        ThreesomeTopKicker,
        ThreesomeOverGoodKicker,
        ThreesomeOverWeakKicker,
        ThreesomeNoneKicker,
        OverTwoPairs,
        HighTwoPairs,
        AboveMiddleTwoPairs,
        AboveLowTwoPairs,
        LowTwoPairs,
        UnderTwoPairs,
        TopKicker,
        OverGoodKicker,
        OverWeakKicker,
        NoneKicker,
    }

    public enum LowPairOutcomeEnum
    {
        Foursome,
        HighFullHouse,
        MiddleFullHouse,
        LowFullHouseHighPair,
        LowFullHouseMiddlePair,
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        ThreesomeTopKicker,
        ThreesomeOverGoodKicker,
        ThreesomeOverWeakKicker,
        ThreesomeNoneKicker,
        OverTwoPairs,
        HighTwoPairs,
        AboveMiddleTwoPairs,
        MiddleTwoPairs,
        AboveLowTwoPairs,
        UnderTwoPairs,
        TopKicker,
        OverGoodKicker,
        OverWeakKicker,
        NoneKicker,
    }

    public enum SinglesOutcomeEnum
    {
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        HighSet,
        SecondSet,
        ThirdSet,
        LowSet,
        HighSecondTwoPairs,
        HighThirdTwoPairs,
        HighLowTwoPairs,
        SecondThirdTwoPairs,
        SecondLowTwoPairs,
        ThirdLowTwoPairs,
        OverTopPair,
        OverGoodPair,
        OverWeakPair,
        HighPairTopKicker,
        HighPairGoodKicker,
        HighPairWeakKicker,
        SecondPair,
        ThirdPair,
        LowPair,
        HighCard,
    }

    public enum TurnBoardSuitTextureEnum
    {
        //nothing
        Offsuit,

        //flush draw
        SuitedTwo,

        //flush draw
        SuitedTwoPairs,

        //flush, flush draw
        //kicker
        SuitedThree,

        //flush, flush draw
        //kicker
        SuitedFour,
    }

    /// <summary>
    /// RoyalFlush/StraightFlush/FlushWithTopKicker/FlushWithGoodKicker/FlushWithWeakKicker/FlushDraw/Nothing
    /// </summary>
    public enum SuitTextureOutcomeEnum
    {
        RoyalFlush,
        StraightFlush,
        FlushWithTopKicker,
        FlushWithGoodKicker,
        FlushWithWeakKicker,
        FlushDraw,
        Nothing
    }
}
