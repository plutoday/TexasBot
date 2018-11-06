using System;
using System.Linq;
using Coaching.Postflop.Boards.BoardSpectrums;
using Coaching.Postflop.EquityCalculation;
using Coaching.Postflop.Ranging;
using Models;
using Models.Ranging;

namespace Coaching.Postflop.Betting
{
    public class BettingStrategy
    {
        private readonly RangeEstimator _rangeEstimator = new RangeEstimator();
        private readonly BoardSpectrumMaker _boardSpectrumMaker = new BoardSpectrumMaker();
        private readonly BoardSpectrumFilter _boardSpectrumFilter = new BoardSpectrumFilter();

        public Decision MakeDecision(PostflopStatusSummary statusSummary)
        {
            if (statusSummary.AliveVillains.Count == 1)
            {
                var villain = statusSummary.AliveVillains[0];
                return MakeDecisionAgainstSingleVillain(statusSummary, villain);
            }
            else
            {
                return MakeDecisionAgainstMultipleVillains(statusSummary);
            }
        }

        private Decision MakeDecisionAgainstMultipleVillains(PostflopStatusSummary statusSummary)
        {
            throw new NotImplementedException();
        }

        private Decision MakeDecisionAgainstSingleVillain(PostflopStatusSummary statusSummary, PostflopPlayerSummary villain)
        {
            var boardStatus = statusSummary.BoardStatus;
            var heroHoles = statusSummary.Me.Holes;

            var villainRange = _rangeEstimator.EstimateRange(statusSummary, villain);
            var heroRange = GetVillainGuessOnHeroRange(statusSummary.Me);

            var rawBoardSpectrum = _boardSpectrumMaker.MakeSpectrum(boardStatus);

            var villainBoardSpectrum = _boardSpectrumFilter.FilterAgainstRange(rawBoardSpectrum, villainRange);
            var heroBoardSpectrum = _boardSpectrumFilter.FilterAgainstRange(rawBoardSpectrum, heroRange);

            var heroEquityAgainstVillainRange = villainBoardSpectrum.GetEquity(heroHoles);

            var villainHolesCombos = villainRange.GetAliveGrids().SelectMany(g => g.Grid.EnumerateAllCombos());
            foreach (var villainHoles in villainHolesCombos)
            {
                var villainEquityAgainstHeroRange = heroBoardSpectrum.GetEquity(villainHoles);
                var fightResult = villainBoardSpectrum.Fight(heroHoles, villainHoles);

                var bettingDecisionEnum = MakeBettingDecisionBasedOnEquitiesComparison(heroEquityAgainstVillainRange,
                    villainEquityAgainstHeroRange, fightResult);

                //todo accumulate them to make the final decision
            }

            throw new NotImplementedException();
        }
        

        private BettingDecisionEnum MakeBettingDecisionBasedOnEquitiesComparison(double heroEquity, double villainEquity,
            BoardSpectrumFightResult fightResult)
        {
            double middleEquity = 0.5;
            int middleNumOfOuts = 6;
            if (heroEquity > villainEquity && villainEquity > middleEquity)
            {
                if (fightResult.HeroWin)
                {
                    if (fightResult.VillainOuts.Count < middleNumOfOuts)
                    {
                        return BettingDecisionEnum.ValueBet;
                    }
                    else
                    {
                        return BettingDecisionEnum.ForceFold;
                    }
                }
                else
                {
                    //todo: Hero should win
                    throw new NotImplementedException();
                }
            }

            if (heroEquity > villainEquity && middleEquity > villainEquity)
            {
                if (fightResult.HeroWin)
                {
                    if (fightResult.VillainOuts.Count < middleNumOfOuts)
                    {
                        return BettingDecisionEnum.CollectMoney;
                    }
                }
            }

            if (middleEquity > villainEquity && villainEquity > heroEquity)
            {
                return BettingDecisionEnum.Bluff;
            }

            //all the other situations, check
            return BettingDecisionEnum.Check;
        }
        
        /// <summary>
        /// The hero's range guessed by villain
        /// for a dumb implementation, return a full Range
        /// or, return a position-based range.
        /// </summary>
        /// <returns></returns>
        private PlayerRange GetVillainGuessOnHeroRange(PostflopPlayerSummary hero)
        {
            throw new NotImplementedException();
        }
    }
}
