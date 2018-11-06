using System.Collections.Generic;
using System.Linq;
using Common;
using Flop;
using Flop.Strategy;
using Infra;
using Models;

namespace Turn.Strategy.Multiway
{
    public class TurnMultiwayBettingStrategy
    {
        private readonly VillainProbabilityCalculator _probabilityCalculator;

        private readonly TurnBoard _turnBoard;

        private readonly TurnFolder _turnFolder;

        public TurnMultiwayBettingStrategy(TurnBoard turnBoard, HoldingHoles heroHoles)
        {
            _turnBoard = turnBoard;
            _turnFolder = new TurnFolder();
            var enumerator = new TurnFiveCardsEnumerator(_turnBoard, heroHoles);
            _probabilityCalculator = new VillainProbabilityCalculator(
                grid => _turnFolder.ShouldAGridFoldToBoardByRank(grid, _turnBoard),
                grid => _turnFolder.ShouldAGridFoldToBoardBySuit(grid, _turnBoard, heroHoles),
                enumerator.Enumerate);
        }

        public Decision MakeDecision(TurnDecisionContext context)
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
