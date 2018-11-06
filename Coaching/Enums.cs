namespace Coaching
{
    public enum PreflopStatusEnum
    {
        FoldedToYou,
        CallersBeforeYou,
        RaisedBeforeYou,
        ReraisedBeforeYou,
        AllInBeforeYou,
    }

    public enum StartingHandTypeEnum
    {
        Pair,
        AceXSuited,
        AceXOffsuit,
        Suited,
        OffSuit
    }

    public enum StartingHandGradeEnum
    {
        Premium,
        StrongPair,
        RegularPair,

        AceKing,
        AceStrong,
        AceGood,
        AceFair,

        StrongPlayable,
        Playable
    }

    /// <summary>
    /// Decision types for BettingStrategy to make
    /// </summary>
    public enum BettingDecisionEnum
    {
        ValueBet,
        ForceFold,
        Bluff,
        CollectMoney,
        Check
    }

    public enum PreflopPlayerStatus
    {
        NotPooledYet,
        Fold,
        Limped,
        Raised,
        RaiseCalled,
        ThreeBet,
        ThreeBetCalled,
        FourBet,
        FourBetCalled,
        AllIn,
        AllInCalled
    }

    public enum FlopPlayerStatus
    {
        NotPolledYet,
        CheckRaiser,
        Check,
        Raise,
        AllIn,
        CallRaise,
        RaiseRaise,
        AllInRaise,
    }
}