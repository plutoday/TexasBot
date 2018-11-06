using System.Collections.Generic;
using System.Linq;
using Common;
using Infra;
using Models;

namespace River.Strategy.Multiway
{
    public class MultiwayRiverBettingStrategy
    {
        private readonly VillainProbabilityCalculator _probabilityCalculator;

        private readonly RiverBoard _riverBoard;

        private readonly RiverFolder _riverFolder;

        public MultiwayRiverBettingStrategy(RiverBoard riverBoard, HoldingHoles heroHoles)
        {
            _riverBoard = riverBoard;
            _riverFolder = new RiverFolder();
            var enumerator = new RiverFiveCardsEnumerator(_riverBoard);
            _probabilityCalculator = new VillainProbabilityCalculator(
                grid => _riverFolder.ShouldAGridFoldToBoardByRank(grid, _riverBoard),
                grid => _riverFolder.ShouldAGridFoldToBoardBySuit(grid, _riverBoard, heroHoles),
                enumerator.Enumerate);
        }

        public Decision MakeDecision(RiverDecisionContext context)
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
