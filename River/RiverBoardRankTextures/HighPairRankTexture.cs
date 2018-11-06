using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Ranging;
using Turn;

namespace River.RiverBoardRankTextures
{
    public class HighPairRankTexture : IRiverBoardRankTexture
    {
        public Card PairedCard1 { get; set; }
        public Card PairedCard2 { get; set; }
        public Card SecondCard { get; set; }
        public Card ThirdCard { get; set; }
        public Card LowCard { get; set; }

        public TurnBoard TurnBoard { get; set; }

        public HighPairRankTexture(RiverBoard board)
        {
            TurnBoard = board.TurnBoard;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.HighPair:
                    var highPairTexture = new Turn.TurnBoardRankTextures.HighPairRankTexture(TurnBoard);
                    if (board.River.Rank >= highPairTexture.PairCard1.Rank)
                    {
                        throw new InvalidOperationException();
                    }

                    if (board.River.Rank == highPairTexture.MiddleCard.Rank ||
                        board.River.Rank == highPairTexture.LowCard.Rank)
                    {
                        throw new InvalidOperationException();
                    }

                    PairedCard1 = highPairTexture.PairCard1;
                    PairedCard2 = highPairTexture.PairCard2;

                    var singles = new List<Card>() { highPairTexture.MiddleCard, highPairTexture.LowCard, board.River };
                    singles.Sort();
                    LowCard = singles[0];
                    ThirdCard = singles[1];
                    SecondCard = singles[2];
                    break;

                case TurnBoardRankTextureEnum.Singles:
                    var singlesPairTexture = new Turn.TurnBoardRankTextures.SinglesRankTexture(TurnBoard);
                    if (board.River.Rank != singlesPairTexture.HighCard.Rank)
                    {
                        throw new InvalidOperationException();
                    }

                    PairedCard1 = singlesPairTexture.HighCard;
                    PairedCard2 = board.River;
                    SecondCard = singlesPairTexture.SecondCard;
                    ThirdCard = singlesPairTexture.ThirdCard;
                    LowCard = singlesPairTexture.LowCard;
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            return outcome == HighPairOutcomeEnum.GoodKicker
                || outcome == HighPairOutcomeEnum.WeakKicker
                || outcome == HighPairOutcomeEnum.NoneKicker;
        }

        public GridHitNewRoundResultEnum HitRiver(RangeGrid grid)
        {
            RankHandGradeEnum turnGrade;
            int turnInvolved;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.HighPair:
                    var highPairTuple = new Turn.TurnBoardRankTextures.HighPairRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = highPairTuple.Item1.Grade();
                    turnInvolved = highPairTuple.Item2;
                    break;
                case TurnBoardRankTextureEnum.Singles:
                    var singlesTuple = new Turn.TurnBoardRankTextures.SinglesRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = singlesTuple.Item1.Grade();
                    turnInvolved = singlesTuple.Item2;
                    break;
                default:
                    throw new InvalidOperationException($"Impossible to have {TurnBoard.RankTexture}");
            }
            var riverTuple = TestGridAgainstBoard(grid);
            var riverOutcomeGrade = riverTuple.Item1.Grade();
            var riverInvolved = riverTuple.Item2;

            if (riverOutcomeGrade > turnGrade)
            {
                return (riverInvolved > turnInvolved) ? GridHitNewRoundResultEnum.Promoted : GridHitNewRoundResultEnum.Enhanced;
            }
            return GridHitNewRoundResultEnum.None;
        }

        private Tuple<HighPairOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == PairedCard1.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.Foursome, 2);
                if (grid.HighRank == SecondCard.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.SecondHighFullHouse, 2);
                if (grid.HighRank == ThirdCard.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.ThirdHighFullHouse, 2);
                if (grid.HighRank == LowCard.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.LowHighFullHouse, 2);
            }
            if (grid.HighRank == PairedCard1.Rank && grid.LowRank == SecondCard.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.HighSecondFullHouse, 2);
            if (grid.HighRank == PairedCard1.Rank && grid.LowRank == ThirdCard.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.HighThirdFullHouse, 2);
            if (grid.HighRank == PairedCard1.Rank && grid.LowRank == LowCard.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.HighLowFullHouse, 2);

            var boardRanks = new List<RankEnum>() {PairedCard1.Rank, SecondCard.Rank, ThirdCard.Rank, LowCard.Rank};
            var playerRanks = grid.Category == GridCategoryEnum.Paired ? new List<RankEnum>() {grid.HighRank} :
                new List<RankEnum>() {grid.HighRank, grid.LowRank};

            var straightTuple = boardRanks.FindStraight(playerRanks);
            if (straightTuple != null)
            {
                if (straightTuple.Item1 == 2) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.StraightOverTwo, straightTuple.Item2);
                if (straightTuple.Item1 == 1) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.StraightOverOne, straightTuple.Item2);
                return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.StraightOverNone, straightTuple.Item2);
            }

            RankEnum tri =  RankEnum.Undefined;
            RankEnum kicker = RankEnum.Undefined;
            if (grid.HighRank == PairedCard1.Rank)
            {
                tri = grid.HighRank;
                kicker = grid.LowRank;
            }
            else if (grid.LowRank == PairedCard1.Rank)
            {
                tri = grid.LowRank;
                kicker = grid.HighRank;
            }
            if (tri != RankEnum.Undefined && kicker != RankEnum.Undefined)
            {
                if (kicker > SecondCard.Rank)
                {
                    if (kicker == RankEnum.Ace) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.ThreesomeTopKicker, 1);
                    if (kicker > RankEnum.Ten) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.ThreesomeGoodKicker, 1);
                    return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.ThreesomeWeakKicker, 1);
                }
                return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.ThreesomeNoneKicker, 1);
            }

            if (grid.HighRank == SecondCard.Rank)
            {
                tri = grid.HighRank;
                kicker = grid.LowRank;
            }
            else if (grid.LowRank == SecondCard.Rank)
            {
                tri = grid.LowRank;
                kicker = grid.HighRank;
            }
            if (tri != RankEnum.Undefined && kicker != RankEnum.Undefined)
            {
                if (kicker > ThirdCard.Rank)
                {
                    if (kicker == RankEnum.Ace) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.SecondTwoPairsTopKicker, 1);
                    if (kicker > RankEnum.Ten) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.SecondTwoPairsGoodKicker, 1);
                    return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.SecondTwoPairsWeakKicker, 1);
                }
                return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.SecondTwoPairsNoneKicker, 1);
            }

            if (grid.HighRank == ThirdCard.Rank || grid.LowRank == ThirdCard.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.ThirdTwoPairs, 1);
            if (grid.HighRank == LowCard.Rank || grid.LowRank == LowCard.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.LowTwoPairs, 1);
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank > PairedCard1.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.OverTwoPairs, 2);
                if (grid.HighRank < PairedCard1.Rank && grid.HighRank > SecondCard.Rank)
                    return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.OverSecondTwoPairs, 2);
                if (grid.HighRank < SecondCard.Rank && grid.HighRank > ThirdCard.Rank)
                    return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.OverThirdTwoPairs, 2);
                if (grid.HighRank < ThirdCard.Rank && grid.HighRank > LowCard.Rank)
                    return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.OverLowTwoPairs, 2);
                if (grid.HighRank < LowCard.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.UnderTwoPairs, 2);
                throw new InvalidOperationException("Should be full house");
            }

            if (grid.HighRank > SecondCard.Rank)
            {
                if (grid.HighRank == RankEnum.Ace) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.TopKicker, 0);
                if (grid.HighRank > RankEnum.Ten) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.GoodKicker, 0);
                return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.WeakKicker, 0);
            }
            return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.NoneKicker, 0);
        }
    }
}
