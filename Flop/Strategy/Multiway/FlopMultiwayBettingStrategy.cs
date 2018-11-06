using System.Collections.Generic;
using System.Linq;
using Common;
using Infra;
using Models;

namespace Flop.Strategy.Multiway
{
    public class FlopMultiwayBettingStrategy
    {
        private readonly VillainProbabilityCalculator _probabilityCalculator;

        private readonly FlopBoard _flopBoard;

        private readonly FlopFolder _flopFolder;

        public FlopMultiwayBettingStrategy(FlopBoard flopBoard, HoldingHoles heroHoles)
        {
            _flopBoard = flopBoard;
            _flopFolder = new FlopFolder();
            var enumerator = new FlopFiveCardsEnumerator(_flopBoard, heroHoles);
            _probabilityCalculator = new VillainProbabilityCalculator(
                grid => _flopFolder.ShouldAGridFoldToBoardByRank(grid, _flopBoard),
                grid => _flopFolder.ShouldAGridFoldToBoardBySuit(grid, _flopBoard, heroHoles),
                enumerator.Enumerate);
        }

        public Decision MakeDecision(FlopDecisionContext context)
        {
            List<VillainProbabilityResult> probabilityResults = context.AliveVillains.Select(villain
                => _probabilityCalculator.Calculate(context.HeroHoles, villain)).ToList();

            List<List<ProbabilityTuple>> tupleLists = Common.Utils.EnumerateProbabilities(0, probabilityResults).ToList();

            foreach (var tupleList in tupleLists)
            {
                Logger.Instance.Log($"{string.Join(";", tupleList.Select(t => $"{t.VillainName}-{t.ProbabilityCategory}-{t.Probability}"))}");
            }

            int betSize = Common.Utils.GetBetSize(context.CurrentPotSize);
            int potSize = context.CurrentPotSize;
            double ev = tupleLists.Sum(tupleList => Common.Utils.CalculateEv(tupleList, betSize, potSize));
            Logger.Instance.Log($"{betSize} to bet a pot of size {potSize}, ev={ev}");

            if (Common.Utils.EvProfitable(ev, potSize, betSize))
            {
                return new Decision(DecisionType.Raise, betSize);
            }

            return new Decision(DecisionType.Check, 0);
        }
    }
}
