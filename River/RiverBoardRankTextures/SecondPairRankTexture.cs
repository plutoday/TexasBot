using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Ranging;
using Turn;
using Turn.TurnBoardRankTextures;

namespace River.RiverBoardRankTextures
{
    public class SecondPairRankTexture : IRiverBoardRankTexture
    {
        public Card HighCard { get; set; }
        public Card PairedCard1 { get; set; }
        public Card PairedCard2 { get; set; }
        public Card ThirdCard { get; set; }
        public Card LowCard { get; set; }

        public TurnBoard TurnBoard { get; set; }

        public SecondPairRankTexture(RiverBoard board)
        {
            TurnBoard = board.TurnBoard;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.HighPair:
                    var highPairTexture = new Turn.TurnBoardRankTextures.HighPairRankTexture(TurnBoard);
                    if (board.River.Rank <= highPairTexture.PairCard1.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    if (board.River.Rank == highPairTexture.MiddleCard.Rank
                        || board.River.Rank == highPairTexture.LowCard.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighCard = board.River;
                    PairedCard1 = highPairTexture.PairCard1;
                    PairedCard2 = highPairTexture.PairCard2;
                    ThirdCard = highPairTexture.MiddleCard;
                    LowCard = highPairTexture.LowCard;
                    break;
                case TurnBoardRankTextureEnum.MiddlePair:
                    var middlePairTexture = new MiddlePairRankTexture(TurnBoard);
                    if (board.River.Rank >= middlePairTexture.PairCard1.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    if (board.River.Rank == middlePairTexture.LowCard.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighCard = middlePairTexture.HighCard;
                    PairedCard1 = middlePairTexture.PairCard1;
                    PairedCard2 = middlePairTexture.PairCard2;
                    if (board.River.Rank > middlePairTexture.LowCard.Rank)
                    {
                        ThirdCard = board.River;
                        LowCard = middlePairTexture.LowCard;
                    }
                    else
                    {
                        ThirdCard = middlePairTexture.LowCard;
                        LowCard = board.River;
                    }
                    break;
                case TurnBoardRankTextureEnum.Singles:
                    var singlesTexture = new Turn.TurnBoardRankTextures.SinglesRankTexture(TurnBoard);
                    if (board.River.Rank != singlesTexture.SecondCard.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighCard = singlesTexture.HighCard;
                    PairedCard1 = singlesTexture.SecondCard;
                    PairedCard2 = board.River;
                    ThirdCard = singlesTexture.ThirdCard;
                    LowCard = singlesTexture.LowCard;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            return outcome == SecondPairOutcomeEnum.GoodKicker
                || outcome == SecondPairOutcomeEnum.WeakKicker
                || outcome == SecondPairOutcomeEnum.NoneKicker;
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
                    var middlePairTuple = new Turn.TurnBoardRankTextures.HighPairRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = middlePairTuple.Item1.Grade();
                    turnInvolved = middlePairTuple.Item2;
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

        private Tuple<SecondPairOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            /*
        Foursome,
        HighSecondFullHouse,
        SecondHighFullHouse,
        SecondThirdFullHouse,
        SecondLowFullHouse,
        ThirdSecondFullHouse,
        LowSecondFullHouse,
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        ThreesomeTopKicker,
        ThreesomeGoodKicker,
        ThreesomeWeakKicker,
        ThreesomeNoneKicker,
        OverTwoPairs,
        OverSecondTwoPairs,
        OverThirdTwoPairs,
        OverLowTwoPairs,
        UnderTwoPairs,
        HighTwoPairsTopKicker,
        HighTwoPairsGoodKicker,
        HighTwoPairsWeakKicker,
        HighTwoPairsNoneKicker,
        ThirdTwoPairs,
        LowTwoPairs,
        TopKicker,
        GoodKicker,
        WeakKicker,
        NoneKicker*/
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == PairedCard1.Rank) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.Foursome, 2);
                if (grid.HighRank == HighCard.Rank) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.HighSecondFullHouse, 2);
                if (grid.HighRank == ThirdCard.Rank) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.ThirdSecondFullHouse, 2);
                if (grid.HighRank == LowCard.Rank) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.LowSecondFullHouse, 2);
            }

            if (grid.HighRank == HighCard.Rank && grid.LowRank == PairedCard1.Rank) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.SecondHighFullHouse, 2);
            if (grid.HighRank == PairedCard1.Rank && grid.LowRank == ThirdCard.Rank) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.SecondThirdFullHouse, 2);
            if (grid.HighRank == PairedCard1.Rank && grid.LowRank == LowCard.Rank) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.SecondLowFullHouse, 2);

            var boardRanks = new List<RankEnum>() { PairedCard1.Rank, HighCard.Rank, ThirdCard.Rank, LowCard.Rank };
            var playerRanks = grid.Category == GridCategoryEnum.Paired ? new List<RankEnum>() { grid.HighRank } :
                new List<RankEnum>() { grid.HighRank, grid.LowRank };

            var straightTuple = boardRanks.FindStraight(playerRanks);
            if (straightTuple != null)
            {
                if (straightTuple.Item1 == 2) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.StraightOverTwo, straightTuple.Item2);
                if (straightTuple.Item1 == 1) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.StraightOverOne, straightTuple.Item2);
                return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.StraightOverNone, straightTuple.Item2);
            }

            RankEnum tri = RankEnum.Undefined;
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
                if (kicker > HighCard.Rank)
                {
                    if (kicker == RankEnum.Ace) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.ThreesomeTopKicker, 1);
                    if (kicker > RankEnum.Ten) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.ThreesomeGoodKicker, 1);
                    return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.ThreesomeWeakKicker, 1);
                }
                return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.ThreesomeNoneKicker, 1);
            }

            if (grid.HighRank == HighCard.Rank)
            {
                tri = grid.HighRank;
                kicker = grid.LowRank;
            }
            else if (grid.LowRank == HighCard.Rank)
            {
                tri = grid.LowRank;
                kicker = grid.HighRank;
            }
            if (tri != RankEnum.Undefined && kicker != RankEnum.Undefined)
            {
                if (kicker > ThirdCard.Rank)
                {
                    if (kicker == RankEnum.Ace) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.HighTwoPairsTopKicker, 1);
                    if (kicker > RankEnum.Ten) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.HighTwoPairsGoodKicker, 1);
                    return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.HighTwoPairsWeakKicker, 1);
                }
                return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.HighTwoPairsNoneKicker, 1);
            }
            
            if (grid.HighRank == ThirdCard.Rank || grid.LowRank == ThirdCard.Rank) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.ThirdTwoPairs, 1);
            if (grid.HighRank == LowCard.Rank || grid.LowRank == LowCard.Rank) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.LowTwoPairs, 1);
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank > HighCard.Rank) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.OverTwoPairs, 2);
                if (grid.HighRank < HighCard.Rank && grid.HighRank > PairedCard1.Rank)
                    return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.OverSecondTwoPairs, 2);
                if (grid.HighRank < PairedCard1.Rank && grid.HighRank > ThirdCard.Rank)
                    return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.OverThirdTwoPairs, 2);
                if (grid.HighRank < ThirdCard.Rank && grid.HighRank > LowCard.Rank)
                    return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.OverLowTwoPairs, 2);
                if (grid.HighRank < LowCard.Rank) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.UnderTwoPairs, 2);
                throw new InvalidOperationException("Should be full house");
            }

            if (grid.HighRank > HighCard.Rank)
            {
                if (grid.HighRank == RankEnum.Ace) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.TopKicker, 0);
                if (grid.HighRank > RankEnum.Ten) return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.GoodKicker, 0);
                return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.WeakKicker, 0);
            }
            return new Tuple<SecondPairOutcomeEnum, int>(SecondPairOutcomeEnum.NoneKicker, 0);
        }
    }
}
