using System.Collections.Generic;
using Coaching.Postflop.Betting;
using Coaching.Postflop.Boards;
using Coaching.Postflop.Ranging;
using Models;
using Models.Ranging;

namespace Coaching.Postflop.EquityCalculation
{
    public class BruteForceEquityCalculator : IEquityCalculator
    {
        public double CalculateEquityAgainstARange(HoldingHoles heroHoles, PlayerRange villainPlayerRange, BoardStatus boardStatus)
        {
            var villainGrids = villainPlayerRange.GetAliveGrids();

            int numOfCombos = 0;
            double equitySum = 0;

            foreach (var villainGrid in villainGrids)
            {
                foreach (var villainHoles in villainGrid.Grid.EnumerateAllCombos())
                {
                    var handEquity = CalculateEquityAgainstGrid(heroHoles, villainHoles, boardStatus);
                    equitySum += handEquity * villainGrid.Grid.PossibleCount;
                    numOfCombos += villainGrid.Grid.PossibleCount;
                }
            }

            return equitySum / numOfCombos;
        }

        public double CalculateEquityAgainstGrid(HoldingHoles heroHoles, HoldingHoles villainHoles, BoardStatus boardStatus)
        {
            var boards =
                boardStatus.GenerateAllPossibleBoards(new List<Card>()
                {
                    heroHoles.Hole1,
                    heroHoles.Hole2,
                    villainHoles.Hole1,
                    villainHoles.Hole2
                });

            double heroWin = 0, villainWin = 0;

            foreach (var board in boards)
            {
                var heroSeven = new SevenCardsHand(heroHoles.Hole1, heroHoles.Hole2, board.Flop1, board.Flop2, board.Flop3, board.Turn, board.River);
                var villainSeven = new SevenCardsHand(villainHoles.Hole1, villainHoles.Hole2, board.Flop1, board.Flop2, board.Flop3, board.Turn, board.River);

                var heroFive = BettingUtils.FindBestFive(heroSeven);
                var villainFive = BettingUtils.FindBestFive(villainSeven);

                int heroScore = BettingUtils.GetScoreForFiveCardHand(heroFive);
                int villainScore = BettingUtils.GetScoreForFiveCardHand(villainFive);

                if (heroScore > villainScore)
                {
                    heroWin++;
                }
                else
                {
                    villainWin++;
                }
            }

            return heroWin / (heroWin + villainWin);
        }
    }
}
