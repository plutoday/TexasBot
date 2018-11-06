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
    public class ThirdPairRankTexture : IRiverBoardRankTexture
    {
        public Card HighCard { get; set; }
        public Card SecondCard { get; set; }
        public Card PairedCard1 { get; set; }
        public Card PairedCard2 { get; set; }
        public Card LowCard { get; set; }
        public TurnBoard TurnBoard { get; set; }

        public ThirdPairRankTexture(RiverBoard board)
        {
            TurnBoard = board.TurnBoard;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.MiddlePair:
                    var middlePairTexture = new MiddlePairRankTexture(TurnBoard);
                    if (board.River.Rank <= middlePairTexture.PairCard1.Rank
                        || board.River.Rank == middlePairTexture.HighCard.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    if (board.River.Rank > middlePairTexture.HighCard.Rank)
                    {
                        HighCard = board.River;
                        SecondCard = middlePairTexture.HighCard;
                    }
                    else
                    {
                        HighCard = middlePairTexture.HighCard;
                        SecondCard = board.River;
                    }
                    PairedCard1 = middlePairTexture.PairCard1;
                    PairedCard2 = middlePairTexture.PairCard2;
                    LowCard = middlePairTexture.LowCard;
                    break;
                case TurnBoardRankTextureEnum.LowPair:
                    var lowPairTexture = new Turn.TurnBoardRankTextures.LowPairRankTexture(TurnBoard);
                    if (board.River.Rank >= lowPairTexture.PairCard1.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighCard = lowPairTexture.HighCard;
                    SecondCard = lowPairTexture.MiddleCard;
                    PairedCard1 = lowPairTexture.PairCard1;
                    PairedCard2 = lowPairTexture.PairCard2;
                    LowCard = board.River;
                    break;
                case TurnBoardRankTextureEnum.Singles:
                    var singlesTexture = new Turn.TurnBoardRankTextures.SinglesRankTexture(TurnBoard);
                    if (board.River.Rank != singlesTexture.ThirdCard.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighCard = singlesTexture.HighCard;
                    SecondCard = singlesTexture.SecondCard;
                    PairedCard1 = singlesTexture.ThirdCard;
                    PairedCard2 = board.River;
                    LowCard = singlesTexture.LowCard;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            return outcome == ThirdPairOutcomeEnum.GoodKicker
                   || outcome == ThirdPairOutcomeEnum.WeakKicker
                   || outcome == ThirdPairOutcomeEnum.NoneKicker;
        }

        public GridHitNewRoundResultEnum HitRiver(RangeGrid grid)
        {
            RankHandGradeEnum turnGrade;
            int turnInvolved;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.MiddlePair:
                    var middlePairTuple = new Turn.TurnBoardRankTextures.HighPairRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = middlePairTuple.Item1.Grade();
                    turnInvolved = middlePairTuple.Item2;
                    break;
                case TurnBoardRankTextureEnum.LowPair:
                    var lowPairTuple = new Turn.TurnBoardRankTextures.LowPairRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = lowPairTuple.Item1.Grade();
                    turnInvolved = lowPairTuple.Item2;
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

        private Tuple<ThirdPairOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            /*
        Foursome,
        HighThirdFullHouse,
        SecondThirdFullHouse,
        ThirdHighFullHouse,
        ThirdSecondFullHouse,
        ThirdLowFullHouse,
        LowThirdFullHouse,
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
        SecondTwoPairs,
        LowTwoPairs,
        TopKicker,
        GoodKicker,
        WeakKicker,
        NoneKicker*/
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == PairedCard1.Rank) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.Foursome, 2);
                if (grid.HighRank == HighCard.Rank) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.HighThirdFullHouse, 2);
                if (grid.HighRank == SecondCard.Rank) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.SecondThirdFullHouse, 2);
                if (grid.HighRank == LowCard.Rank) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.LowThirdFullHouse, 2);
            }

            if (grid.HighRank == HighCard.Rank && grid.LowRank == PairedCard1.Rank) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.HighThirdFullHouse, 2);
            if (grid.LowRank == PairedCard1.Rank && grid.HighRank == SecondCard.Rank) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.ThirdSecondFullHouse, 2);
            if (grid.HighRank == PairedCard1.Rank && grid.LowRank == LowCard.Rank) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.ThirdLowFullHouse, 2);

            var boardRanks = new List<RankEnum>() { PairedCard1.Rank, HighCard.Rank, SecondCard.Rank, LowCard.Rank };
            var playerRanks = grid.Category == GridCategoryEnum.Paired ? new List<RankEnum>() { grid.HighRank } :
                new List<RankEnum>() { grid.HighRank, grid.LowRank };

            var straightTuple = boardRanks.FindStraight(playerRanks);
            if (straightTuple != null)
            {
                if (straightTuple.Item1 == 2) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.StraightOverTwo, straightTuple.Item2);
                if (straightTuple.Item1 == 1) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.StraightOverOne, straightTuple.Item2);
                return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.StraightOverNone, straightTuple.Item2);
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
                    if (kicker == RankEnum.Ace) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.ThreesomeTopKicker, 1);
                    if (kicker > RankEnum.Ten) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.ThreesomeGoodKicker, 1);
                    return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.ThreesomeWeakKicker, 1);
                }
                return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.ThreesomeNoneKicker, 1);
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
                if (kicker > SecondCard.Rank)
                {
                    if (kicker == RankEnum.Ace) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.HighTwoPairsTopKicker, 1);
                    if (kicker > RankEnum.Ten) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.HighTwoPairsGoodKicker, 1);
                    return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.HighTwoPairsWeakKicker, 1);
                }
                return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.HighTwoPairsNoneKicker, 1);
            }

            if (grid.HighRank == SecondCard.Rank || grid.LowRank == SecondCard.Rank) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.SecondTwoPairs, 1);
            if (grid.HighRank == LowCard.Rank || grid.LowRank == LowCard.Rank) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.LowTwoPairs, 1);
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank > HighCard.Rank) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.OverTwoPairs, 2);
                if (grid.HighRank < HighCard.Rank && grid.HighRank > SecondCard.Rank)
                    return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.OverSecondTwoPairs, 2);
                if (grid.HighRank < SecondCard.Rank && grid.HighRank > PairedCard1.Rank)
                    return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.OverThirdTwoPairs, 2);
                if (grid.HighRank < PairedCard1.Rank && grid.HighRank > LowCard.Rank)
                    return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.OverLowTwoPairs, 2);
                if (grid.HighRank < LowCard.Rank) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.UnderTwoPairs, 2);
                throw new InvalidOperationException("Should be full house");
            }

            if (grid.HighRank > HighCard.Rank)
            {
                if (grid.HighRank == RankEnum.Ace) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.TopKicker, 0);
                if (grid.HighRank > RankEnum.Ten) return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.GoodKicker, 0);
                return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.WeakKicker, 0);
            }
            return new Tuple<ThirdPairOutcomeEnum, int>(ThirdPairOutcomeEnum.NoneKicker, 0);
        }
    }
}
