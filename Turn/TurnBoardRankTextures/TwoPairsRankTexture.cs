using System;
using Flop;
using Flop.FlopBoardRankTextures;
using Models;
using Models.Ranging;

namespace Turn.TurnBoardRankTextures
{
    public class TwoPairsRankTexture : ITurnFlopBoardRankTexture
    {
        public FlopBoard FlopBoard { get; set; }
        public Card HighPairCard1 { get; set; }
        public Card HighPairCard2 { get; set; }
        public Card LowPairCard1 { get; set; }
        public Card LowPairCard2 { get; set; }

        public TwoPairsRankTexture(TurnBoard turnBoard)
        {
            FlopBoard = turnBoard.FlopBoard;
            if (FlopBoard.RankTexture == FlopBoardRankTextureEnum.HighPair)
            {
                var highPairBoard = new Flop.FlopBoardRankTextures.HighPairRankTexture(FlopBoard);
                if (highPairBoard.SingleCard.Rank != turnBoard.TurnCard.Rank)
                {
                    throw new InvalidOperationException();
                }
                HighPairCard1 = highPairBoard.PairCard1;
                HighPairCard2 = highPairBoard.PairCard2;
                LowPairCard1 = highPairBoard.SingleCard;
                LowPairCard2 = turnBoard.TurnCard;
            }
            else if (FlopBoard.RankTexture == FlopBoardRankTextureEnum.LowPair)
            {
                var lowPairBoard = new Flop.FlopBoardRankTextures.LowPairRankTexture(FlopBoard);
                if (lowPairBoard.SingleCard.Rank != turnBoard.TurnCard.Rank)
                {
                    throw new InvalidOperationException();
                }
                HighPairCard1 = lowPairBoard.SingleCard;
                HighPairCard2 = turnBoard.TurnCard;
                LowPairCard1 = lowPairBoard.PairCard1;
                LowPairCard2 = lowPairBoard.PairCard2;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public BoardRangeGridStatusEnum TestGridAgainstFlopBoard(RangeGrid grid)
        {
            throw new NotImplementedException();
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            return outcome.Grade() <= RankHandGradeEnum.TwoPairs && outcome == TwoPairsOutcomeEnum.WeakKicker;
        }

        public GridHitNewRoundResultEnum HitTurn(RangeGrid grid)
        {
            RankHandGradeEnum flopGrade;
            int flopInvolved;
            switch (FlopBoard.RankTexture)
            {
                case FlopBoardRankTextureEnum.LowPair:
                    var lowPairTuple = new Flop.FlopBoardRankTextures.LowPairRankTexture(FlopBoard).TestGridAgainstBoard(grid);
                    flopGrade = lowPairTuple.Item1.Grade();
                    flopInvolved = lowPairTuple.Item2;
                    break;
                case FlopBoardRankTextureEnum.HighPair:
                    var highPairTuple = new Flop.FlopBoardRankTextures.HighPairRankTexture(FlopBoard).TestGridAgainstBoard(grid);
                    flopGrade = highPairTuple.Item1.Grade();
                    flopInvolved = highPairTuple.Item2;
                    break;
                default:
                    throw new InvalidOperationException($"Impossible to have {FlopBoard.RankTexture}");
            }
            var turnTuple = TestGridAgainstBoard(grid);
            var turnOutcomeGrade = turnTuple.Item1.Grade();
            var turnInvolved = turnTuple.Item2;

            if (turnOutcomeGrade > flopGrade)
            {
                return (turnInvolved > flopInvolved) ? GridHitNewRoundResultEnum.Promoted : GridHitNewRoundResultEnum.Enhanced;
            }
            return GridHitNewRoundResultEnum.None;
        }

        public Tuple<TwoPairsOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {/*
        HighFoursome,
        LowFoursome,
        HighFullHouse,
        LowFullHouse,
        OverTwoPairs,
        HighBetweenTwoPairs,
        TopKicker,
        OverGoodKicker,
        OverWeakKicker,
        NoneKicker*/
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == HighPairCard1.Rank)
                {
                    return new Tuple<TwoPairsOutcomeEnum, int>(TwoPairsOutcomeEnum.HighFoursome, 2);
                }
                if (grid.HighRank == LowPairCard1.Rank)
                {
                    return new Tuple<TwoPairsOutcomeEnum, int>(TwoPairsOutcomeEnum.LowFoursome, 2);
                }
            }

            if (grid.HighRank == HighPairCard1.Rank || grid.LowRank == HighPairCard1.Rank)
            {
                return new Tuple<TwoPairsOutcomeEnum, int>(TwoPairsOutcomeEnum.HighFullHouse, 1);
            }

            if (grid.HighRank == LowPairCard1.Rank || grid.LowRank == LowPairCard2.Rank)
            {
                return new Tuple<TwoPairsOutcomeEnum, int>(TwoPairsOutcomeEnum.LowFullHouse, 1);
            }

            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank > HighPairCard1.Rank)
                {
                    return new Tuple<TwoPairsOutcomeEnum, int>(TwoPairsOutcomeEnum.OverTwoPairs, 2);
                }
                if (grid.HighRank > LowPairCard1.Rank)
                {
                    return new Tuple<TwoPairsOutcomeEnum, int>(TwoPairsOutcomeEnum.HighBetweenTwoPairs, 2);
                }
            }

            if (grid.HighRank == RankEnum.Ace)
            {
                return new Tuple<TwoPairsOutcomeEnum, int>(TwoPairsOutcomeEnum.TopKicker, 0);
            }

            if (grid.HighRank > RankEnum.Ten)
            {
                return new Tuple<TwoPairsOutcomeEnum, int>(TwoPairsOutcomeEnum.GoodKicker, 0);
            }

            return new Tuple<TwoPairsOutcomeEnum, int>(TwoPairsOutcomeEnum.WeakKicker, 0);
        }
    }
}
