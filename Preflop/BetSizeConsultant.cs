using System;
using Models;

namespace Preflop
{
    public class BetSizeConsultant
    {
        public int GetBetSize(PreflopStatusSummary statusSummary, DecisionType decisionType)
        {
            switch (decisionType)
            {
                case DecisionType.Call:
                    return statusSummary.ChipsToCall;
                    case DecisionType.Fold:
                    return 0;
                    case DecisionType.Check:
                    return 0;
                    case DecisionType.AllIn:
                    return statusSummary.Me.StackSize;
                    case DecisionType.Raise:
                case DecisionType.Reraise:
                    return Math.Max((int)(statusSummary.ChipsToCall * 1.5), Math.Min((int)(statusSummary.PotSize*0.5), statusSummary.Me.StackSize));
            }

            //todo: this method should contain more logics than above
            throw new NotImplementedException();
        }
    }
}
