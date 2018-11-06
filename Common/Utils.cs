using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Ranging;

namespace Common
{
    public static class Utils
    {
        public static IEnumerable<List<ProbabilityTuple>> EnumerateProbabilities(int index,
            List<VillainProbabilityResult> probabilityResults)
        {
            if (index == probabilityResults.Count)
            {
                yield break;
            }

            var probabilityResult = probabilityResults[index];
            foreach (var entry in probabilityResult.Probabilities)
            {
                if (index == probabilityResults.Count - 1)
                {
                    var result = new List<ProbabilityTuple>();
                    result.Add(new ProbabilityTuple(probabilityResult.VillainName, entry.Key, entry.Value));
                    yield return result;
                }
                else
                {
                    foreach (var list in EnumerateProbabilities(index + 1, probabilityResults).ToList())
                    {
                        var result = new List<ProbabilityTuple>();
                        var tuple = new ProbabilityTuple(probabilityResult.VillainName, entry.Key, entry.Value);
                        result.Add(tuple);
                        result.AddRange(list);
                        yield return result;
                    }
                }
            }
        }

        public static bool EvProfitable(double ev, double potSize, double betSize)
        {
            return ev > betSize;
        }


        public static int GetBetSize(int currentPotSize)
        {
            return currentPotSize / 2;
        }

        public static double CalculateEv(List<ProbabilityTuple> tuples, int betSize, int potSize)
        {
            double probability = 1;
            foreach (var tuple in tuples)
            {
                probability *= tuple.Probability;
            }

            if (tuples.Any(t => t.ProbabilityCategory == ProbabilityEnum.CallWin))
            {
                return -probability * betSize;
            }

            int callerCount = tuples.Count(t => t.ProbabilityCategory != ProbabilityEnum.Fold);
            int winnerCount = tuples.Count(t => t.ProbabilityCategory == ProbabilityEnum.CallTie) + 1;

            int winSize = potSize + betSize * (callerCount);
            return probability * winSize / winnerCount;
        }

        /// <summary>
        /// See if villain could still possibly fold
        /// </summary>
        /// <param name="raiser"></param>
        /// <param name="hero"></param>
        /// <param name="villain"></param>
        /// <returns></returns>
        public static bool VillainFoldable(PlayerRoundProfile raiser, PlayerRoundProfile hero, PlayerRoundProfile villain)
        {
            if (!villain.IsAlive)
            {
                throw new InvalidOperationException();
            }

            if (villain.PlayerStatus == PlayerStatusEnum.AllIned)
            {
                return false;
            }

            if (villain.Position == raiser.Position)
            {
                return false;
            }

            PositionEnum position = raiser.Position;
            while (true)
            {
                //moving around
                position++;
                if (position == PositionEnum.Total)
                {
                    position = PositionEnum.SmallBlind;
                }

                //villain is in a later position to hero, foldable
                if (position == hero.Position)
                {
                    return true;
                }

                //the villain is in an earlier position to hero, already called, not foldable
                if (position == villain.Position)
                {
                    return false;
                }
            }
        }

        public static bool IsRaiseMove(this DecisionType decisionType)
        {
            return decisionType == DecisionType.AllIn
                   || decisionType == DecisionType.Ante
                   || decisionType == DecisionType.Raise
                   || decisionType == DecisionType.Reraise;
        }

        public static List<Tuple<Card, Card>> EnumerateAllPossibleHoles(this RangeGrid grid)
        {
            switch (grid.Category)
            {
                case GridCategoryEnum.Suited:
                    return grid.EnumerateAllPossibleHolesForSuitedGrid().ToList();
                case GridCategoryEnum.Paired:
                    return grid.EnumerateAllPossibleHolesForPairedGrid().ToList();
                case GridCategoryEnum.Offsuit:
                    return grid.EnumerateAllPossibleHolesForOffsuitGrid().ToList();
                default:
                    throw new InvalidOperationException();
            }
        }

        public static List<Tuple<Card, Card>> EnumerateAllPossibleHolesContainingSuit(this RangeGrid grid, SuitEnum suit)
        {
            return grid.EnumerateAllPossibleHoles().Where(h => (h.Item1.Suit == suit || h.Item2.Suit == suit)).ToList();
        }

        private static IEnumerable<Tuple<Card, Card>> EnumerateAllPossibleHolesForSuitedGrid(this RangeGrid grid)
        {
            var suits = new[] { SuitEnum.Heart, SuitEnum.Spade, SuitEnum.Diamond, SuitEnum.Club, };
            foreach (var suit in suits)
            {
                if (grid.Card1AvaliableSuits.Contains(suit) && grid.Card2AvaliableSuits.Contains(suit))
                    yield return new Tuple<Card, Card>(new Card(suit, grid.HighRank), new Card(suit, grid.LowRank));
            }
        }

        private static IEnumerable<Tuple<Card, Card>> EnumerateAllPossibleHolesForPairedGrid(this RangeGrid grid)
        {
            var suits = new[] { SuitEnum.Heart, SuitEnum.Spade, SuitEnum.Diamond, SuitEnum.Club, };

            for (int i = 0; i < suits.Length; i++)
            {
                for (int j = i + 1; j < suits.Length; j++)
                {
                    var suit1 = suits[i];
                    var suit2 = suits[j];
                    if (grid.Card1AvaliableSuits.Contains(suit1) && grid.Card2AvaliableSuits.Contains(suit2)
                        && suit1 != suit2)
                        yield return new Tuple<Card, Card>(new Card(suit1, grid.HighRank), new Card(suit2, grid.LowRank));
                }
            }
        }

        private static IEnumerable<Tuple<Card, Card>> EnumerateAllPossibleHolesForOffsuitGrid(this RangeGrid grid)
        {
            var suits = new[] { SuitEnum.Heart, SuitEnum.Spade, SuitEnum.Diamond, SuitEnum.Club, };

            for (int i = 0; i < suits.Length; i++)
            {
                for (int j = 0; j < suits.Length; j++)
                {
                    if (i == j) continue;
                    var suit1 = suits[i];
                    var suit2 = suits[j];
                    if (grid.Card1AvaliableSuits.Contains(suit1) && grid.Card2AvaliableSuits.Contains(suit2)
                        && suit1 != suit2)
                        yield return new Tuple<Card, Card>(new Card(suit1, grid.HighRank), new Card(suit2, grid.LowRank));
                }
            }
        }

        public static IEnumerable<HoldingHoles> EnumerateAvailableHoles(PlayerRangePkGrid pkGrid)
        {
            return EnumerateHoles(pkGrid, status => status != PlayerGridPkStatusEnum.NotAvailable).FilterCardAvailabilities(pkGrid);
        } 

        public static IEnumerable<HoldingHoles> EnumerateUnfoldedHoles(PlayerRangePkGrid pkGrid)
        {
            return EnumerateHoles(pkGrid, status => status != PlayerGridPkStatusEnum.Fold && status != PlayerGridPkStatusEnum.NotAvailable)
                .FilterCardAvailabilities(pkGrid);
        }

        private static IEnumerable<HoldingHoles> FilterCardAvailabilities(this IEnumerable<HoldingHoles> holeses, PlayerRangePkGrid pkGrid)
        {
            return holeses.Where(holes => pkGrid.Grid.Card1AvaliableSuits.Contains(holes.Hole1.Suit) &&
                                          pkGrid.Grid.Card2AvaliableSuits.Contains(holes.Hole2.Suit));
        }

        private static IEnumerable<HoldingHoles> EnumerateHoles(PlayerRangePkGrid pkGrid, Func<PlayerGridPkStatusEnum, bool> valid)
        {
            var grid = pkGrid.Grid;
            switch (pkGrid.GridPkStatus.Category)
            {
                case GridCategoryEnum.Offsuit:
                    if (valid(pkGrid.GridPkStatus.OffsuitStatus.HeartSpadeStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Heart, grid.HighRank), new Card(SuitEnum.Spade, grid.LowRank));
                    }
                    if (valid(pkGrid.GridPkStatus.OffsuitStatus.HeartDiamondStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Heart, grid.HighRank), new Card(SuitEnum.Diamond, grid.LowRank));
                    }
                    if (valid(pkGrid.GridPkStatus.OffsuitStatus.HeartClubStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Heart, grid.HighRank), new Card(SuitEnum.Club, grid.LowRank));
                    }

                    if (valid(pkGrid.GridPkStatus.OffsuitStatus.SpadeHeartStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Spade, grid.HighRank), new Card(SuitEnum.Heart, grid.LowRank));
                    }
                    if (valid(pkGrid.GridPkStatus.OffsuitStatus.SpadeDiamondStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Spade, grid.HighRank), new Card(SuitEnum.Diamond, grid.LowRank));
                    }
                    if (valid(pkGrid.GridPkStatus.OffsuitStatus.SpadeClubStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Spade, grid.HighRank), new Card(SuitEnum.Club, grid.LowRank));
                    }

                    if (valid(pkGrid.GridPkStatus.OffsuitStatus.DiamondHeartStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Diamond, grid.HighRank), new Card(SuitEnum.Heart, grid.LowRank));
                    }
                    if (valid(pkGrid.GridPkStatus.OffsuitStatus.DiamondSpadeStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Diamond, grid.HighRank), new Card(SuitEnum.Spade, grid.LowRank));
                    }
                    if (valid(pkGrid.GridPkStatus.OffsuitStatus.DiamondClubStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Diamond, grid.HighRank), new Card(SuitEnum.Club, grid.LowRank));
                    }

                    if (valid(pkGrid.GridPkStatus.OffsuitStatus.ClubHeartStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Club, grid.HighRank), new Card(SuitEnum.Heart, grid.LowRank));
                    }
                    if (valid(pkGrid.GridPkStatus.OffsuitStatus.ClubSpadeStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Club, grid.HighRank), new Card(SuitEnum.Spade, grid.LowRank));
                    }
                    if (valid(pkGrid.GridPkStatus.OffsuitStatus.ClubDiamondStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Club, grid.HighRank), new Card(SuitEnum.Diamond, grid.LowRank));
                    }
                    break;
                case GridCategoryEnum.Paired:
                    if (valid(pkGrid.GridPkStatus.PairedStatus.HeartSpadeStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Heart, grid.HighRank), new Card(SuitEnum.Spade, grid.LowRank));
                    }
                    if (valid(pkGrid.GridPkStatus.PairedStatus.HeartDiamondStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Heart, grid.HighRank), new Card(SuitEnum.Diamond, grid.LowRank));
                    }
                    if (valid(pkGrid.GridPkStatus.PairedStatus.HeartClubStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Heart, grid.HighRank), new Card(SuitEnum.Club, grid.LowRank));
                    }
                    if (valid(pkGrid.GridPkStatus.PairedStatus.SpadeDiamondStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Spade, grid.HighRank), new Card(SuitEnum.Diamond, grid.LowRank));
                    }
                    if (valid(pkGrid.GridPkStatus.PairedStatus.SpadeClubStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Spade, grid.HighRank), new Card(SuitEnum.Club, grid.LowRank));
                    }
                    if (valid(pkGrid.GridPkStatus.PairedStatus.DiamondClubStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Diamond, grid.HighRank), new Card(SuitEnum.Club, grid.LowRank));
                    }
                    break;
                case GridCategoryEnum.Suited:
                    if (valid(pkGrid.GridPkStatus.SuitedStatus.HeartStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Heart, grid.HighRank), new Card(SuitEnum.Heart, grid.LowRank));
                    }
                    if (valid(pkGrid.GridPkStatus.SuitedStatus.SpadeStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Spade, grid.HighRank), new Card(SuitEnum.Spade, grid.LowRank));
                    }
                    if (valid(pkGrid.GridPkStatus.SuitedStatus.DiamondStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Diamond, grid.HighRank), new Card(SuitEnum.Diamond, grid.LowRank));
                    }
                    if (valid(pkGrid.GridPkStatus.SuitedStatus.ClubStatus))
                    {
                        yield return new HoldingHoles(new Card(SuitEnum.Club, grid.HighRank), new Card(SuitEnum.Club, grid.LowRank));
                    }
                    break;
            }
        }

        public static IEnumerable<T> EnumerateMultiDimensionalArray<T>(this T[,] array)
        {
            foreach (var element in array)
            {
                yield return element;
            }
        }
    }
}
