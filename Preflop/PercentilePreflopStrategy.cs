using System;
using Models;
using Preflop.StartingHands;

namespace Preflop
{
    public class PercentilePreflopStrategy
    {
        private readonly HandRangeConsultant _handRangeConsultant = new HandRangeConsultant();
        private readonly BetSizeConsultant _betSizeConsultant = new BetSizeConsultant();

        public Decision MakeDecision(PreflopStatusSummary statusSummary, StartingHand startingHand)
        {
            var handGrade = _handRangeConsultant.GenerateHandGrader(statusSummary).GradeAHand(startingHand);
            return GenerateDecisionFromHandGrade(handGrade, statusSummary);
        }

        private Decision GenerateDecisionFromHandGrade(HandValueGradeEnum handGrade, PreflopStatusSummary statusSummary)
        {
            DecisionType decisionType = TranslateBasedOnIsRaised(handGrade, statusSummary);
            int chipsAdded = _betSizeConsultant.GetBetSize(statusSummary, decisionType);

            return new Decision(decisionType, chipsAdded);
        }

        private DecisionType TranslateBasedOnIsRaised(HandValueGradeEnum handGrade, PreflopStatusSummary statusSummary)
        {
            switch (handGrade)
            {
                case HandValueGradeEnum.AllIn:
                    return DecisionType.AllIn;
                case HandValueGradeEnum.BetForValue:
                    return statusSummary.IsRaised ? DecisionType.Reraise : DecisionType.Raise;
                case HandValueGradeEnum.Flat:
                    return DecisionType.Call;
                case HandValueGradeEnum.BetForBluff:
                    return statusSummary.IsRaised ? DecisionType.Reraise : DecisionType.Raise;
                    case HandValueGradeEnum.Fold:
                    return DecisionType.Fold;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
