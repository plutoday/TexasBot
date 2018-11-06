using Coaching.Postflop.Boards;
using Coaching.Postflop.EquityCalculation;
using Models;

namespace Coaching.Postflop.Betting
{
    public class ValueBetEvaluator
    {
        private double _potOddsToBet = 0.5;
        private readonly BruteForceEquityCalculator _equityCalculator = new BruteForceEquityCalculator();

        public bool CanBetForValue(BoardStatus boardStatus, HoldingHoles heroHoles, HoldingHoles villainHoles)
        {
            if (BettingUtils.CompareHoles(heroHoles, villainHoles, boardStatus) <= 0)
            {
                //It's not a value bet if hero's hand is worse than villain's
                return false;
            }

            var heroRange = BettingUtils.GetVillainGuessOnHeroRange();
            var villainEquity = _equityCalculator.CalculateEquityAgainstARange(villainHoles, heroRange, boardStatus);

            return BettingUtils.VillainIsWillingToCall(villainEquity, _potOddsToBet) == true;
        }
    }
}
