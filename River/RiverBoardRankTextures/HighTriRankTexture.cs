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
    public class HighTriRankTexture : IRiverBoardRankTexture
    {
        public Card TriCard1 { get; set; }
        public Card TriCard2 { get; set; }
        public Card TriCard3 { get; set; }
        public Card MiddleCard { get; set; }
        public Card LowCard { get; set; }

        public TurnBoard TurnBoard { get; set; }

        public HighTriRankTexture(RiverBoard board)
        {
            TurnBoard = board.TurnBoard;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.HighTri:
                    var highTriTexture = new Turn.TurnBoardRankTextures.HighTriRankTexture(TurnBoard);
                    if (board.River.Rank == highTriTexture.SingleCard.Rank ||
                        board.River.Rank == highTriTexture.TriCard1.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    TriCard1 = highTriTexture.TriCard1;
                    TriCard2 = highTriTexture.TriCard2;
                    TriCard3 = highTriTexture.TriCard3;
                    MiddleCard = highTriTexture.SingleCard.Rank > board.River.Rank
                        ? highTriTexture.SingleCard
                        : board.River;
                    LowCard = highTriTexture.SingleCard.Rank < board.River.Rank
                       ? highTriTexture.SingleCard
                       : board.River;
                    break;
                case TurnBoardRankTextureEnum.HighPair:
                    var highPairTexture = new Turn.TurnBoardRankTextures.HighPairRankTexture(TurnBoard);
                    if (board.River.Rank != highPairTexture.PairCard1.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    TriCard1 = highPairTexture.PairCard1;
                    TriCard2 = highPairTexture.PairCard2;
                    TriCard3 = board.River;
                    MiddleCard = highPairTexture.MiddleCard;
                    LowCard = highPairTexture.LowCard;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            return outcome == HighTriOutcomeEnum.GoodKicker
                || outcome == HighTriOutcomeEnum.WeakKicker
                || outcome == HighTriOutcomeEnum.NoneKicker;
        }

        public GridHitNewRoundResultEnum HitRiver(RangeGrid grid)
        {
            RankHandGradeEnum turnGrade;
            int turnInvolved;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.HighTri:
                    var highTriTuple = new Turn.TurnBoardRankTextures.HighTriRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = highTriTuple.Item1.Grade();
                    turnInvolved = highTriTuple.Item2;
                    break;
                case TurnBoardRankTextureEnum.HighPair:
                    var highPairTuple = new Turn.TurnBoardRankTextures.HighPairRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = highPairTuple.Item1.Grade();
                    turnInvolved = highPairTuple.Item2;
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

        private Tuple<HighTriOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            if (grid.HighRank == TriCard1.Rank || grid.LowRank == TriCard1.Rank)
            {
                return new Tuple<HighTriOutcomeEnum, int>(HighTriOutcomeEnum.Foursome, 1);
            }

            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank > MiddleCard.Rank)
                {
                    if (grid.HighRank == RankEnum.Ace)
                    {
                        return new Tuple<HighTriOutcomeEnum, int>(HighTriOutcomeEnum.TopOverPairFullHouse, 2);
                    }
                    if (grid.HighRank > RankEnum.Ten)
                    {
                        return new Tuple<HighTriOutcomeEnum, int>(HighTriOutcomeEnum.GoodOverPairFullHouse, 2);
                    }
                    return new Tuple<HighTriOutcomeEnum, int>(HighTriOutcomeEnum.WeakOverPairFullHouse, 2);
                }
                return new Tuple<HighTriOutcomeEnum, int>(HighTriOutcomeEnum.UnderPairFullHouse, 2);
            }

            if (grid.HighRank == MiddleCard.Rank || grid.LowRank == MiddleCard.Rank)
            {
                return new Tuple<HighTriOutcomeEnum, int>(HighTriOutcomeEnum.MiddlePairFullHouse, 1);
            }

            if (grid.HighRank == LowCard.Rank || grid.LowRank == LowCard.Rank)
            {
                return new Tuple<HighTriOutcomeEnum, int>(HighTriOutcomeEnum.LowPairFullHouse, 1);
            }

            var boardRanks = new List<RankEnum>() { TriCard1.Rank, MiddleCard.Rank, LowCard.Rank};
            var playerRanks = grid.Category == GridCategoryEnum.Paired ? new List<RankEnum>() { grid.HighRank } :
                new List<RankEnum>() { grid.HighRank, grid.LowRank };

            var straightTuple = boardRanks.FindStraight(playerRanks);
            if (straightTuple != null)
            {
                if (straightTuple.Item1 == 2) return new Tuple<HighTriOutcomeEnum, int>(HighTriOutcomeEnum.StraightOverTwo, straightTuple.Item2);
                if (straightTuple.Item1 == 1) return new Tuple<HighTriOutcomeEnum, int>(HighTriOutcomeEnum.StraightOverOne, straightTuple.Item2);
                return new Tuple<HighTriOutcomeEnum, int>(HighTriOutcomeEnum.StraightOverNone, straightTuple.Item2);
            }

            if (grid.HighRank > MiddleCard.Rank)
            {
                if (grid.HighRank == RankEnum.Ace) return new Tuple<HighTriOutcomeEnum, int>(HighTriOutcomeEnum.TopKicker, 0);
                if (grid.HighRank > RankEnum.Ten) return new Tuple<HighTriOutcomeEnum, int>(HighTriOutcomeEnum.GoodKicker, 0);
                return new Tuple<HighTriOutcomeEnum, int>(HighTriOutcomeEnum.WeakKicker, 0);
            }

            return new Tuple<HighTriOutcomeEnum, int>(HighTriOutcomeEnum.NoneKicker, 0);
        }
    }
}
