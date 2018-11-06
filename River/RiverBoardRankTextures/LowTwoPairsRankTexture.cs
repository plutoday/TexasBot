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
    public class LowTwoPairsRankTexture : IRiverBoardRankTexture
    {
        public Card HighCard { get; set; }
        public Card MiddlePairCard1 { get; set; }
        public Card MiddlePairCard2 { get; set; }
        public Card LowPairCard1 { get; set; }
        public Card LowPairCard2 { get; set; }
        public TurnBoard TurnBoard { get; set; }

        public LowTwoPairsRankTexture(RiverBoard board)
        {
            TurnBoard = board.TurnBoard;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.MiddlePair:
                    var middlePairTexture = new MiddlePairRankTexture(TurnBoard);
                    if (board.River.Rank != middlePairTexture.LowCard.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighCard = middlePairTexture.HighCard;
                    MiddlePairCard1 = middlePairTexture.PairCard1;
                    MiddlePairCard2 = middlePairTexture.PairCard2;
                    LowPairCard1 = middlePairTexture.LowCard;
                    LowPairCard2 = board.River;
                    break;
                case TurnBoardRankTextureEnum.LowPair:
                    var lowPairTexture = new Turn.TurnBoardRankTextures.LowPairRankTexture(TurnBoard);
                    if (board.River.Rank != lowPairTexture.MiddleCard.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighCard = lowPairTexture.HighCard;
                    MiddlePairCard1 = lowPairTexture.MiddleCard;
                    MiddlePairCard2 = board.River;
                    LowPairCard1 = lowPairTexture.PairCard1;
                    LowPairCard2 = lowPairTexture.PairCard2;
                    break;
                case TurnBoardRankTextureEnum.TwoPairs:
                    var twoPairsTexture = new TwoPairsRankTexture(TurnBoard);
                    if (board.River.Rank <= twoPairsTexture.HighPairCard1.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighCard = board.River;
                    MiddlePairCard1 = twoPairsTexture.HighPairCard1;
                    MiddlePairCard2 = twoPairsTexture.HighPairCard2;
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
            return outcome == LowTwoPairsOutcomeEnum.GoodKicker
                || outcome == LowTwoPairsOutcomeEnum.WeakKicker
                || outcome == LowTwoPairsOutcomeEnum.NoneKicker;
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
                case TurnBoardRankTextureEnum.MiddlePair:
                    var middlePairTuple = new Turn.TurnBoardRankTextures.MiddlePairRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = middlePairTuple.Item1.Grade();
                    turnInvolved = middlePairTuple.Item2;
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

        private Tuple<LowTwoPairsOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            /*
        MiddleFoursome,
        LowFoursome,
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
                if (grid.HighRank == MiddlePairCard1.Rank) return new Tuple<LowTwoPairsOutcomeEnum, int>(LowTwoPairsOutcomeEnum.MiddleFoursome, 2);
                if (grid.HighRank == LowPairCard1.Rank) return new Tuple<LowTwoPairsOutcomeEnum, int>(LowTwoPairsOutcomeEnum.LowFoursome, 2);
                if (grid.HighRank == HighCard.Rank) return new Tuple<LowTwoPairsOutcomeEnum, int>(LowTwoPairsOutcomeEnum.HighFullHouse, 2);
                if (grid.HighRank > MiddlePairCard1.Rank)
                {
                    if (grid.HighRank == RankEnum.Ace) return new Tuple<LowTwoPairsOutcomeEnum, int>(LowTwoPairsOutcomeEnum.OverTopTwoPairs, 2);
                    if (grid.HighRank > RankEnum.Ten) return new Tuple<LowTwoPairsOutcomeEnum, int>(LowTwoPairsOutcomeEnum.OverGoodTwoPairs, 2);
                    return new Tuple<LowTwoPairsOutcomeEnum, int>(LowTwoPairsOutcomeEnum.OverWeakTwoPairs, 2);
                }
            }

            if (grid.HighRank == MiddlePairCard1.Rank || grid.LowRank == MiddlePairCard1.Rank) return new Tuple<LowTwoPairsOutcomeEnum, int>(LowTwoPairsOutcomeEnum.MiddleFullHouse, 1);
            if (grid.HighRank == LowPairCard1.Rank || grid.LowRank == LowPairCard1.Rank) return new Tuple<LowTwoPairsOutcomeEnum, int>(LowTwoPairsOutcomeEnum.LowFullHouse, 1);

            var boardRanks = new List<RankEnum>() { HighCard.Rank, MiddlePairCard1.Rank, LowPairCard1.Rank };
            var playerRanks = grid.Category == GridCategoryEnum.Paired ? new List<RankEnum>() { grid.HighRank } :
                new List<RankEnum>() { grid.HighRank, grid.LowRank };

            var straightTuple = boardRanks.FindStraight(playerRanks);
            if (straightTuple != null)
            {
                if (straightTuple.Item1 == 2) return new Tuple<LowTwoPairsOutcomeEnum, int>(LowTwoPairsOutcomeEnum.StraightOverTwo, straightTuple.Item2);
                if (straightTuple.Item1 == 1) return new Tuple<LowTwoPairsOutcomeEnum, int>(LowTwoPairsOutcomeEnum.StraightOverOne, straightTuple.Item2);
                return new Tuple<LowTwoPairsOutcomeEnum, int>(LowTwoPairsOutcomeEnum.StraightOverNone, straightTuple.Item2);
            }

            if (grid.HighRank == HighCard.Rank || grid.LowRank == HighCard.Rank) return new Tuple<LowTwoPairsOutcomeEnum, int>(LowTwoPairsOutcomeEnum.HighTwoPairs, 1);
            if (grid.HighRank > HighCard.Rank)
            {
                if (grid.HighRank == RankEnum.Ace) return new Tuple<LowTwoPairsOutcomeEnum, int>(LowTwoPairsOutcomeEnum.TopKicker, 0);
                if (grid.HighRank > RankEnum.Ten) return new Tuple<LowTwoPairsOutcomeEnum, int>(LowTwoPairsOutcomeEnum.GoodKicker, 0);
                return new Tuple<LowTwoPairsOutcomeEnum, int>(LowTwoPairsOutcomeEnum.WeakKicker, 0);
            }
            return new Tuple<LowTwoPairsOutcomeEnum, int>(LowTwoPairsOutcomeEnum.NoneKicker, 0);
        }
    }
}
