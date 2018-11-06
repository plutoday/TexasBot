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
    public class MiddleTriRankTexture : IRiverBoardRankTexture
    {
        public Card HighCard { get; set; }
        public Card TriCard1 { get; set; }
        public Card TriCard2 { get; set; }
        public Card TriCard3 { get; set; }
        public Card LowCard { get; set; }

        public TurnBoard TurnBoard { get; set; }

        public MiddleTriRankTexture(RiverBoard board)
        {
            TurnBoard = board.TurnBoard;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.HighTri:
                    var highTriTexture = new Turn.TurnBoardRankTextures.HighTriRankTexture(TurnBoard);
                    if (board.River.Rank <= highTriTexture.TriCard1.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighCard = board.River;
                    TriCard1 = highTriTexture.TriCard1;
                    TriCard2 = highTriTexture.TriCard2;
                    TriCard3 = highTriTexture.TriCard3;
                    LowCard = highTriTexture.SingleCard;
                    break;
                case TurnBoardRankTextureEnum.LowTri:
                    var lowTriTexture = new Turn.TurnBoardRankTextures.LowTriRankTexture(TurnBoard);
                    if (board.River.Rank >= lowTriTexture.TriCard1.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighCard = lowTriTexture.SingleCard;
                    TriCard1 = lowTriTexture.TriCard1;
                    TriCard2 = lowTriTexture.TriCard2;
                    TriCard3 = lowTriTexture.TriCard3;
                    LowCard = board.River;
                    break;
                case TurnBoardRankTextureEnum.MiddlePair:
                    var middlePairTexture = new MiddlePairRankTexture(TurnBoard);
                    if (board.River.Rank != middlePairTexture.PairCard1.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighCard = middlePairTexture.HighCard;
                    TriCard1 = middlePairTexture.PairCard1;
                    TriCard2 = middlePairTexture.PairCard2;
                    TriCard3 = board.River;
                    LowCard = middlePairTexture.LowCard;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            return outcome == MiddleTriOutcomeEnum.GoodKicker
                || outcome == MiddleTriOutcomeEnum.WeakKicker
                || outcome == MiddleTriOutcomeEnum.NoneKicker;
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
                case TurnBoardRankTextureEnum.LowTri:
                    var lowTriTuple = new Turn.TurnBoardRankTextures.LowTriRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = lowTriTuple.Item1.Grade();
                    turnInvolved = lowTriTuple.Item2;
                    break;
                case TurnBoardRankTextureEnum.MiddlePair:
                    var middlePair = new Turn.TurnBoardRankTextures.MiddlePairRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = middlePair.Item1.Grade();
                    turnInvolved = middlePair.Item2;
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

        public Tuple<MiddleTriOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            if (grid.HighRank == TriCard1.Rank || grid.LowRank == TriCard1.Rank)
            {
                return new Tuple<MiddleTriOutcomeEnum, int>(MiddleTriOutcomeEnum.Foursome, 1);
            }

            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == HighCard.Rank) return new Tuple<MiddleTriOutcomeEnum, int>(MiddleTriOutcomeEnum.HighFullHouse, 2);
                if (grid.HighRank == RankEnum.Ace) return new Tuple<MiddleTriOutcomeEnum, int>(MiddleTriOutcomeEnum.TopPairFullHouse, 2);
                if (grid.HighRank > RankEnum.Ten) return new Tuple<MiddleTriOutcomeEnum, int>(MiddleTriOutcomeEnum.GoodPairFullHouse, 2);
                return new Tuple<MiddleTriOutcomeEnum, int>(MiddleTriOutcomeEnum.WeakPairFullHouse, 2);
            }

            if (grid.HighRank == HighCard.Rank || grid.LowRank == HighCard.Rank) return new Tuple<MiddleTriOutcomeEnum, int>(MiddleTriOutcomeEnum.HighPairFullHouse, 1);
            if (grid.HighRank == LowCard.Rank || grid.LowRank == LowCard.Rank) return new Tuple<MiddleTriOutcomeEnum, int>(MiddleTriOutcomeEnum.LowPairFullHouse, 1);

            var boardRanks = new List<RankEnum>() { TriCard1.Rank, HighCard.Rank, LowCard.Rank };
            var playerRanks = grid.Category == GridCategoryEnum.Paired ? new List<RankEnum>() { grid.HighRank } :
                new List<RankEnum>() { grid.HighRank, grid.LowRank };

            var straightTuple = boardRanks.FindStraight(playerRanks);
            if (straightTuple != null)
            {
                if (straightTuple.Item1 == 2) return new Tuple<MiddleTriOutcomeEnum, int>(MiddleTriOutcomeEnum.StraightOverTwo, straightTuple.Item2);
                if (straightTuple.Item1 == 1) return new Tuple<MiddleTriOutcomeEnum, int>(MiddleTriOutcomeEnum.StraightOverOne, straightTuple.Item2);
                return new Tuple<MiddleTriOutcomeEnum, int>(MiddleTriOutcomeEnum.StraightOverNone, straightTuple.Item2);
            }

            if (grid.HighRank > HighCard.Rank)
            {
                if (grid.HighRank == RankEnum.Ace) return new Tuple<MiddleTriOutcomeEnum, int>(MiddleTriOutcomeEnum.TopKicker, 0);
                if (grid.HighRank > RankEnum.Ten) return new Tuple<MiddleTriOutcomeEnum, int>(MiddleTriOutcomeEnum.GoodKicker, 0);
                return new Tuple<MiddleTriOutcomeEnum, int>(MiddleTriOutcomeEnum.WeakKicker, 0);
            }
            return new Tuple<MiddleTriOutcomeEnum, int>(MiddleTriOutcomeEnum.NoneKicker, 0);
        }
    }
}
