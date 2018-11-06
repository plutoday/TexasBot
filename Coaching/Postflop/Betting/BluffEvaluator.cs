using System;
using Coaching.Postflop.Boards;
using Coaching.Postflop.EquityCalculation;
using Models;

namespace Coaching.Postflop.Betting
{
    public class BluffEvaluator
    {
        private double _potOddsToBluff = 0.5;

        private readonly BruteForceEquityCalculator _equityCalculator = new BruteForceEquityCalculator();

        /// <summary>
        /// Don't call this method if the board is not scary enough for bluffing
        /// or the position/number of alive players doesn't allow a bluffing play
        /// Check those condition prior to calling this method.
        /// </summary>
        /// <param name="boardStatus"></param>
        /// <param name="heroHoles"></param>
        /// <param name="villainHoles"></param>
        /// <returns></returns>
        public bool CanBluff(BoardStatus boardStatus, HoldingHoles heroHoles, HoldingHoles villainHoles)
        {
            if (BettingUtils.CompareHoles(heroHoles, villainHoles, boardStatus) > 0)
            {
                //It's not a bluff bet with a better hand than villain's
                return false;
            }

            var heroRange = BettingUtils.GetVillainGuessOnHeroRange();
            var villainEquity = _equityCalculator.CalculateEquityAgainstARange(villainHoles, heroRange, boardStatus);

            return BettingUtils.VillainIsWillingToCall(villainEquity, _potOddsToBluff) == false;
        }
    }
}
