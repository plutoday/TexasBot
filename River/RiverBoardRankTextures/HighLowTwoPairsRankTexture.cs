using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Ranging;
using Turn;
using Turn.TurnBoardRankTextures;

namespace River.RiverBoardRankTextures
{
    public class HighLowTwoPairsRankTexture
    {
        public Card HighPairCard1 { get; set; }
        public Card HighPairCard2 { get; set; }
        public Card LowPairCard1 { get; set; }
        public Card LowPairCard2 { get; set; }
        public Card MiddleCard { get; set; }

        public TurnBoard TurnBoard { get; set; }

        public HighLowTwoPairsRankTexture(RiverBoard board)
        {
            TurnBoard = board.TurnBoard;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.HighPair:
                    var highPairTexture = new Turn.TurnBoardRankTextures.HighPairRankTexture(TurnBoard);
                    if (board.River.Rank != highPairTexture.LowCard.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighPairCard1 = highPairTexture.PairCard1;
                    HighPairCard2 = highPairTexture.PairCard2;
                    MiddleCard = highPairTexture.MiddleCard;
                    LowPairCard1 = highPairTexture.LowCard;
                    LowPairCard2 = board.River;
                    break;
                case TurnBoardRankTextureEnum.LowPair:
                    var lowPairTexture = new Turn.TurnBoardRankTextures.LowPairRankTexture(TurnBoard);
                    if (board.River.Rank != lowPairTexture.HighCard.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighPairCard1 = lowPairTexture.HighCard;
                    HighPairCard2 = board.River;
                    MiddleCard = lowPairTexture.MiddleCard;
                    LowPairCard1 = lowPairTexture.PairCard1;
                    LowPairCard2 = lowPairTexture.PairCard2;
                    break;
                case TurnBoardRankTextureEnum.TwoPairs:
                    var twoPairsTexture = new TwoPairsRankTexture(TurnBoard);
                    if (board.River.Rank >= twoPairsTexture.HighPairCard1.Rank ||
                        board.River.Rank <= twoPairsTexture.LowPairCard1.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighPairCard1 = twoPairsTexture.HighPairCard1;
                    HighPairCard2 = twoPairsTexture.HighPairCard2;
                    MiddleCard = board.River;
                    LowPairCard1 = twoPairsTexture.LowPairCard1;
                    LowPairCard2 = twoPairsTexture.LowPairCard2;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
        
        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            return outcome == HighLowTwoPairsOutcomeEnum.GoodKicker
                || outcome == HighLowTwoPairsOutcomeEnum.WeakKicker
                || outcome == HighLowTwoPairsOutcomeEnum.NoneKicker;
        }

        public GridHitNewRoundResultEnum HitRiver(RangeGrid grid)
        {
            RankHandGradeEnum turnGrade;
            int turnInvolved;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.TwoPairs:
                    var twoPairsTuple = new Turn.TurnBoardRankTextures.TwoPairsRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = twoPairsTuple.Item1.Grade();
                    turnInvolved = twoPairsTuple.Item2;
                    break;
                case TurnBoardRankTextureEnum.HighPair:
                    var highPairTuple = new Turn.TurnBoardRankTextures.HighPairRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = highPairTuple.Item1.Grade();
                    turnInvolved = highPairTuple.Item2;
                    break;
                case TurnBoardRankTextureEnum.LowPair:
                    var lowPairTuple = new Turn.TurnBoardRankTextures.LowPairRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = lowPairTuple.Item1.Grade();
                    turnInvolved = lowPairTuple.Item2;
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

        private Tuple<HighLowTwoPairsOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            /*
        HighFoursome,
        LowFoursome,
        HighMiddleFullHouse,
        HighFullHouse,
        MiddleFullHouse,
        LowFullHouse,
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        OverTopTwoPairs,
        OverGoodTwoPairs,
        OverWeakTwoPairs,
        HighTwoPairs,
        TopKicker,
        GoodKicker,
        WeakKicker,
        NoneKicker,*/

            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == HighPairCard1.Rank) return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.HighFoursome, 2);
                if (grid.HighRank == LowPairCard1.Rank) return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.LowFoursome, 2);
                if (grid.HighRank == MiddleCard.Rank) return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.MiddleFullHouse, 2);
                if (grid.HighRank > HighPairCard1.Rank)
                {
                    //over two pairs
                    if (grid.HighRank == RankEnum.Ace) return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.OverTopTwoPairs, 2);
                    if (grid.HighRank > RankEnum.Ten) return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.OverGoodTwoPairs, 2);
                    return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.OverWeakTwoPairs, 2);
                }
                if (grid.HighRank > LowPairCard1.Rank)
                {
                    return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.OverLowTwoPairs, 2);
                }
            }

            if (grid.HighRank == HighPairCard1.Rank && grid.LowRank == MiddleCard.Rank) return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.MiddleFullHouse, 2);
            if (grid.HighRank == HighPairCard1.Rank) return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.HighFullHouse, 1);
            if (grid.HighRank == LowPairCard1.Rank || grid.LowRank == LowPairCard1.Rank) return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.LowFullHouse, 2);

            var boardRanks = new List<RankEnum>() { HighPairCard1.Rank, LowPairCard1.Rank, MiddleCard.Rank };
            var playerRanks = grid.Category == GridCategoryEnum.Paired ? new List<RankEnum>() { grid.HighRank } :
                new List<RankEnum>() { grid.HighRank, grid.LowRank };

            var straightTuple = boardRanks.FindStraight(playerRanks);
            if (straightTuple != null)
            {
                if (straightTuple.Item1 == 2) return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.StraightOverTwo, straightTuple.Item2);
                if (straightTuple.Item1 == 1) return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.StraightOverOne, straightTuple.Item2);
                return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.StraightOverNone, straightTuple.Item2);
            }
            if (grid.HighRank == MiddleCard.Rank || grid.LowRank == MiddleCard.Rank)
            {
                var kicker = grid.HighRank == MiddleCard.Rank ? grid.LowRank : grid.HighRank;
                if (kicker > MiddleCard.Rank)
                {
                    if (kicker == RankEnum.Ace) return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.HighTwoPairsTopKicker, 1);
                    if (kicker > RankEnum.Ten) return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.HighTwoPairsGoodKicker, 1);
                    return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.HighTwoPairsWeakKicker, 1);
                }
                return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.HighTwoPairsNoneKicker, 1);
            }

            if (grid.HighRank > MiddleCard.Rank)
            {
                if (grid.HighRank == RankEnum.Ace) return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.TopKicker, 0);
                if (grid.HighRank > RankEnum.Ten) return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.GoodKicker, 0);
                return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.WeakKicker, 0);
            }
            return new Tuple<HighLowTwoPairsOutcomeEnum, int>(HighLowTwoPairsOutcomeEnum.NoneKicker, 0);
        }
    }
}
