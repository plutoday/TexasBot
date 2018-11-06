using System.Collections.Generic;
using System.Linq;
using Common;
using Infra;
using Models;

namespace Turn.Strategy.Multiway
{
    public class TurnMultiwayCallingStrategy
    {
        private readonly VillainProbabilityCalculator _probabilityCalculator;

        private readonly TurnBoard _turnBoard;

        private readonly TurnFolder _turnFolder;

        public TurnMultiwayCallingStrategy(TurnBoard turnBoard, HoldingHoles heroHoles)
        {
            _turnBoard = turnBoard;
            _turnFolder = new TurnFolder();
            var fiveCardsEnumerator = new TurnFiveCardsEnumerator(turnBoard, heroHoles);
            _probabilityCalculator = new VillainProbabilityCalculator(
                grid => _turnFolder.ShouldAGridFoldToBoardByRank(grid, _turnBoard),
                grid => _turnFolder.ShouldAGridFoldToBoardBySuit(grid, _turnBoard, heroHoles),
                fiveCardsEnumerator.Enumerate);
        }

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
            List<VillainProbabilityResult> probabilityResults = context.AliveVillains.Select(villain
                => _probabilityCalculator.Calculate(context.HeroHoles, villain,
                Common.Utils.VillainFoldable(context.TurnRaiser, context.Hero, villain))).ToList();

            List<List<ProbabilityTuple>> tupleLists = Common.Utils.EnumerateProbabilities(0, probabilityResults).ToList();

            foreach (var tupleList in tupleLists)
            {
                Logger.Instance.Log($"{string.Join(";", tupleList.Select(t => $"{t.VillainName}-{t.ProbabilityCategory}-{t.Probability}"))}");
            }

            int callSize = context.TurnRaiser.TurnBet - context.Hero.TurnBet;
            int potSize = context.CurrentPotSize;
            double ev = tupleLists.Sum(tupleList => Common.Utils.CalculateEv(tupleList, callSize, potSize));
            Logger.Instance.Log($"{callSize} to call a pot of size {potSize}, ev={ev}");

            if (Common.Utils.EvProfitable(ev, potSize, callSize))
            {
                Logger.Instance.Log($"Profitable, calling");
                return new Decision(DecisionType.Call, callSize);
            }

            Logger.Instance.Log($"Not profitable, folding");
            return new Decision(DecisionType.Fold, 0);

        }
    }
}
