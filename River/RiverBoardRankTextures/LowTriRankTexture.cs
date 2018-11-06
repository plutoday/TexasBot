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
    public class LowTriRankTexture
    {
        public Card HighCard { get; set; }
        public Card MiddleCard { get; set; }
        public Card TriCard1 { get; set; }
        public Card TriCard2 { get; set; }
        public Card TriCard3 { get; set; }
        public TurnBoard TurnBoard { get; set; }

        public LowTriRankTexture(RiverBoard board)
        {
            TurnBoard = board.TurnBoard;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.LowTri:
                    var lowTriTexture = new Turn.TurnBoardRankTextures.LowTriRankTexture(TurnBoard);
                    if (board.River.Rank == lowTriTexture.SingleCard.Rank ||
                        board.River.Rank < lowTriTexture.TriCard1.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighCard = board.River.Rank > lowTriTexture.SingleCard.Rank
                        ? board.River
                        : lowTriTexture.SingleCard;
                    MiddleCard = board.River.Rank < lowTriTexture.SingleCard.Rank
                        ? board.River
                        : lowTriTexture.SingleCard;
                    TriCard1 = lowTriTexture.TriCard1;
                    TriCard2 = lowTriTexture.TriCard2;
                    TriCard3 = lowTriTexture.TriCard3;
                    break;
                case TurnBoardRankTextureEnum.LowPair:
                    var lowPairTexture = new Turn.TurnBoardRankTextures.LowPairRankTexture(TurnBoard);
                    if (board.River.Rank != lowPairTexture.PairCard1.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighCard = lowPairTexture.HighCard;
                    MiddleCard = lowPairTexture.MiddleCard;
                    TriCard1 = lowPairTexture.PairCard1;
                    TriCard2 = lowPairTexture.PairCard2;
                    TriCard3 = board.River;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            return outcome == LowTriOutcomeEnum.GoodKicker
                || outcome == LowTriOutcomeEnum.WeakKicker
                || outcome == LowTriOutcomeEnum.NoneKicker;
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

        private Tuple<LowTriOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            /*
             
        Foursome,
        HighFullHouse,
        MiddleFullHouse,
        TopOverPairFullHouse,
        GoodOverPairFullHouse,
        WeakOverPairFullHouse,
        HighPairFullHouse,
        MiddlePairFullHouse,
        Straight,
        TopKicker,
        GoodKicker,
        WeakKicker,
        NoneKicker,*/

            if (grid.HighRank == TriCard1.Rank || grid.LowRank == TriCard1.Rank)
            {
                return new Tuple<LowTriOutcomeEnum, int>(LowTriOutcomeEnum.Foursome, 1);
            }
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == HighCard.Rank) return new Tuple<LowTriOutcomeEnum, int>(LowTriOutcomeEnum.HighFullHouse, 2);
                if (grid.HighRank == MiddleCard.Rank) return new Tuple<LowTriOutcomeEnum, int>(LowTriOutcomeEnum.MiddleFullHouse, 2);
                if (grid.HighRank > HighCard.Rank)
                {
                    if (grid.HighRank == RankEnum.Ace) return new Tuple<LowTriOutcomeEnum, int>(LowTriOutcomeEnum.TopOverPairFullHouse, 2);
                    if (grid.HighRank > RankEnum.Ten) return new Tuple<LowTriOutcomeEnum, int>(LowTriOutcomeEnum.GoodOverPairFullHouse, 2);
                    return new Tuple<LowTriOutcomeEnum, int>(LowTriOutcomeEnum.WeakOverPairFullHouse, 2);
                }
                if (grid.HighRank > MiddleCard.Rank) return new Tuple<LowTriOutcomeEnum, int>(LowTriOutcomeEnum.OverMiddlePairFullHouse, 2);
                return new Tuple<LowTriOutcomeEnum, int>(LowTriOutcomeEnum.OverNonePairFullHouse, 2);
            }

            if (grid.HighRank == HighCard.Rank || grid.LowRank == HighCard.Rank)
            {
                return new Tuple<LowTriOutcomeEnum, int>(LowTriOutcomeEnum.HighPairFullHouse, 1);
            }

            if (grid.HighRank == MiddleCard.Rank || grid.LowRank == MiddleCard.Rank)
            {
                return new Tuple<LowTriOutcomeEnum, int>(LowTriOutcomeEnum.MiddlePairFullHouse, 1);
            }
            var boardRanks = new List<RankEnum>() { TriCard1.Rank, HighCard.Rank, MiddleCard.Rank };
            var playerRanks = grid.Category == GridCategoryEnum.Paired ? new List<RankEnum>() { grid.HighRank } :
                new List<RankEnum>() { grid.HighRank, grid.LowRank };

            var straightTuple = boardRanks.FindStraight(playerRanks);
            if (straightTuple != null)
            {
                if (straightTuple.Item1 == 2) return new Tuple<LowTriOutcomeEnum, int>(LowTriOutcomeEnum.StraightOverTwo, straightTuple.Item2);
                if (straightTuple.Item1 == 1) return new Tuple<LowTriOutcomeEnum, int>(LowTriOutcomeEnum.StraightOverOne, straightTuple.Item2);
                return new Tuple<LowTriOutcomeEnum, int>(LowTriOutcomeEnum.StraightOverNone, straightTuple.Item2);
            }

            if (grid.HighRank > HighCard.Rank)
            {
                if (grid.HighRank == RankEnum.Ace) return new Tuple<LowTriOutcomeEnum, int>(LowTriOutcomeEnum.TopKicker, 0);
                if (grid.HighRank > RankEnum.Ten) return new Tuple<LowTriOutcomeEnum, int>(LowTriOutcomeEnum.GoodKicker, 0);
                return new Tuple<LowTriOutcomeEnum, int>(LowTriOutcomeEnum.WeakKicker, 0);
            }
            return new Tuple<LowTriOutcomeEnum, int>(LowTriOutcomeEnum.NoneKicker, 0);
        }
    }
}
