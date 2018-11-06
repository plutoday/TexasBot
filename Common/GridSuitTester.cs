using System;
using System.Linq;
using Models;
using Models.Ranging;

namespace Common
{
    public class GridSuitTester
    {
        private readonly Func<Card, Card, Tuple<SuitHandGradeEnum, int>> _previousRoundTest;
        private readonly Func<Card, Card, Tuple<SuitHandGradeEnum, int>> _currentRoundTest;

        public GridSuitTester(Func<Card, Card, Tuple<SuitHandGradeEnum, int>> previousRoundTest,
            Func<Card, Card, Tuple<SuitHandGradeEnum, int>> currentRoundTest)
        {
            _previousRoundTest = previousRoundTest;
            _currentRoundTest = currentRoundTest;
        }

        public SuitedStatus<GridHitNewRoundResultEnum> TestSuitedGrid(RangeGrid grid)
        {
            if (grid.Category != GridCategoryEnum.Suited)
            {
                throw new InvalidOperationException($"{grid.Category}");
            }
            var result = new SuitedStatus<GridHitNewRoundResultEnum>();
            var suits = grid.Card1AvaliableSuits.Intersect(grid.Card2AvaliableSuits);
            foreach (var suit in suits)
            {
                if (!(grid.Card1AvaliableSuits.Contains(suit) && grid.Card2AvaliableSuits.Contains(suit)))
                {
                    switch (suit)
                    {
                        case SuitEnum.Heart:
                            result.HeartStatus = GridHitNewRoundResultEnum.Unavailable;
                            break;
                        case SuitEnum.Spade:
                            result.SpadeStatus = GridHitNewRoundResultEnum.Unavailable;
                            break;
                        case SuitEnum.Diamond:
                            result.DiamondStatus = GridHitNewRoundResultEnum.Unavailable;
                            break;
                        case SuitEnum.Club:
                            result.ClubStatus = GridHitNewRoundResultEnum.Unavailable;
                            break;
                    }
                    continue;
                }

                var hole1 = new Card(suit, grid.HighRank);
                var hole2 = new Card(suit, grid.LowRank);

                var lastRoundTuple = _previousRoundTest(hole1, hole2);
                var currentRoundTuple = _currentRoundTest(hole1, hole2);

                switch (suit)
                {
                    case SuitEnum.Heart:
                        result.HeartStatus = TranslateOutcomeToGridStatus(currentRoundTuple.Item1, currentRoundTuple.Item2, lastRoundTuple.Item1, lastRoundTuple.Item2);
                        break;
                    case SuitEnum.Spade:
                        result.SpadeStatus = TranslateOutcomeToGridStatus(currentRoundTuple.Item1, currentRoundTuple.Item2, lastRoundTuple.Item1, lastRoundTuple.Item2);
                        break;
                    case SuitEnum.Diamond:
                        result.DiamondStatus = TranslateOutcomeToGridStatus(currentRoundTuple.Item1, currentRoundTuple.Item2, lastRoundTuple.Item1, lastRoundTuple.Item2);
                        break;
                    case SuitEnum.Club:
                        result.ClubStatus = TranslateOutcomeToGridStatus(currentRoundTuple.Item1, currentRoundTuple.Item2, lastRoundTuple.Item1, lastRoundTuple.Item2);
                        break;
                }
            }

            return result;
        }

        public PairedStatus<GridHitNewRoundResultEnum> TestPairedGrid(RangeGrid grid)
        {
            if (grid.Category != GridCategoryEnum.Paired)
            {
                throw new InvalidOperationException();
            }
            var result = new PairedStatus<GridHitNewRoundResultEnum>();
            Card hole1, hole2;
            Tuple<SuitHandGradeEnum, int> currentTuple, previousTuple;
            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Heart) && grid.Card2AvaliableSuits.Contains(SuitEnum.Spade))
            {
                hole1 = new Card(SuitEnum.Heart, grid.HighRank);
                hole2 = new Card(SuitEnum.Spade, grid.HighRank);
                currentTuple = _currentRoundTest(hole1, hole2);
                previousTuple = _previousRoundTest(hole1, hole2);
                result.HeartSpadeStatus = TranslateOutcomeToGridStatus(currentTuple.Item1, currentTuple.Item2, previousTuple.Item1, previousTuple.Item2);
            }
            else
            {
                result.HeartSpadeStatus = GridHitNewRoundResultEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Heart) && grid.Card2AvaliableSuits.Contains(SuitEnum.Diamond))
            {
                hole1 = new Card(SuitEnum.Heart, grid.HighRank);
                hole2 = new Card(SuitEnum.Diamond, grid.HighRank);
                currentTuple = _currentRoundTest(hole1, hole2);
                previousTuple = _previousRoundTest(hole1, hole2);
                result.HeartDiamondStatus = TranslateOutcomeToGridStatus(currentTuple.Item1, currentTuple.Item2, previousTuple.Item1, previousTuple.Item2);
            }
            else
            {
                result.HeartDiamondStatus = GridHitNewRoundResultEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Heart) && grid.Card2AvaliableSuits.Contains(SuitEnum.Club))
            {
                hole1 = new Card(SuitEnum.Heart, grid.HighRank);
                hole2 = new Card(SuitEnum.Club, grid.HighRank);
                currentTuple = _currentRoundTest(hole1, hole2);
                previousTuple = _previousRoundTest(hole1, hole2);
                result.HeartClubStatus = TranslateOutcomeToGridStatus(currentTuple.Item1, currentTuple.Item2, previousTuple.Item1, previousTuple.Item2);
            }
            else
            {
                result.HeartClubStatus = GridHitNewRoundResultEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Spade) && grid.Card2AvaliableSuits.Contains(SuitEnum.Diamond))
            {
                hole1 = new Card(SuitEnum.Spade, grid.HighRank);
                hole2 = new Card(SuitEnum.Diamond, grid.HighRank);
                currentTuple = _currentRoundTest(hole1, hole2);
                previousTuple = _previousRoundTest(hole1, hole2);
                result.SpadeDiamondStatus = TranslateOutcomeToGridStatus(currentTuple.Item1, currentTuple.Item2, previousTuple.Item1, previousTuple.Item2);
            }
            else
            {
                result.SpadeDiamondStatus = GridHitNewRoundResultEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Spade) && grid.Card2AvaliableSuits.Contains(SuitEnum.Club))
            {
                hole1 = new Card(SuitEnum.Spade, grid.HighRank);
                hole2 = new Card(SuitEnum.Club, grid.HighRank);
                currentTuple = _currentRoundTest(hole1, hole2);
                previousTuple = _previousRoundTest(hole1, hole2);
                result.SpadeClubStatus = TranslateOutcomeToGridStatus(currentTuple.Item1, currentTuple.Item2, previousTuple.Item1, previousTuple.Item2);
            }
            else
            {
                result.SpadeClubStatus = GridHitNewRoundResultEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Diamond) && grid.Card2AvaliableSuits.Contains(SuitEnum.Club))
            {
                hole1 = new Card(SuitEnum.Diamond, grid.HighRank);
                hole2 = new Card(SuitEnum.Club, grid.HighRank);
                currentTuple = _currentRoundTest(hole1, hole2);
                previousTuple = _previousRoundTest(hole1, hole2);
                result.DiamondClubStatus = TranslateOutcomeToGridStatus(currentTuple.Item1, currentTuple.Item2, previousTuple.Item1, previousTuple.Item2);
            }
            else
            {
                result.DiamondClubStatus = GridHitNewRoundResultEnum.Unavailable;
            }

            return result;
        }

        public OffsuitStatus<GridHitNewRoundResultEnum> TestOffsuitGrid(RangeGrid grid)
        {
            if (grid.Category != GridCategoryEnum.Offsuit)
            {
                throw new InvalidOperationException();
            }
            var result = new OffsuitStatus<GridHitNewRoundResultEnum>();
            Card hole1;
            Card hole2;
            Tuple<SuitHandGradeEnum, int> currentTuple;
            Tuple<SuitHandGradeEnum, int> previousTuple;

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Heart) && grid.Card2AvaliableSuits.Contains(SuitEnum.Spade))
            {
                hole1 = new Card(SuitEnum.Heart, grid.HighRank);
                hole2 = new Card(SuitEnum.Spade, grid.LowRank);
                currentTuple = _currentRoundTest(hole1, hole2);
                previousTuple = _previousRoundTest(hole1, hole2);
                result.HeartSpadeStatus = TranslateOutcomeToGridStatus(currentTuple.Item1, currentTuple.Item2, previousTuple.Item1, previousTuple.Item2);
            }
            else
            {
                result.HeartSpadeStatus = GridHitNewRoundResultEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Heart) && grid.Card2AvaliableSuits.Contains(SuitEnum.Diamond))
            {
                hole1 = new Card(SuitEnum.Heart, grid.HighRank);
                hole2 = new Card(SuitEnum.Diamond, grid.LowRank);
                currentTuple = _currentRoundTest(hole1, hole2);
                previousTuple = _previousRoundTest(hole1, hole2);
                result.HeartDiamondStatus = TranslateOutcomeToGridStatus(currentTuple.Item1, currentTuple.Item2, previousTuple.Item1, previousTuple.Item2);
            }
            else
            {
                result.HeartDiamondStatus = GridHitNewRoundResultEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Heart) && grid.Card2AvaliableSuits.Contains(SuitEnum.Club))
            {
                hole1 = new Card(SuitEnum.Heart, grid.HighRank);
                hole2 = new Card(SuitEnum.Club, grid.LowRank);
                currentTuple = _currentRoundTest(hole1, hole2);
                previousTuple = _previousRoundTest(hole1, hole2);
                result.HeartClubStatus = TranslateOutcomeToGridStatus(currentTuple.Item1, currentTuple.Item2, previousTuple.Item1, previousTuple.Item2);
            }
            else
            {
                result.HeartClubStatus = GridHitNewRoundResultEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Spade) && grid.Card2AvaliableSuits.Contains(SuitEnum.Diamond))
            {
                hole1 = new Card(SuitEnum.Spade, grid.HighRank);
                hole2 = new Card(SuitEnum.Diamond, grid.LowRank);
                currentTuple = _currentRoundTest(hole1, hole2);
                previousTuple = _previousRoundTest(hole1, hole2);
                result.SpadeDiamondStatus = TranslateOutcomeToGridStatus(currentTuple.Item1, currentTuple.Item2, previousTuple.Item1, previousTuple.Item2);
            }
            else
            {
                result.SpadeDiamondStatus = GridHitNewRoundResultEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Spade) && grid.Card2AvaliableSuits.Contains(SuitEnum.Club))
            {
                hole1 = new Card(SuitEnum.Spade, grid.HighRank);
                hole2 = new Card(SuitEnum.Club, grid.LowRank);
                currentTuple = _currentRoundTest(hole1, hole2);
                previousTuple = _previousRoundTest(hole1, hole2);
                result.SpadeClubStatus = TranslateOutcomeToGridStatus(currentTuple.Item1, currentTuple.Item2, previousTuple.Item1, previousTuple.Item2);
            }
            else
            {
                result.SpadeClubStatus = GridHitNewRoundResultEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Diamond) && grid.Card2AvaliableSuits.Contains(SuitEnum.Club))
            {
                hole1 = new Card(SuitEnum.Diamond, grid.HighRank);
                hole2 = new Card(SuitEnum.Club, grid.LowRank);
                currentTuple = _currentRoundTest(hole1, hole2);
                previousTuple = _previousRoundTest(hole1, hole2);
                result.DiamondClubStatus = TranslateOutcomeToGridStatus(currentTuple.Item1, currentTuple.Item2, previousTuple.Item1, previousTuple.Item2);
            }
            else
            {
                result.DiamondClubStatus = GridHitNewRoundResultEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Spade) && grid.Card2AvaliableSuits.Contains(SuitEnum.Heart))
            {
                hole1 = new Card(SuitEnum.Spade, grid.HighRank);
                hole2 = new Card(SuitEnum.Heart, grid.LowRank);
                currentTuple = _currentRoundTest(hole1, hole2);
                previousTuple = _previousRoundTest(hole1, hole2);
                result.SpadeHeartStatus = TranslateOutcomeToGridStatus(currentTuple.Item1, currentTuple.Item2, previousTuple.Item1, previousTuple.Item2);
            }
            else
            {
                result.SpadeHeartStatus = GridHitNewRoundResultEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Diamond) && grid.Card2AvaliableSuits.Contains(SuitEnum.Heart))
            {
                hole1 = new Card(SuitEnum.Diamond, grid.HighRank);
                hole2 = new Card(SuitEnum.Heart, grid.LowRank);
                currentTuple = _currentRoundTest(hole1, hole2);
                previousTuple = _previousRoundTest(hole1, hole2);
                result.DiamondHeartStatus = TranslateOutcomeToGridStatus(currentTuple.Item1, currentTuple.Item2, previousTuple.Item1, previousTuple.Item2);
            }
            else
            {
                result.DiamondHeartStatus = GridHitNewRoundResultEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Club) && grid.Card2AvaliableSuits.Contains(SuitEnum.Heart))
            {
                hole1 = new Card(SuitEnum.Club, grid.HighRank);
                hole2 = new Card(SuitEnum.Heart, grid.LowRank);
                currentTuple = _currentRoundTest(hole1, hole2);
                previousTuple = _previousRoundTest(hole1, hole2);
                result.ClubHeartStatus = TranslateOutcomeToGridStatus(currentTuple.Item1, currentTuple.Item2, previousTuple.Item1, previousTuple.Item2);
            }
            else
            {
                result.ClubHeartStatus = GridHitNewRoundResultEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Diamond) && grid.Card2AvaliableSuits.Contains(SuitEnum.Spade))
            {
                hole1 = new Card(SuitEnum.Diamond, grid.HighRank);
                hole2 = new Card(SuitEnum.Spade, grid.LowRank);
                currentTuple = _currentRoundTest(hole1, hole2);
                previousTuple = _previousRoundTest(hole1, hole2);
                result.DiamondSpadeStatus = TranslateOutcomeToGridStatus(currentTuple.Item1, currentTuple.Item2, previousTuple.Item1, previousTuple.Item2);
            }
            else
            {
                result.DiamondSpadeStatus = GridHitNewRoundResultEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Club) && grid.Card2AvaliableSuits.Contains(SuitEnum.Spade))
            {

                hole1 = new Card(SuitEnum.Club, grid.HighRank);
                hole2 = new Card(SuitEnum.Spade, grid.LowRank);
                currentTuple = _currentRoundTest(hole1, hole2);
                previousTuple = _previousRoundTest(hole1, hole2);
                result.ClubSpadeStatus = TranslateOutcomeToGridStatus(currentTuple.Item1, currentTuple.Item2, previousTuple.Item1, previousTuple.Item2);
            }
            else
            {
                result.ClubSpadeStatus = GridHitNewRoundResultEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Club) && grid.Card2AvaliableSuits.Contains(SuitEnum.Diamond))
            {
                hole1 = new Card(SuitEnum.Club, grid.HighRank);
                hole2 = new Card(SuitEnum.Diamond, grid.LowRank);
                currentTuple = _currentRoundTest(hole1, hole2);
                previousTuple = _previousRoundTest(hole1, hole2);
                result.ClubDiamondStatus = TranslateOutcomeToGridStatus(currentTuple.Item1, currentTuple.Item2, previousTuple.Item1, previousTuple.Item2);
            }
            else
            {
                result.ClubDiamondStatus = GridHitNewRoundResultEnum.Unavailable;
            }

            return result;
        }

        private GridHitNewRoundResultEnum TranslateOutcomeToGridStatus(SuitHandGradeEnum currentGrade, int currentInvolved,
            SuitHandGradeEnum lastGrade, int lastInvolved)
        {
            if (currentGrade > lastGrade)
            {
                return currentInvolved > lastInvolved ? GridHitNewRoundResultEnum.Promoted : GridHitNewRoundResultEnum.Enhanced;
            }
            return GridHitNewRoundResultEnum.None;
        }
    }
}
