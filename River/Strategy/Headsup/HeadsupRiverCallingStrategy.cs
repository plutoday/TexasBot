using Common;
using Infra;
using Models;

namespace River.Strategy.Headsup
{
    public class HeadsupRiverCallingStrategy
    {
        public Decision MakeDecision(RiverDecisionContext context)
        {
            var decision = MakeReraiseDecision(context) ?? MakeCallDecision(context);

            return decision;
        }

        private Decision MakeReraiseDecision(RiverDecisionContext context)
        {
            //todo: implement the reraise logic
            return null;
        }

        private Decision MakeCallDecision(RiverDecisionContext context)
        {
            int chipsToCall = context.RiverRaiser.RiverBet - context.Hero.RiverBet;
            double potOdds = (double)chipsToCall / (context.CurrentPotSize + chipsToCall);
            var raiserRange = context.RiverRaiser.RiverRange;
            var equityCalculator = new EquityCalculator(new RiverFiveCardsEnumerator(context.RiverBoard));
            var equity = equityCalculator.CalculateEquity(context.HeroHoles, raiserRange);

            Logger.Instance.Log($"Pot odds is {potOdds}, hero's equity is {equity} against raiser {context.RiverRaiser.Position}-{context.RiverRaiserName}'s range: {raiserRange.ToString()}");

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
