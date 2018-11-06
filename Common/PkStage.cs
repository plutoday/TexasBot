using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infra;
using Models;
using Models.Hand;
using Models.Ranging;

namespace Common
{
    public class PkStage
    {
        private readonly OffsuitHandFinder _offsuitHandFinder = new OffsuitHandFinder();
        private readonly SuitedHandFinder _suitedHandFinder = new SuitedHandFinder();

        private readonly Func<IEnumerable<List<Card>>> _fiveCardsEnumerator;
        private readonly Func<PlayerRangePkGrid, IEnumerable<HoldingHoles>> _holesEnumerator;
        
        public PkStage(Func<IEnumerable<List<Card>>> fiveCardsEnumerator, Func<PlayerRangePkGrid, IEnumerable<HoldingHoles>> holesEnumerator)
        {
            _fiveCardsEnumerator = fiveCardsEnumerator;
            _holesEnumerator = holesEnumerator;
        }

        public PkResult Pk(HoldingHoles heroHoles, VillainPkRange villainRange)
        {
            var pkResult = new PkResult();
            Parallel.ForEach(villainRange.Grids.EnumerateMultiDimensionalArray(), playerGrid =>
           {
               var gridPkResult = Pk(heroHoles, playerGrid);
               Logger.Instance.Log($"Pk finished between heroHoles {heroHoles.Hole1.GetStringForCard() + heroHoles.Hole2.GetStringForCard()}" +
                         $" and {playerGrid.Grid}, result is {gridPkResult}");
               pkResult.Add(gridPkResult);
           });

            return pkResult;
        }

        private PkResult Pk(HoldingHoles heroHoles, PlayerRangePkGrid villainGrid)
        {
            var pkResult = new PkResult();

            foreach (var fiveCards in _fiveCardsEnumerator.Invoke())
            {
                if (ConflictsWithVillainGrid(fiveCards, villainGrid.Grid))
                {
                    continue;
                }
                

                var conflictCards = new List<Card>(fiveCards);
                conflictCards.Add(heroHoles.Hole1);
                conflictCards.Add(heroHoles.Hole2);

                villainGrid.Grid.EliminateConflicts(conflictCards);

                var turnRiverPkResult = PkV2(heroHoles, villainGrid, fiveCards);
                
                pkResult.Add(turnRiverPkResult);
            }

            return pkResult;
        }

        private PkResult PkV2(HoldingHoles heroHoles, PlayerRangePkGrid villainGrid, List<Card> fullBoard)
        {
            var pkResult = new PkResult();
            SuitEnum suit;
            int suitedCount;
            var boardSuited = FullBoardHasThreeSuitedCards(fullBoard, out suit, out suitedCount);

            IHand bestHeroHand = boardSuited ? FindBestHand(heroHoles, fullBoard, suit) : FindBestOffsuitHand(heroHoles, fullBoard);

            foreach (var villainHoles in _holesEnumerator(villainGrid))
            {
                IHand bestVillainHand = boardSuited
                    ? FindBestHand(villainHoles, fullBoard, suit)
                    : FindBestOffsuitHand(villainHoles, fullBoard);

                var pk = bestHeroHand.CompareTo(bestVillainHand);
                if (pk == 0)
                {
                    pkResult.TiedScenariosCount++;
                }
                else if (pk < 0)
                {
                    pkResult.VillainWinScenariosCount++;
                }
                else if (pk > 0)
                {
                    pkResult.HeroWinScenariosCount++;
                }
            }

            return pkResult;
        }

        private IHand FindBestHand(HoldingHoles holes, List<Card> fullBoard, SuitEnum suit)
        {
            var bestOffsuitHand = FindBestOffsuitHand(holes, fullBoard);
            var bestSuitedHand = FindBestSuitedHand(holes, fullBoard, suit);
            return bestSuitedHand == null
                ? bestOffsuitHand
                : (bestSuitedHand.CompareTo(bestOffsuitHand) > 0 ? bestSuitedHand : bestOffsuitHand);
        }

        private IHand FindBestOffsuitHand(HoldingHoles holes, List<Card> fullBoard)
        {
            var ranks = fullBoard.Select(c => c.Rank).ToList();
            ranks.Add(holes.Hole1.Rank);
            ranks.Add(holes.Hole2.Rank);

            return _offsuitHandFinder.FindBestHand(ranks);
        }

        private IHand FindBestSuitedHand(HoldingHoles holes, List<Card> fullBoard, SuitEnum suit)
        {
            var suitedRanks = fullBoard.Where(c => c.Suit == suit).Select(c => c.Rank).ToList();
            if (holes.Hole1.Suit == suit)
            {
                suitedRanks.Add(holes.Hole1.Rank);
            }
            if (holes.Hole2.Suit == suit)
            {
                suitedRanks.Add(holes.Hole2.Rank);
            }
            
            return _suitedHandFinder.FindBestHand(suitedRanks);
        }

        private PkResult Pk(HoldingHoles heroHoles, PlayerRangePkGrid pkGrid, List<Card> fullBoard)
        {
            SuitEnum suit;
            int suitedCount;
            if (FullBoardHasThreeSuitedCards(fullBoard, out suit, out suitedCount))
            {
                return PkOnSuitedBoard(heroHoles, pkGrid, fullBoard, suit);
            }
            else
            {
                return PkOffsuit(heroHoles, pkGrid, fullBoard);
            }
        }

        /// <summary>
        /// PK on a board that has at least three cards of the same suit
        /// </summary>
        /// <param name="heroHoles"></param>
        /// <param name="villainGrid"></param>
        /// <param name="fullBoard"></param>
        /// <param name="suit"></param>
        /// <returns></returns>
        private PkResult PkOnSuitedBoard(HoldingHoles heroHoles, PlayerRangePkGrid villainGrid, List<Card> fullBoard,
            SuitEnum suit)
        {
            var heroOffsuitRanks = fullBoard.Select(c => c.Rank).ToList();
            heroOffsuitRanks.Add(heroHoles.Hole1.Rank);
            heroOffsuitRanks.Add(heroHoles.Hole2.Rank);

            var heroSuitedRanks = fullBoard.Where(c => c.Suit == suit).Select(c => c.Rank).ToList();
            if (heroHoles.Hole1.Suit == suit)
            {
                heroSuitedRanks.Add(heroHoles.Hole1.Rank);
            }
            if (heroHoles.Hole2.Suit == suit)
            {
                heroSuitedRanks.Add(heroHoles.Hole2.Rank);
            }

            var heroOffsuitHand = _offsuitHandFinder.FindBestHand(heroOffsuitRanks);
            var heroSuitedHand = _suitedHandFinder.FindBestHand(heroSuitedRanks);
            IHand heroBestHand = heroSuitedHand == null
                ? heroOffsuitHand
                : (heroSuitedHand.CompareTo(heroOffsuitHand) > 0 ? heroSuitedHand : heroOffsuitHand);

            var pkResult = PkHeroHandOnSuitedBoard(heroBestHand, villainGrid, fullBoard, suit);

            return pkResult;
        }

        private PkResult PkHeroHandOnSuitedBoard(IHand heroBestHand,
            PlayerRangePkGrid villainGrid, List<Card> fullBoard, SuitEnum suit)
        {
            var villainOffsuitRanks = fullBoard.Select(c => c.Rank).ToList();
            villainOffsuitRanks.Add(villainGrid.Grid.HighRank);
            villainOffsuitRanks.Add(villainGrid.Grid.LowRank);
            var villainOffsuitHand = _offsuitHandFinder.FindBestHand(villainOffsuitRanks);
            var pkResult = PkHeroHandWithOffsuitHand(heroBestHand, villainGrid, villainOffsuitHand);

            if (pkResult.HeroWinScenariosCount == 0 && pkResult.TiedScenariosCount == 0 &&
                pkResult.TiedScenariosCount == 0)
            {
                //villain already won, no need to compare anymore
                return pkResult;
            }

            bool tied = pkResult.TiedScenariosCount != 0;

            foreach (var villainSuitedHand in FindBetterSuitedHandsFromOffsuitGrid(villainGrid, villainOffsuitHand, fullBoard, suit))
            {
                var pk = heroBestHand.CompareTo(villainSuitedHand);

                if (pk == 0)
                {
                    pkResult.TiedScenariosCount++;
                }
                else if (pk < 0)
                {
                    pkResult.VillainWinScenariosCount++;
                }

                if (tied)
                {
                    pkResult.TiedScenariosCount--;
                }
                else
                {
                    pkResult.HeroWinScenariosCount--;
                }
            }

            return pkResult;
        }

        private PkResult PkHeroHandWithOffsuitHand(IHand heroBestHand,
            PlayerRangePkGrid villainGrid, IHand villainOffsuitHand)
        {
            int pk = heroBestHand.CompareTo(villainOffsuitHand);

            if (pk > 0)
            {
                return new PkResult(villainGrid.Grid.AvailableRankCombCount, 0, 0, 0);
            }
            else if (pk == 0)
            {
                return new PkResult(0, 0, villainGrid.Grid.AvailableRankCombCount, 0);
            }
            else
            {
                return new PkResult(0, villainGrid.Grid.AvailableRankCombCount, 0, 0);
            }
        }

        private List<IHand> FindBetterSuitedHandsFromOffsuitGrid(PlayerRangePkGrid villainGrid,
            IHand villainOffsuitHand, List<Card> fullBoard, SuitEnum suit)
        {
            var betterSuitedHands = new List<IHand>();
            var villainSuitedRanks = fullBoard.Where(c => c.Suit == suit).Select(c => c.Rank).ToList();

            var hand1 = _suitedHandFinder.FindBestHand(villainSuitedRanks);
            if (hand1 != null && hand1.CompareTo(villainOffsuitHand) > 0)
            {
                betterSuitedHands.Add(hand1);
            }
            
            foreach (var holes in villainGrid.Grid.EnumerateAllPossibleHolesContainingSuit(suit))
            {
                var newVillainSuitedRanks = new List<RankEnum>(villainSuitedRanks);
                if (holes.Item1.Suit == suit) newVillainSuitedRanks.Add(holes.Item1.Rank);
                if (holes.Item2.Suit == suit) newVillainSuitedRanks.Add(holes.Item2.Rank);
                var newHand = _suitedHandFinder.FindBestHand(newVillainSuitedRanks);
                if (newHand != null && newHand.CompareTo(villainOffsuitHand) > 0)
                {
                    betterSuitedHands.Add(newHand);
                }
            }
            
            return betterSuitedHands;
        }

        /// <summary>
        /// PK on a board which doesn't have more than two cards of any suit
        /// </summary>
        /// <param name="heroHoles"></param>
        /// <param name="villainGrid"></param>
        /// <param name="fullBoard"></param>
        /// <returns></returns>
        private PkResult PkOffsuit(HoldingHoles heroHoles, PlayerRangePkGrid villainGrid, List<Card> fullBoard)
        {
            var heroRanks = fullBoard.Select(c => c.Rank).ToList();
            heroRanks.Add(heroHoles.Hole1.Rank);
            heroRanks.Add(heroHoles.Hole2.Rank);
            var heroHand = _offsuitHandFinder.FindBestHand(heroRanks);

            var villainRanks = fullBoard.Select(c => c.Rank).ToList();
            villainRanks.Add(villainGrid.Grid.HighRank);
            villainRanks.Add(villainGrid.Grid.LowRank);
            var villainHand = _offsuitHandFinder.FindBestHand(villainRanks);

            var pk = heroHand.CompareTo(villainHand);
            if (pk > 0)
            {
                return new PkResult(villainGrid.Grid.AvailableRankCombCount, 0, 0, 0);
            }
            else if (pk == 0)
            {
                return new PkResult(0, 0, villainGrid.Grid.AvailableRankCombCount, 0);
            }
            else
            {
                return new PkResult(0, villainGrid.Grid.AvailableRankCombCount, 0, 0);
            }
        }

        private bool ConflictsWithVillainGrid(List<Card> fullBoard, RangeGrid villainGrid)
        {
            if (villainGrid.HighRank == villainGrid.LowRank)
            {
                if (fullBoard.Count(c => c.Rank == villainGrid.HighRank) >= 3)
                {
                    return true;
                }
                return false;
            }

            if (fullBoard.Count(c => c.Rank == villainGrid.HighRank) >= 4)
            {
                return true;
            }

            if (fullBoard.Count(c => c.Rank == villainGrid.LowRank) >= 4)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// At least 3 cards are of the same suit
        /// </summary>
        /// <param name="fullBoard"></param>
        /// <param name="suit"></param>
        /// <returns></returns>
        private bool FullBoardHasThreeSuitedCards(List<Card> fullBoard, out SuitEnum suit, out int suitedCount)
        {
            suit = SuitEnum.Undefined;
            suitedCount = fullBoard.Count(c => c.Suit == SuitEnum.Heart);
            if (suitedCount >= 3)
            {
                suit = SuitEnum.Heart;
                return true;
            }

            suitedCount = fullBoard.Count(c => c.Suit == SuitEnum.Spade);
            if (suitedCount >= 3)
            {
                suit = SuitEnum.Spade;
                return true;
            }

            suitedCount = fullBoard.Count(c => c.Suit == SuitEnum.Diamond);
            if (suitedCount >= 3)
            {
                suit = SuitEnum.Diamond;
                return true;
            }

            suitedCount = fullBoard.Count(c => c.Suit == SuitEnum.Club);
            if (suitedCount >= 3)
            {
                suit = SuitEnum.Club;
                return true;
            }

            return false;
        }
    }
}
