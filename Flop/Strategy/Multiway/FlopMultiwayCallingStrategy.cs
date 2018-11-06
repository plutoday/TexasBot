using System.Collections.Generic;
using System.Linq;
using Common;
using Infra;
using Models;

namespace Flop.Strategy.Multiway
{
    public class FlopMultiwayCallingStrategy
    {
        private readonly VillainProbabilityCalculator _probabilityCalculator;

        private readonly FlopBoard _flopBoard;

        private readonly FlopFolder _flopFolder;

        public FlopMultiwayCallingStrategy(FlopBoard flopBoard, HoldingHoles heroHoles)
        {
            _flopBoard = flopBoard;
            _flopFolder = new FlopFolder();
            var fiveCardsEnumerator = new FlopFiveCardsEnumerator(flopBoard, heroHoles);
            _probabilityCalculator = new VillainProbabilityCalculator(
                grid => _flopFolder.ShouldAGridFoldToBoardByRank(grid, _flopBoard),
                grid => _flopFolder.ShouldAGridFoldToBoardBySuit(grid, flopBoard, heroHoles),
                fiveCardsEnumerator.Enumerate);
        }

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
            List<VillainProbabilityResult> probabilityResults = context.AliveVillains.Select(villain
                => _probabilityCalculator.Calculate(context.HeroHoles, villain,
                Common.Utils.VillainFoldable(context.FlopRaiser, context.Hero, villain))).ToList();

            List<List<ProbabilityTuple>> tupleLists = Common.Utils.EnumerateProbabilities(0, probabilityResults).ToList();

            foreach (var tupleList in tupleLists)
            {
                Logger.Instance.Log($"{string.Join(";", tupleList.Select(t => $"{t.VillainName}-{t.ProbabilityCategory}-{t.Probability}"))}");
            }

            int callSize = context.FlopRaiser.FlopBet - context.Hero.FlopBet;
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
