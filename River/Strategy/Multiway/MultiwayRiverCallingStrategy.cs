using System.Collections.Generic;
using System.Linq;
using Common;
using Infra;
using Models;

namespace River.Strategy.Multiway
{
    public class MultiwayRiverCallingStrategy
    {
        private readonly VillainProbabilityCalculator _probabilityCalculator;

        private readonly RiverBoard _riverBoard;

        private readonly RiverFolder _riverFolder;

        public MultiwayRiverCallingStrategy(RiverBoard riverBoard, HoldingHoles heroHoles)
        {
            _riverBoard = riverBoard;
            _riverFolder = new RiverFolder();
            var fiveCardsEnumerator = new RiverFiveCardsEnumerator(riverBoard);
            _probabilityCalculator = new VillainProbabilityCalculator(
                grid => _riverFolder.ShouldAGridFoldToBoardByRank(grid, _riverBoard),
                grid => _riverFolder.ShouldAGridFoldToBoardBySuit(grid, _riverBoard, heroHoles),
                fiveCardsEnumerator.Enumerate);
        }

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
            List<VillainProbabilityResult> probabilityResults = context.AliveVillains.Select(villain
                => _probabilityCalculator.Calculate(context.HeroHoles, villain,
                Common.Utils.VillainFoldable(context.RiverRaiser, context.Hero, villain))).ToList();

            List<List<ProbabilityTuple>> tupleLists = Common.Utils.EnumerateProbabilities(0, probabilityResults).ToList();

            foreach (var tupleList in tupleLists)
            {
                Logger.Instance.Log($"{string.Join(";", tupleList.Select(t => $"{t.VillainName}-{t.ProbabilityCategory}-{t.Probability}"))}");
            }

            int callSize = context.RiverRaiser.RiverBet - context.Hero.RiverBet;
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
