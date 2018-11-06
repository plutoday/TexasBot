namespace Models
{
    public enum PositionEnum
    {
        SmallBlind,
        BigBlind,
        UnderTheGun,
        MiddlePosition,
        CuttingOff,
        Button,
        Total = 6
    }

    public enum SuitEnum
    {
        Undefined,
        Diamond,
        Club,
        Heart,
        Spade,
    }

    public enum RankEnum
    {
        Undefined,
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace,
    }

    public enum HandEnum
    {
        HighCard,
        OnePair,
        StraightDraw,
        FlushDraw,
        TwoPairs,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush,
        RoyalFlush
    }

    public enum StageEnum
    {
        Preflop,
        Flop,
        Turn,
        River,
        Showhand
    }

    /// <summary>
    /// Unpolled/Polled/AllIned/Folded
    /// </summary>
    public enum PlayerStatusEnum
    {
        Unpolled,
        Polled,
        AllIned,
        Folded,
    }

    /// <summary>
    /// Included/Neutral/Excluded/Undefined
    /// </summary>
    public enum PlayerRangeGridStatusEnum
    {
        //keep this order in place, why?

        //We should give Included grids and Neutral grids different weights when accumulating the PkResult

        //Should be in player's range based on his moves
        Included = 0,
        
        //Initial state
        Neutral = 1,    
        
        //Should be excluded from player's range based on his moves
        Excluded = 2,

        //No information acquired against this grid based on player's moves, we should neglect this testing result
        //we should never be able to see it when Pking
        Undefined = 3,
    }

    /// <summary>
    /// Nuts/Elite/Good/Marginal/Trash/SuitDepending/Unavailable
    /// </summary>
    public enum BoardRangeGridStatusEnum
    {
        //yield to none, should always raise regardless to position and initiative
        Nuts,

        //yield to few, raise as long as in position or has the initiative
        Elite,

        Good,

        //
        Marginal,

        //yield to most, check/fold
        Trash,

        //varies on suits
        SuitDepending,

        //Not available, in player's range, maybe
        Unavailable,
    }

    /// <summary>
    /// Undefined/NotAvailable/Fold/CallWin/CallTie/CallLose
    /// </summary>
    public enum PlayerGridPkStatusEnum
    {
        Undefined,
        NotAvailable,
        Fold,
        CallWin,
        CallTie,
        CallLose
    }

    /// <summary>
    /// NotPolledYet/Fold/Check/Call/Raise/Reraise/AllIn
    /// </summary>
    public enum PlayerStreetStatusEnum
    {
        NotPolledYet,
        Fold,
        Check,
        Call,
        Raise,
        Reraise,
        AllIn
    }

    /// <summary>
    /// Suited/Offsuit/Paired
    /// </summary>
    public enum GridCategoryEnum
    {
        Suited,
        Offsuit,
        Paired
    }

    /// <summary>
    /// Promoted/Enhanced/None
    /// How the grid hit the Turn or River
    /// </summary>
    public enum GridHitNewRoundResultEnum
    {
        Promoted,   //improved the involvement, from 0 to 1 card, or from 1 to 2 cards
        Enhanced,   //involvement unchanged, but hand improved, from 1 pair to two pairs, from threesome to fullhouse
        None,       //the turn doesn't help at all
        Unavailable
    }

    /// <summary>
    /// HighCard/OnePair/TwoPairs/Threesome/Straight/FullHouse/Foursome
    /// </summary>
    public enum RankHandGradeEnum
    {
        HighCard,
        OnePair,
        TwoPairs,
        Threesome,
        Straight,
        FullHouse,
        Foursome
    }

    /// <summary>
    /// Nothing/FlushDraw/Flush/StraightFlush/RoyalFlush
    /// </summary>
    public enum SuitHandGradeEnum
    {
        Nothing,
        FlushDraw,
        Flush,
        StraightFlush,
        RoyalFlush
    }
}
