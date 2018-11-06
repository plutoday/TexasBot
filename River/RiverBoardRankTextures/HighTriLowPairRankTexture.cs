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
    public class HighTriLowPairRankTexture : IRiverBoardRankTexture
    {
        public Card TriCard1 { get; set; }
        public Card TriCard2 { get; set; }
        public Card TriCard3 { get; set; }
        public Card PairCard1 { get; set; }
        public Card PairCard2 { get; set; }

        public TurnBoard TurnBoard { get; set; }

        public HighTriLowPairRankTexture(RiverBoard board)
        {
            TurnBoard = board.TurnBoard;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.HighTri:
                    var highTriTexture = new Turn.TurnBoardRankTextures.HighTriRankTexture(TurnBoard);
                    TriCard1 = highTriTexture.TriCard1;
                    TriCard2 = highTriTexture.TriCard2;
                    TriCard3 = highTriTexture.TriCard3;
                    if (highTriTexture.SingleCard.Rank != board.River.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    PairCard1 = highTriTexture.SingleCard;
                    PairCard2 = board.River;
                    break;
                case TurnBoardRankTextureEnum.TwoPairs:
                    var twoPairsTexture = new TwoPairsRankTexture(TurnBoard);
                    if (twoPairsTexture.HighPairCard1.Rank != board.River.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    TriCard1 = twoPairsTexture.HighPairCard1;
                    TriCard2 = twoPairsTexture.HighPairCard2;
                    TriCard3 = board.River;
                    PairCard1 = twoPairsTexture.LowPairCard1;
                    PairCard2 = twoPairsTexture.LowPairCard2;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            return outcome == HighTriLowPairOutcomeEnum.FullHouse;
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

        private Tuple<HighTriLowPairOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            /*
        HighFoursome,
        LowFoursome,
        TopPairFullHouse,
        GoodOverPairFullHouse,
        WeakOverPairFullHouse,
        MiddleFullHouse,*/
            if (grid.HighRank == TriCard1.Rank || grid.LowRank == TriCard1.Rank)
            {
                return new Tuple<HighTriLowPairOutcomeEnum, int>(HighTriLowPairOutcomeEnum.HighFoursome, 1);
            }
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == PairCard1.Rank) return new Tuple<HighTriLowPairOutcomeEnum, int>(HighTriLowPairOutcomeEnum.LowFoursome, 2);
                if (grid.HighRank > PairCard1.Rank)
                {
                    if (grid.HighRank == RankEnum.Ace) return new Tuple<HighTriLowPairOutcomeEnum, int>(HighTriLowPairOutcomeEnum.TopPairFullHouse, 2);
                    if (grid.HighRank > RankEnum.Ten) return new Tuple<HighTriLowPairOutcomeEnum, int>(HighTriLowPairOutcomeEnum.GoodOverPairFullHouse, 2);
                    return new Tuple<HighTriLowPairOutcomeEnum, int>(HighTriLowPairOutcomeEnum.WeakOverPairFullHouse, 2);
                }
            }
            return new Tuple<HighTriLowPairOutcomeEnum, int>(HighTriLowPairOutcomeEnum.FullHouse, 0);
        }
    }
}

