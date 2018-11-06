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
    public class LowTriHighPairRankTexture : IRiverBoardRankTexture
    {
        public Card TriCard1 { get; set; }
        public Card TriCard2 { get; set; }
        public Card TriCard3 { get; set; }
        public Card PairCard1 { get; set; }
        public Card PairCard2 { get; set; }

        public TurnBoard TurnBoard { get; set; }

        public LowTriHighPairRankTexture(RiverBoard board)
        {
            TurnBoard = board.TurnBoard;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.LowTri:
                    var lowTriTexture = new Turn.TurnBoardRankTextures.LowTriRankTexture(TurnBoard);
                    TriCard1 = lowTriTexture.TriCard1;
                    TriCard2 = lowTriTexture.TriCard2;
                    TriCard3 = lowTriTexture.TriCard3;
                    if (lowTriTexture.SingleCard.Rank != board.River.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    PairCard1 = lowTriTexture.SingleCard;
                    PairCard2 = board.River;
                    break;
                case TurnBoardRankTextureEnum.TwoPairs:
                    var twoPairsTexture = new TwoPairsRankTexture(TurnBoard);
                    if (twoPairsTexture.LowPairCard1.Rank != board.River.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    TriCard1 = twoPairsTexture.LowPairCard1;
                    TriCard2 = twoPairsTexture.LowPairCard2;
                    TriCard3 = board.River;
                    PairCard1 = twoPairsTexture.HighPairCard1;
                    PairCard2 = twoPairsTexture.HighPairCard2;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            return outcome == LowTriHighPairOutcomeEnum.FullHouse;
        }

        public GridHitNewRoundResultEnum HitRiver(RangeGrid grid)
        {
            RankHandGradeEnum turnGrade;
            int turnInvolved;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.LowTri:
                    var lowTriTuple = new Turn.TurnBoardRankTextures.LowTriRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = lowTriTuple.Item1.Grade();
                    turnInvolved = lowTriTuple.Item2;
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

        private Tuple<LowTriHighPairOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            /*
        HighFoursome,
        LowFoursome,
        HighFullHouse,
        TopPairFullHouse,
        GoodOverPairFullHouse,
        WeakOverPairFullHouse,
        MiddleFullHouse,*/
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == PairCard1.Rank) return new Tuple<LowTriHighPairOutcomeEnum, int>(LowTriHighPairOutcomeEnum.HighFoursome, 2);
                if (grid.HighRank > PairCard1.Rank)
                {
                    if (grid.HighRank == RankEnum.Ace) return new Tuple<LowTriHighPairOutcomeEnum, int>(LowTriHighPairOutcomeEnum.TopPairFullHouse, 2);
                    if (grid.HighRank > RankEnum.Ten) return new Tuple<LowTriHighPairOutcomeEnum, int>(LowTriHighPairOutcomeEnum.GoodOverPairFullHouse, 2);
                    return new Tuple<LowTriHighPairOutcomeEnum, int>(LowTriHighPairOutcomeEnum.WeakOverPairFullHouse, 2);
                }
            }
            if (grid.HighRank == TriCard1.Rank || grid.LowRank == TriCard1.Rank)
            {
                return new Tuple<LowTriHighPairOutcomeEnum, int>(LowTriHighPairOutcomeEnum.LowFoursome, 1);
            }
            return new Tuple<LowTriHighPairOutcomeEnum, int>(LowTriHighPairOutcomeEnum.FullHouse, 0);
        }
    }
}
