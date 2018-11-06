namespace Coaching.Postflop
{
    //todo: 所謂hit就是某一張牌幫助我的牌型在BoardHandTypeEnum這個列表里完成了一次提升
    //提升越大，hit越狠
    //結合betting history可以預估那一步hit了，进而压缩對手的Range，并且識別出Bluff
    public enum BoardHandTypeEnum
    {
        //HighCard
        HighCard = 1,
        OverHighCard,
        TopHighCard,    //AT on KQ543

        //Pair
        UnderPair,

        LowPairWithWeakKicker,
        LowPairWithGoodKicker,
        LowPairWithTopKicker,

        MiddlePairWeakKicker,
        MiddlePairWithGoodKicker,
        MiddlePairWithTopKicker,

        TopPairWithWeakerKicker,
        TopPairWithGoodKicker,
        TopPairWithTopKicker,

        OverPiarWithWeakKicker,
        OverPairWithGoodKicker,
        OverPairWithTopKicker,

        //Straight Draw On Turn
        StraightDrawOnTurn,
        OpenEndedStraightOnTurn,

        //Flush Draw On Turn
        FlushDrawOnTurn,

        //Two Pairs
        UnderTwoPairs,  //33 on 445
        LowTwoPairs,    //23 on 344
        HighTwoPairs,   //45 on 334
        OverTwoPairs,   //44 on 233

        //Straight Draw On Flop
        StraightDrawOnFlop,
        OpenEndedStraightDrawOnFlop,

        //Flush Draw On Flop
        FlushDrawOnFlop,

        //Three Of A Kind
        LowSet,         //22 on 234
        MiddleSet,      //33 on 234
        ThreeOfAKindWithWeakerKicker,   //23 on 334
        ThreeOfAKindWithGoodKicker,   //3K on 334
        ThreeOfAKindWithTopKicker,   //3A on 334
        TopSet,         //44 on 234

        //Straight
        LowStraight,
        HighStraight,

        //Flush
        LowFlush,
        HighFlush,
        TopFlush,

        //Full House
        LowSetFullHouse,    //55 on KTT75
        MiddleSetFullHouse, //77 on KTT75
        TopSetFullHouse,    //KK on KTT75

        LowPairFullHouse,       //T5 on KTT75
        MiddlePairFullHouse,    //T7 on KTT75
        TopPairFullHouse,       //KT on KTT75

        //Four Of A Kind
        FourOfAKind,

        //Flush Straight
        LowFlushStraight,
        HighFlushStraight,

        //Royal Flush
        RoyalFlush
    }

    public enum DrawingEnum
    {
        None,
        StraightDrawing,
        FlushDrawing,
        Both
    }
}