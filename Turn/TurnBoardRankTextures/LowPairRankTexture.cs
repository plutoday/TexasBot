using System;
using System.Collections.Generic;
using System.Linq;
using Flop;
using Models;
using Models.Ranging;

namespace Turn.TurnBoardRankTextures
{
    public class LowPairRankTexture : ITurnFlopBoardRankTexture
    {
        public FlopBoard FlopBoard { get; set; }
        public Card HighCard { get; set; }
        public Card MiddleCard { get; set; }
        public Card PairCard1 { get; set; }
        public Card PairCard2 { get; set; }

        public LowPairRankTexture(TurnBoard board)
        {
            FlopBoard = board.FlopBoard;

            var cards = new List<Card>() { FlopBoard.Flop1, FlopBoard.Flop2, FlopBoard.Flop3, board.TurnCard };
            cards.Sort();
            if (cards[0].Rank != cards[1].Rank)
            {
                throw new InvalidOperationException();
            }
            if (cards[1].Rank == cards[2].Rank || cards[2].Rank == cards[3].Rank)
            {
                throw new InvalidOperationException();
            }

            PairCard1 = cards[0];
            PairCard2 = cards[1];
            MiddleCard = cards[2];
            HighCard = cards[3];
        }

        public BoardRangeGridStatusEnum TestGridAgainstFlopBoard(RangeGrid grid)
        {
            throw new NotImplementedException();
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            return TestGridAgainstBoard(grid).Item1.Grade() <= RankHandGradeEnum.OnePair;
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
                case FlopBoardRankTextureEnum.Singles:
                    var singlesTuple = new Flop.FlopBoardRankTextures.SinglesRankTexture(FlopBoard).TestGridAgainstBoard(grid);
                    flopGrade = singlesTuple.Item1.Grade();
                    flopInvolved = singlesTuple.Item2;
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

        public Tuple<LowPairOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {/*
        Foursome,
        HighFullHouse,
        MiddleFullHouse,
        LowFullHouseHighPair,
        LowFullHouseMiddlePair,
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,*/
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == PairCard1.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.Foursome, 2);
                if (grid.HighRank == HighCard.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.HighFullHouse, 2);
                if (grid.HighRank == MiddleCard.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.MiddleFullHouse, 2);
            }

            if (grid.HighRank == HighCard.Rank && grid.LowRank == PairCard1.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.LowFullHouseHighPair, 2);
            if (grid.HighRank == MiddleCard.Rank && grid.LowRank == PairCard1.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.LowFullHouseMiddlePair, 2);

            var ranks = new List<RankEnum>() { HighCard.Rank, MiddleCard.Rank, PairCard1.Rank };
            if (ranks.All(r => r != grid.HighRank)) ranks.Add(grid.HighRank);
            if (ranks.All(r => r != grid.LowRank)) ranks.Add(grid.LowRank);

            if (ranks.Count == 5)
            {
                ranks.Sort();
                if (ranks[4] == ranks[0] + 4)
                {
                    int over = ranks[4] - HighCard.Rank;
                    return over == 2
                        ? new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.StraightOverTwo, 2)
                        : over == 1
                            ? new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.StraightOverOne, 2)
                            : new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.StraightOverNone, 2);
                }
            }

            /*
        ThreesomeTopKicker,
        ThreesomeOverGoodKicker,
        ThreesomeOverWeakKicker,
        ThreesomeNoneKicker,*/

            RankEnum triedRank = RankEnum.Undefined;
            RankEnum kicker = RankEnum.Undefined;
            if (grid.HighRank == PairCard1.Rank)
            {
                triedRank = grid.HighRank;
                kicker = grid.LowRank;
            }
            else if (grid.LowRank == PairCard1.Rank)
            {
                triedRank = grid.LowRank;
                kicker = grid.HighRank;
            }

            if (triedRank != RankEnum.Undefined && kicker != RankEnum.Undefined)
            {
                if (kicker == RankEnum.Ace)
                    return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.ThreesomeTopKicker, 1);
                if (kicker > HighCard.Rank)
                    return kicker > RankEnum.Ten ? new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.ThreesomeOverGoodKicker, 1)
                        : new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.ThreesomeOverWeakKicker, 1);
                return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.ThreesomeNoneKicker, 1);
            }

            /*             
        OverTwoPairs,
        HighTwoPairs,
        AboveMiddleTwoPairs,
        MiddleTwoPairs,
        AboveLowTwoPairs,
        UnderTwoPairs,
        TopKicker,
        OverGoodKicker,
        OverWeakKicker,
        NoneKicker,*/

            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank > HighCard.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.OverTwoPairs, 2);
                if (grid.HighRank > MiddleCard.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.AboveMiddleTwoPairs, 2);
                if (grid.HighRank > PairCard1.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.AboveLowTwoPairs, 2);
                return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.UnderTwoPairs, 2);
            }

            if (grid.HighRank == HighCard.Rank || grid.LowRank == HighCard.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.HighTwoPairs, 1);
            if (grid.HighRank == MiddleCard.Rank || grid.LowRank == MiddleCard.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.MiddleTwoPairs, 2);

            if (grid.HighRank == RankEnum.Ace) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.TopKicker, 0);
            if (grid.HighRank > HighCard.Rank)
            {
                if (grid.HighRank > RankEnum.Ten) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.OverGoodKicker, 0);
                return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.OverWeakKicker, 0);
            }

            return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.NoneKicker, 0);
        }
    }
}
