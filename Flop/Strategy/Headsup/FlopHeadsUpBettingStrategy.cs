using Common;
using Models;

namespace Flop.Strategy.Headsup
{
    public class FlopHeadsUpBettingStrategy
    {
        private readonly VillainProbabilityCalculator _probabilityCalculator;

        private readonly FlopBoard _flopBoard;

        private readonly FlopFolder _flopFolder;

        public FlopHeadsUpBettingStrategy(FlopBoard flopBoard, HoldingHoles heroHoles)
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
