using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Ranging;
using Turn;
using Turn.TurnBoardRankTextures;

namespace River.RiverBoardRankTextures
{
    public class HighTwoPairsRankTexture : IRiverBoardRankTexture
    {
        public Card HighPairCard1 { get; set; }
        public Card HighPairCard2 { get; set; }
        public Card MiddlePairCard1 { get; set; }
        public Card MiddlePairCard2 { get; set; }
        public Card LowCard { get; set; }
        public TurnBoard TurnBoard { get; set; }

        public HighTwoPairsRankTexture(RiverBoard board)
        {
            TurnBoard = board.TurnBoard;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.TwoPairs:
                    var twoPairTexture = new TwoPairsRankTexture(TurnBoard);
                    if (board.River.Rank >= twoPairTexture.LowPairCard1.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighPairCard1 = twoPairTexture.HighPairCard1;
                    HighPairCard2 = twoPairTexture.HighPairCard2;
                    MiddlePairCard1 = twoPairTexture.LowPairCard1;
                    MiddlePairCard2 = twoPairTexture.LowPairCard2;
                    LowCard = board.River;
                    break;
                case TurnBoardRankTextureEnum.HighPair:
                    var highPairTexture = new Turn.TurnBoardRankTextures.HighPairRankTexture(TurnBoard);
                    if (board.River.Rank != highPairTexture.MiddleCard.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighPairCard1 = highPairTexture.PairCard1;
                    HighPairCard2 = highPairTexture.PairCard2;
                    MiddlePairCard1 = highPairTexture.MiddleCard;
                    MiddlePairCard2 = board.River;
                    LowCard = highPairTexture.LowCard;
                    break;
                case TurnBoardRankTextureEnum.MiddlePair:
                    var middlePairTexture = new MiddlePairRankTexture(TurnBoard);
                    if (board.River.Rank != middlePairTexture.HighCard.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighPairCard1 = board.River;
                    HighPairCard2 = middlePairTexture.HighCard;
                    MiddlePairCard1 = middlePairTexture.PairCard1;
                    MiddlePairCard2 = middlePairTexture.PairCard2;
                    LowCard = middlePairTexture.LowCard;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            return outcome == HighTwoPairsOutcomeEnum.GoodKicker
                || outcome == HighTwoPairsOutcomeEnum.WeakKicker
                || outcome == HighTwoPairsOutcomeEnum.NoneKicker;
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
                case TurnBoardRankTextureEnum.MiddlePair:
                    var middlePairTuple = new Turn.TurnBoardRankTextures.MiddlePairRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = middlePairTuple.Item1.Grade();
                    turnInvolved = middlePairTuple.Item2;
                    break;
                case TurnBoardRankTextureEnum.TwoPairs:
                    var twoPairsTuple = new Turn.TurnBoardRankTextures.TwoPairsRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = twoPairsTuple.Item1.Grade();
                    turnInvolved = twoPairsTuple.Item2;
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

        private Tuple<HighTwoPairsOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == HighPairCard1.Rank) return new Tuple<HighTwoPairsOutcomeEnum, int>(HighTwoPairsOutcomeEnum.HighFoursome, 2);
                if (grid.HighRank == MiddlePairCard1.Rank) return new Tuple<HighTwoPairsOutcomeEnum, int>(HighTwoPairsOutcomeEnum.MiddleFoursome, 2);
                if (grid.HighRank == LowCard.Rank) return new Tuple<HighTwoPairsOutcomeEnum, int>(HighTwoPairsOutcomeEnum.LowFullHouse, 2);
                if (grid.HighRank > HighPairCard1.Rank)
                {
                    //over two pairs
                    if (grid.HighRank == RankEnum.Ace) return new Tuple<HighTwoPairsOutcomeEnum, int>(HighTwoPairsOutcomeEnum.OverTopTwoPairs, 2);
                    if (grid.HighRank > RankEnum.Ten) return new Tuple<HighTwoPairsOutcomeEnum, int>(HighTwoPairsOutcomeEnum.OverGoodTwoPairs, 2);
                    return new Tuple<HighTwoPairsOutcomeEnum, int>(HighTwoPairsOutcomeEnum.OverWeakTwoPairs, 2);
                }
            }

            if (grid.HighRank == HighPairCard1.Rank || grid.LowRank == HighPairCard1.Rank) return new Tuple<HighTwoPairsOutcomeEnum, int>(HighTwoPairsOutcomeEnum.HighFullHouse, 1);
            if (grid.HighRank == MiddlePairCard1.Rank || grid.LowRank == MiddlePairCard1.Rank) return new Tuple<HighTwoPairsOutcomeEnum, int>(HighTwoPairsOutcomeEnum.MiddleFullHouse, 1);

            var boardRanks = new List<RankEnum>() { HighPairCard1.Rank, MiddlePairCard1.Rank, LowCard.Rank };
            var playerRanks = grid.Category == GridCategoryEnum.Paired ? new List<RankEnum>() { grid.HighRank } :
                new List<RankEnum>() { grid.HighRank, grid.LowRank };

            var straightTuple = boardRanks.FindStraight(playerRanks);
            if (straightTuple != null)
            {
                if (straightTuple.Item1 == 2) return new Tuple<HighTwoPairsOutcomeEnum, int>(HighTwoPairsOutcomeEnum.StraightOverTwo, straightTuple.Item2);
                if (straightTuple.Item1 == 1) return new Tuple<HighTwoPairsOutcomeEnum, int>(HighTwoPairsOutcomeEnum.StraightOverOne, straightTuple.Item2);
                return new Tuple<HighTwoPairsOutcomeEnum, int>(HighTwoPairsOutcomeEnum.StraightOverNone, straightTuple.Item2);
            }

            if (grid.HighRank > LowCard.Rank)
            {
                if (grid.HighRank == RankEnum.Ace) return new Tuple<HighTwoPairsOutcomeEnum, int>(HighTwoPairsOutcomeEnum.TopKicker, 0);
                if (grid.HighRank > RankEnum.Ten) return new Tuple<HighTwoPairsOutcomeEnum, int>(HighTwoPairsOutcomeEnum.GoodKicker, 0);
                return new Tuple<HighTwoPairsOutcomeEnum, int>(HighTwoPairsOutcomeEnum.WeakKicker, 0);
            }
            return new Tuple<HighTwoPairsOutcomeEnum, int>(HighTwoPairsOutcomeEnum.NoneKicker, 0);
        }
    }
}
