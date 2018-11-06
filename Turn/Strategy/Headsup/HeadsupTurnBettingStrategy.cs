using Common;
using Flop;
using Flop.Strategy;
using Models;

namespace Turn.Strategy.Headsup
{
    public class HeadsupTurnBettingStrategy
    {
        private readonly VillainProbabilityCalculator _probabilityCalculator;

        private readonly TurnBoard _turnBoard;

        private readonly TurnFolder _turnFolder;
        
        public HeadsupTurnBettingStrategy(TurnBoard turnBoard, HoldingHoles heroHoles)
        {
            _turnBoard = turnBoard;
            _turnFolder = new TurnFolder();
            var fiveCardsEnumerator = new TurnFiveCardsEnumerator(_turnBoard, heroHoles);
            _probabilityCalculator = new VillainProbabilityCalculator(
                grid => _turnFolder.ShouldAGridFoldToBoardByRank(grid, _turnBoard),
                grid => _turnFolder.ShouldAGridFoldToBoardBySuit(grid, _turnBoard, heroHoles),
                fiveCardsEnumerator.Enumerate);
        }

        public Decision MakeDecision(TurnDecisionContext context)
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
