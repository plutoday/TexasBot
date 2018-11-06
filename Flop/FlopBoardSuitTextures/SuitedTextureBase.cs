using System;
using System.Collections.Generic;
using Models;
using Models.Ranging;

namespace Flop.FlopBoardSuitTextures
{
    public abstract class SuitedTextureBase : IFlopBoardSuitTexture
    {
        public abstract Dictionary<Tuple<SuitEnum, SuitEnum>, bool> ShouldAGridFoldToBet(RangeGrid grid);

        public SuitedStatus<BoardRangeGridStatusEnum> TestSuitedGrid(RangeGrid grid)
        {
            if (grid.Category != GridCategoryEnum.Suited)
            {
                throw new InvalidOperationException($"{grid.Category}");
            }
            var result = new SuitedStatus<BoardRangeGridStatusEnum>();
            var suits = new SuitEnum[] { SuitEnum.Heart, SuitEnum.Spade, SuitEnum.Diamond, SuitEnum.Club };
            foreach (var suit in suits)
            {
                if (!(grid.Card1AvaliableSuits.Contains(suit) && grid.Card2AvaliableSuits.Contains(suit)))
                {
                    switch (suit)
                    {
                        case SuitEnum.Heart:
                            result.HeartStatus = BoardRangeGridStatusEnum.Unavailable;
                            break;
                        case SuitEnum.Spade:
                            result.SpadeStatus = BoardRangeGridStatusEnum.Unavailable;
                            break;
                        case SuitEnum.Diamond:
                            result.DiamondStatus = BoardRangeGridStatusEnum.Unavailable;
                            break;
                        case SuitEnum.Club:
                            result.ClubStatus = BoardRangeGridStatusEnum.Unavailable;
                            break;
                    }
                    continue;
                }

                var hole1 = new Card(suit, grid.HighRank);
                var hole2 = new Card(suit, grid.LowRank);

                var outcome = TestGridAgainstBoard(hole1, hole2).Item1;
                switch (suit)
                {
                    case SuitEnum.Heart:
                        result.HeartStatus = TranslateOutcomeToGridStatus(outcome);
                        break;
                    case SuitEnum.Spade:
                        result.SpadeStatus = TranslateOutcomeToGridStatus(outcome);
                        break;
                    case SuitEnum.Diamond:
                        result.DiamondStatus = TranslateOutcomeToGridStatus(outcome);
                        break;
                    case SuitEnum.Club:
                        result.ClubStatus = TranslateOutcomeToGridStatus(outcome);
                        break;
                }
            }

            return result;
        }

        protected abstract BoardRangeGridStatusEnum TranslateOutcomeToGridStatus(SuitTextureOutcomeEnum outcome);

        public abstract Tuple<SuitTextureOutcomeEnum, int> TestGridAgainstBoard(Card hole1, Card hole2);

        public PairedStatus<BoardRangeGridStatusEnum> TestPairedGrid(RangeGrid grid)
        {
            if (grid.Category != GridCategoryEnum.Paired)
            {
                throw new InvalidOperationException();
            }
            var result = new PairedStatus<BoardRangeGridStatusEnum>();
            Card hole1, hole2;

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Heart) && grid.Card2AvaliableSuits.Contains(SuitEnum.Spade))
            {
                hole1 = new Card(SuitEnum.Heart, grid.HighRank);
                hole2 = new Card(SuitEnum.Spade, grid.HighRank);
                result.HeartSpadeStatus = TranslateOutcomeToGridStatus(TestGridAgainstBoard(hole1, hole2).Item1);
            }
            else
            {
                result.HeartSpadeStatus = BoardRangeGridStatusEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Heart) && grid.Card2AvaliableSuits.Contains(SuitEnum.Diamond))
            {
                hole1 = new Card(SuitEnum.Heart, grid.HighRank);
                hole2 = new Card(SuitEnum.Diamond, grid.HighRank);
                result.HeartDiamondStatus = TranslateOutcomeToGridStatus(TestGridAgainstBoard(hole1, hole2).Item1);
            }
            else
            {
                result.HeartDiamondStatus = BoardRangeGridStatusEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Heart) && grid.Card2AvaliableSuits.Contains(SuitEnum.Club))
            {
                hole1 = new Card(SuitEnum.Heart, grid.HighRank);
                hole2 = new Card(SuitEnum.Club, grid.HighRank);
                result.HeartClubStatus = TranslateOutcomeToGridStatus(TestGridAgainstBoard(hole1, hole2).Item1);
            }
            else
            {
                result.HeartClubStatus = BoardRangeGridStatusEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Spade) && grid.Card2AvaliableSuits.Contains(SuitEnum.Diamond))
            {
                hole1 = new Card(SuitEnum.Spade, grid.HighRank);
                hole2 = new Card(SuitEnum.Diamond, grid.HighRank);
                result.SpadeDiamondStatus = TranslateOutcomeToGridStatus(TestGridAgainstBoard(hole1, hole2).Item1);
            }
            else
            {
                result.SpadeDiamondStatus = BoardRangeGridStatusEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Spade) && grid.Card2AvaliableSuits.Contains(SuitEnum.Club))
            {
                hole1 = new Card(SuitEnum.Spade, grid.HighRank);
                hole2 = new Card(SuitEnum.Club, grid.HighRank);
                result.SpadeClubStatus = TranslateOutcomeToGridStatus(TestGridAgainstBoard(hole1, hole2).Item1);
            }
            else
            {
                result.SpadeClubStatus = BoardRangeGridStatusEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Diamond) && grid.Card2AvaliableSuits.Contains(SuitEnum.Club))
            {
                hole1 = new Card(SuitEnum.Diamond, grid.HighRank);
                hole2 = new Card(SuitEnum.Club, grid.HighRank);
                result.DiamondClubStatus = TranslateOutcomeToGridStatus(TestGridAgainstBoard(hole1, hole2).Item1);
            }
            else
            {
                result.DiamondClubStatus = BoardRangeGridStatusEnum.Unavailable;
            }

            return result;
        }

        public OffsuitStatus<BoardRangeGridStatusEnum> TestOffsuitGrid(RangeGrid grid)
        {
            if (grid.Category != GridCategoryEnum.Offsuit)
            {
                throw new InvalidOperationException();
            }
            var result = new OffsuitStatus<BoardRangeGridStatusEnum>();
            Card hole1, hole2;

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Heart) && grid.Card2AvaliableSuits.Contains(SuitEnum.Spade))
            {
                hole1 = new Card(SuitEnum.Heart, grid.HighRank);
                hole2 = new Card(SuitEnum.Spade, grid.LowRank);
                result.HeartSpadeStatus = TranslateOutcomeToGridStatus(TestGridAgainstBoard(hole1, hole2).Item1);
            }
            else
            {
                result.HeartSpadeStatus = BoardRangeGridStatusEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Heart) && grid.Card2AvaliableSuits.Contains(SuitEnum.Diamond))
            {
                hole1 = new Card(SuitEnum.Heart, grid.HighRank);
                hole2 = new Card(SuitEnum.Diamond, grid.LowRank);
                result.HeartDiamondStatus = TranslateOutcomeToGridStatus(TestGridAgainstBoard(hole1, hole2).Item1);
            }
            else
            {
                result.HeartDiamondStatus = BoardRangeGridStatusEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Heart) && grid.Card2AvaliableSuits.Contains(SuitEnum.Club))
            {

                hole1 = new Card(SuitEnum.Heart, grid.HighRank);
                hole2 = new Card(SuitEnum.Club, grid.LowRank);
                result.HeartClubStatus = TranslateOutcomeToGridStatus(TestGridAgainstBoard(hole1, hole2).Item1);
            }
            else
            {
                result.HeartClubStatus = BoardRangeGridStatusEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Spade) && grid.Card2AvaliableSuits.Contains(SuitEnum.Diamond))
            {

                hole1 = new Card(SuitEnum.Spade, grid.HighRank);
                hole2 = new Card(SuitEnum.Diamond, grid.LowRank);
                result.SpadeDiamondStatus = TranslateOutcomeToGridStatus(TestGridAgainstBoard(hole1, hole2).Item1);
            }
            else
            {
                result.SpadeDiamondStatus = BoardRangeGridStatusEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Spade) && grid.Card2AvaliableSuits.Contains(SuitEnum.Club))
            {

                hole1 = new Card(SuitEnum.Spade, grid.HighRank);
                hole2 = new Card(SuitEnum.Club, grid.LowRank);
                result.SpadeClubStatus = TranslateOutcomeToGridStatus(TestGridAgainstBoard(hole1, hole2).Item1);
            }
            else
            {
                result.SpadeClubStatus = BoardRangeGridStatusEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Diamond) && grid.Card2AvaliableSuits.Contains(SuitEnum.Club))
            {

                hole1 = new Card(SuitEnum.Diamond, grid.HighRank);
                hole2 = new Card(SuitEnum.Club, grid.LowRank);
                result.DiamondClubStatus = TranslateOutcomeToGridStatus(TestGridAgainstBoard(hole1, hole2).Item1);
            }
            else
            {
                result.DiamondClubStatus = BoardRangeGridStatusEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Spade) && grid.Card2AvaliableSuits.Contains(SuitEnum.Heart))
            {
                hole1 = new Card(SuitEnum.Spade, grid.HighRank);
                hole2 = new Card(SuitEnum.Heart, grid.LowRank);
                result.SpadeHeartStatus = TranslateOutcomeToGridStatus(TestGridAgainstBoard(hole1, hole2).Item1);
            }
            else
            {
                result.SpadeHeartStatus = BoardRangeGridStatusEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Diamond) && grid.Card2AvaliableSuits.Contains(SuitEnum.Heart))
            {

                hole1 = new Card(SuitEnum.Diamond, grid.HighRank);
                hole2 = new Card(SuitEnum.Heart, grid.LowRank);
                result.DiamondHeartStatus = TranslateOutcomeToGridStatus(TestGridAgainstBoard(hole1, hole2).Item1);
            }
            else
            {
                result.DiamondHeartStatus = BoardRangeGridStatusEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Club) && grid.Card2AvaliableSuits.Contains(SuitEnum.Heart))
            {

                hole1 = new Card(SuitEnum.Club, grid.HighRank);
                hole2 = new Card(SuitEnum.Heart, grid.LowRank);
                result.ClubHeartStatus = TranslateOutcomeToGridStatus(TestGridAgainstBoard(hole1, hole2).Item1);
            }
            else
            {
                result.ClubHeartStatus = BoardRangeGridStatusEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Diamond) && grid.Card2AvaliableSuits.Contains(SuitEnum.Spade))
            {

                hole1 = new Card(SuitEnum.Diamond, grid.HighRank);
                hole2 = new Card(SuitEnum.Spade, grid.LowRank);
                result.DiamondSpadeStatus = TranslateOutcomeToGridStatus(TestGridAgainstBoard(hole1, hole2).Item1);
            }
            else
            {
                result.DiamondSpadeStatus = BoardRangeGridStatusEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Club) && grid.Card2AvaliableSuits.Contains(SuitEnum.Spade))
            {

                hole1 = new Card(SuitEnum.Club, grid.HighRank);
                hole2 = new Card(SuitEnum.Spade, grid.LowRank);
                result.ClubSpadeStatus = TranslateOutcomeToGridStatus(TestGridAgainstBoard(hole1, hole2).Item1);
            }
            else
            {
                result.ClubSpadeStatus = BoardRangeGridStatusEnum.Unavailable;
            }

            if (grid.Card1AvaliableSuits.Contains(SuitEnum.Club) && grid.Card2AvaliableSuits.Contains(SuitEnum.Diamond))
            {

                hole1 = new Card(SuitEnum.Club, grid.HighRank);
                hole2 = new Card(SuitEnum.Diamond, grid.LowRank);
                result.ClubDiamondStatus = TranslateOutcomeToGridStatus(TestGridAgainstBoard(hole1, hole2).Item1);
            }
            else
            {
                result.ClubDiamondStatus = BoardRangeGridStatusEnum.Unavailable;
            }

            return result;
        }
    }
}
