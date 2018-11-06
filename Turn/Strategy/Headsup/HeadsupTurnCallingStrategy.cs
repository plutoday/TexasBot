using Common;
using Infra;
using Models;

namespace Turn.Strategy.Headsup
{
    public class HeadsupTurnCallingStrategy
    {
        public Decision MakeDecision(TurnDecisionContext context)
        {
            var decision = MakeReraiseDecision(context) ?? MakeCallDecision(context);

            return decision;
        }

        private Decision MakeReraiseDecision(TurnDecisionContext context)
        {
            //todo: implement the reraise logic
            return null;
        }

        private Decision MakeCallDecision(TurnDecisionContext context)
        {
            int chipsToCall = context.TurnRaiser.TurnBet - context.Hero.TurnBet;
            double potOdds = (double)chipsToCall / (context.CurrentPotSize + chipsToCall);
            var raiserRange = context.TurnRaiser.TurnRange;
            var equityCalculator = new EquityCalculator(new TurnFiveCardsEnumerator(context.TurnBoard, context.HeroHoles));
            var equity = equityCalculator.CalculateEquity(context.HeroHoles, raiserRange);

            Logger.Instance.Log($"Pot odds is {potOdds}, hero's equity is {equity} against raiser {context.TurnRaiser.Position}-{context.TurnRaiserName}'s range: {raiserRange.ToString()}");

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
