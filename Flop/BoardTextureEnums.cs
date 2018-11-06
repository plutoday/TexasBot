namespace Flop
{
    public enum FlopBoardRankTextureEnum
    {
        /*
        Unconnected,    //27Q
        Connectors,     //45Q
        CloseStraightable,   //468/458
        OpenStraightable,    //457/467/468/456 
        */
        Singles,
        LowPair,    //66J
        HighPair,   //6JJ
        ThreeSome,  //JJJ
    }

    public enum SinglesOutcomeEnum
    {
        TwoUsedStraightTwoOver,
        TwoUsedStraightOneOver,
        TwoUsedStraightNoneOver,

        HighSet,
        MiddleSet,
        LowSet,
        TopTwoPairs,
        TopBottomTwoPairs,
        BottomTwoPairs,

        OverPair,
        TopPair,
        OverMiddlePair,
        MiddlePair,
        UnderMiddlePair,
        LowPair,
        UnderPair,

        TopHighCard,
        OverHighCard,
        HighCard
    }

    public enum RankDrawEnum
    {
        TwoUsedOpenDrawTwoOver,
        TwoUsedOpenDrawOneOver,
        TwoUsedOpenDrawNoneOver,
        OneUsedOpenDrawOneOver,
        OneUsedOpenDrawNoneOver,

        TwoUsedCloseDrawThreeOver,
        TwoUsedCloseDrawTwoOver,
        TwoUsedCloseDrawOneOver,
        TwoUsedCloseDrawNoneOver,

        OneUsedCloseDrawTwoOver,
        OneUsedCloseDrawOneOver,
        OneUsedCloseDrawNoneOver,

        Nothing,
    }

    /*
    //Q82
    public enum UnconnectedOutcomeEnum
    {
        TopSet,     //QQ
        MiddleSet,  //88
        BottomSet,  //22
        TopTwoPairs,    //Q8
        TopBottomTwoPairs,  //Q2
        BottomTwoPairs, //82
        TopOverPair,    //AA
        OverPair,   //KK
        TopPairTopKicker,    //AQ
        TopPairGoodKicker,  //QJ
        TopPairWeakKicker,  //Q7
        MiddlePair, //J8
        BottomPair, //52
        UnderPair,  //
        OverCard,   //AK
        HighCard    //JT
    }

    //Q54
    public enum ConnectorsOutcomeEnum
    {
        TopSet,     //QQ
        MiddleSet,  //55
        BottomSet,  //44
        TopTwoPairs,    //Q5
        TopBottomTwoPairs,  //Q4
        BottomTwoPairs, //54
        TopOverPair,    //AA
        OverPair,   //KK
        TopPairTopKicker,    //AQ
        OpenDraw,   //76
        CloseDraw,  //73
        TopPairGoodKicker,  //QJ
        TopPairWeakKicker,  //Q7
        MiddlePair, //85
        BottomPair, //84
        UnderPair,  //33
        OverCard,   //AK
        HighCard    //JT
    }

    //Q62
    public enum StraightDrawableOutcomeEnum
    {
        TopSet,     //QQ
        MiddleSet,  //66
        BottomSet,  //22
        TopTwoPairs,    //Q6
        TopBottomTwoPairs,  //Q2
        BottomTwoPairs, //62
        StraightDraw,   //43
        OverPair,   //KK
        TopPairTopKicker,    //AQ
        TopPairGoodKicker,  //QJ
        TopPairWeakKicker,  //Q7
        MiddlePair, //86
        BottomPair, //82
        UnderPair,  //
        OverCard,   //AK
        HighCard    //JT
    }

    //578, 567
    public enum OpenStraightableOutcomeEnum
    {
        HighStraight,   //96
        MiddleStraight, //84 for 567
        LowStraight,    //64
        TopSet,     //88
        MiddleSet,  //77
        BottomSet,  //55
        TopTwoPairs,    //87
        TopBottomTwoPairs,  //85
        BottomTwoPairs, //75
        OverPairOpenDraw,
        TopPairOpenDraw,    //86
        MiddlePairOpenDraw,
        BottomPairOpenDraw,
        UnderPairOpenDraw,
        OverPairCloseDraw,
        TopPairCloseDraw,   //98
        MiddlePairCloseDraw,
        BottomPairCloseDraw,
        UnderPairCloseDraw,
        OpenStraightDraw,   //43
        CloseStraightDraw,
        TopOverPair,    //AA
        OverPair,   //KK
        TopPairTopKicker,    //A8
        TopPairGoodKicker,  //J8
        TopPairWeakKicker,  //87
        MiddlePair, //76
        BottomPair, //J5
        UnderPair,  //44
        OverCard,   //AK
        HighCard    //82
    }

    //458
    public enum CloseStraightableOutcomeEnum
    {
        Straight,    //76
        TopSet,     //88
        MiddleSet,  //55
        BottomSet,  //44
        TopTwoPairs,    //85
        TopBottomTwoPairs,  //84
        BottomTwoPairs, //54
        TopPairCloseDraw,
        MiddlePairCloseDraw,
        BottomPairCloseDraw,
        OpenStraightDraw,   //36
        CloseStraightDraw,   //J6
        TopOverPair,    //AA
        OverPair,   //KK
        TopPairTopKicker,    //A8
        TopPairGoodKicker,  //J8
        TopPairWeakKicker,  //83
        MiddlePair, //K5
        BottomPair, //J4
        UnderPair,  //22
        OverCard,   //AK
        HighCard    //
    }
    */

    //J66
    public enum LowPairOutcomeEnum
    {
        FourSome,   //66
        HighFullHouse,  //JJ
        LowFullHouse,   //J6
        ThreeSomeTopKicker,  //A6
        ThreeSomeGoodKicker,    //Q6
        ThreeSomeWeakKicker,    //76
        OverTwoPairs,   //AA
        TopTwoPairsTopKicker,    //AJ
        TopTwoPairsGoodKicker,  //QJ
        TopTwoPairsWeakKicker,  //J5
        BetweenTwoPairs, //TT
        UnderTwoPairs,  //55
        OnePairTopKicker,   //AK
        OnePairGoodKicker,  //QT
        OnePairWeakKicker,  //75
    }

    //JJ7
    public enum HighPairOutcomeEnum
    {
        FourSome,   //JJ
        HighFullHouse,  //J7
        LowFullHouse,   //77
        ThreeSomeTopKicker,  //AJ
        ThreeSomeGoodKicker,    //QJ
        ThreeSomeWeakKicker,    //J7
        OverTwoPairs,   //AA
        BetweenTwoPairs, //TT
        BottomTwoPairsTopKicker,    //A7
        BottomTwoPairsGoodKicker,  //Q7
        BottomTwoPairsWeakKicker,  //75
        UnderTwoPairs,  //55
        /*
        OpenStraightDraw,   //
        CloseStraightDraw,  //98
        */
        OnePairTopKicker,   //AK
        OnePairGoodKicker,  //QT
        OnePairWeakKicker,  //65
    }

    //JJJ
    public enum ThreesomeOutcomeEnum
    {
        FourSome, //AJ
        TopFullHouse,   //AA
        GoodFullHouse,  //QQ
        WeakFullHouse, //88
        ThreeSomeTopKicker,  //AQ
        ThreeSomeGoodKicker,    //QT
        ThreeSomeWeakKicker,    //98
    }

    public enum FlopBoardSuitTextureEnum
    {
        Rainbow,
        SuitedTwo,
        SuitedThree,
    }

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
