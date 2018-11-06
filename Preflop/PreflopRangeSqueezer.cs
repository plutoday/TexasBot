using System;
using Common;
using Infra;
using Models;
using Models.Ranging;

namespace Preflop
{
    public class PreflopRangeSqueezer
    {
        private readonly PreflopRangeConsultant _preflopRangeConsultant;
        public PreflopRangeSqueezer()
        {
            _preflopRangeConsultant  = new PreflopRangeConsultant();
        }

        public PlayerRange Squeeze(PlayerRange previousRange, Move lastMove, int bigBlindSize)
        {
            switch (lastMove.Decision.DecisionType)
            {
                case DecisionType.AllIn:
                case DecisionType.AllInRaise:
                case DecisionType.Raise:
                case DecisionType.Reraise:
                    return SqueezeOnRaise(previousRange, lastMove, bigBlindSize);
                case DecisionType.Ante:
                    return previousRange;
                case DecisionType.Call:
                    return SqueezeOnCall(previousRange, lastMove, bigBlindSize);
                default:
                    throw new InvalidOperationException($"{lastMove.Decision.DecisionType} should not show in Preflop Squeeze");
            }
        }

        private PlayerRange SqueezeOnRaise(PlayerRange previousRange, Move lastMove, int bigBlindSize)
        {
            double raiseRatio = (double) lastMove.Decision.ChipsAdded/bigBlindSize;

            var raiseStage = GetRaiseStageBasedOnRaiseRatio(raiseRatio);

            var newRange = _preflopRangeConsultant.GetRaiseRange(raiseStage, lastMove.Player.Position);
            var intersectedRange = previousRange.Intersect(newRange);
            
            Logger.Instance.LogSqueezing(previousRange, newRange, intersectedRange, lastMove);

            return intersectedRange;
        }

        private PlayerRange SqueezeOnCall(PlayerRange previousRange, Move lastMove, int bigBlindSize)
        {
            double callRatio = (double)lastMove.Decision.ChipsAdded / bigBlindSize;

            var raiseStage = GetRaiseStageBasedOnRaiseRatio(callRatio);

            var newRange = _preflopRangeConsultant.GetCallRange(raiseStage, lastMove.Player.Position);

            var intersectedRange = previousRange.Intersect(newRange);

            Logger.Instance.LogSqueezing(previousRange, newRange, intersectedRange, lastMove);

            return intersectedRange;
        }

        private PreflopRaiseStageEnum GetRaiseStageBasedOnRaiseRatio(double raiseRatio)
        {
            if (raiseRatio <= 1)
            {
                return PreflopRaiseStageEnum.Ante;
            }
            if (raiseRatio < 5)
            {
                return PreflopRaiseStageEnum.OpenRaise;
            }
            if (raiseRatio < 15)
            {
                return PreflopRaiseStageEnum.ThreeBet;
            }
            if (raiseRatio < 45)
            {
                return PreflopRaiseStageEnum.FourBet;
            }
            return PreflopRaiseStageEnum.FiveBet;
        }
    }
}
