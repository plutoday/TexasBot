using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preflop
{
    public enum PreflopGameStatusEnum
    {
        FoldedToMe,
        LimpedPot,
        Raised,
        RaisedWithCallers,
        TriBet,
        TriBetWithCallers,
        FourBet,
        FourBetWithCallers,
        FiveBet,
        FiveBetWithCallers,
        AllIn
    }

    public enum PreflopGameStatusSimpleEnum
    {
        Unraised,
        Raised,
        ThreeBet,
        FourBet,
        AllIn
    }

    public static class PreflopEnumMethods
    {
        public static PreflopGameStatusSimpleEnum ToSimple(this PreflopGameStatusEnum statusEnum)
        {
            switch (statusEnum)
            {
                case PreflopGameStatusEnum.FoldedToMe:
                case PreflopGameStatusEnum.LimpedPot:
                    return PreflopGameStatusSimpleEnum.Unraised;
                case PreflopGameStatusEnum.Raised:
                case PreflopGameStatusEnum.RaisedWithCallers:
                    return PreflopGameStatusSimpleEnum.Raised;
                case PreflopGameStatusEnum.TriBet:
                case PreflopGameStatusEnum.TriBetWithCallers:
                    return PreflopGameStatusSimpleEnum.ThreeBet;
                case PreflopGameStatusEnum.FourBet:
                case PreflopGameStatusEnum.FourBetWithCallers:
                    return PreflopGameStatusSimpleEnum.FourBet;
                case PreflopGameStatusEnum.FiveBet:
                case PreflopGameStatusEnum.FiveBetWithCallers:
                case PreflopGameStatusEnum.AllIn:
                    return PreflopGameStatusSimpleEnum.AllIn;
                default:
                    throw new InvalidOperationException();
            }
        }
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
    /// BetForValue/BetForBluff/Flat/Fold/AllIn
    /// </summary>
    public enum HandValueGradeEnum
    {
        BetForValue,    //ready to all in
        BetForBluff,    //ready to fold to reraise
        Flat,
        Fold,
        AllIn
    }

    /// <summary>
    /// Ante/OpenRaise/ThreeBet/FourBet/FiveBet/AllIn
    /// </summary>
    public enum PreflopRaiseStageEnum
    {
        Ante,
        OpenRaise,
        ThreeBet,
        FourBet,
        FiveBet,
        AllIn
    }
}
