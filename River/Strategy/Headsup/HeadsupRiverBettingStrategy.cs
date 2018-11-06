using System.Collections.Generic;
using Common;
using Flop.Strategy;
using Models;

namespace River.Strategy.Headsup
{
    public class HeadsupRiverBettingStrategy
    {
        private readonly VillainProbabilityCalculator _probabilityCalculator;

        private readonly RiverBoard _riverBoard;
        private readonly RiverFolder _riverFolder;

        public HeadsupRiverBettingStrategy(RiverBoard riverBoard, HoldingHoles heroHoles)
        {
            _riverBoard = riverBoard;
            _riverFolder = new RiverFolder();
            _probabilityCalculator = new VillainProbabilityCalculator(
                grid => _riverFolder.ShouldAGridFoldToBoardByRank(grid, _riverBoard),
                grid => _riverFolder.ShouldAGridFoldToBoardBySuit(grid, _riverBoard, heroHoles),
                () => DumbEnumerate(_riverBoard));
        }

        private IEnumerable<List<Card>> DumbEnumerate(RiverBoard riverBoard)
        {
            yield return new List<Card>()
            {
                riverBoard.TurnBoard.FlopBoard.Flop1,
                riverBoard.TurnBoard.FlopBoard.Flop2,
                riverBoard.TurnBoard.FlopBoard.Flop3,
                riverBoard.TurnBoard.TurnCard,
                riverBoard.River
            };
        }

        public Decision MakeDecision(RiverDecisionContext context)
        {
            var probabilityResult = _probabilityCalculator.Calculate(context.HeroHoles,
                context.HeadsUpVillain);

            int betSize = Common.Utils.GetBetSize(context.CurrentPotSize);
            int potSize = context.CurrentPotSize;

            var ev = probabilityResult.Probabilities[ProbabilityEnum.Fold] * potSize
                + probabilityResult.Probabilities[ProbabilityEnum.CallLose] * (potSize + betSize)
                - probabilityResult.Probabilities[ProbabilityEnum.CallWin] * betSize
                + probabilityResult.Probabilities[ProbabilityEnum.CallTie] * (potSize + betSize) / 2;

            if (Common.Utils.EvProfitable(ev, potSize, betSize))
            {
                return new Decision(DecisionType.Raise, betSize);
            }

            return new Decision(DecisionType.Check, 0);
        }
    }
}
