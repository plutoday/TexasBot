using Common;
using Infra;
using Models;

namespace Flop.Strategy.Headsup
{
    public class FlopHeadsUpCallingStrategy
    {
        public Decision MakeDecision(FlopDecisionContext context)
        {
            var decision = MakeReraiseDecision(context) ?? MakeCallDecision(context);

            return decision;
        }

        private Decision MakeReraiseDecision(FlopDecisionContext context)
        {
            //todo: implement the reraise logic
            return null;
        }

        private Decision MakeCallDecision(FlopDecisionContext context)
        {
            int chipsToCall = context.FlopRaiser.FlopBet - context.Hero.FlopBet;
            double potOdds = (double)chipsToCall / (context.CurrentPotSize + chipsToCall);
            var raiserRange = context.FlopRaiser.FlopRange;
            
            var equityCalculator = new EquityCalculator(new FlopFiveCardsEnumerator(context.FlopBoard, context.HeroHoles));
            var equity = equityCalculator.CalculateEquity(context.HeroHoles, raiserRange);

            Logger.Instance.Log($"Pot odds is {potOdds}, hero's equity is {equity} against raiser {context.FlopRaiser.Position}-{context.FlopRaiserName}'s range: {raiserRange.ToString()}");

            if (equity <= potOdds)
            {
                Logger.Instance.Log($"Fold");
                return new Decision(DecisionType.Fold, 0);
            }

            Logger.Instance.Log($"Call {chipsToCall} chips");
            return new Decision(DecisionType.Call, chipsToCall);

            //todo: implement the reraise logic
        }
    }
}
