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
    public class LowPairRankTexture : IRiverBoardRankTexture
    {
        public Card HighCard { get; set; }
        public Card SecondCard { get; set; }
        public Card ThirdCard { get; set; }
        public Card PairedCard1 { get; set; }
        public Card PairedCard2 { get; set; }
        public TurnBoard TurnBoard { get; set; }

        public LowPairRankTexture(RiverBoard board)
        {
            TurnBoard = board.TurnBoard;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.LowPair:
                    var lowPairTexture = new Turn.TurnBoardRankTextures.LowPairRankTexture(TurnBoard);
                    if (board.River.Rank <= lowPairTexture.PairCard1.Rank
                        || board.River.Rank == lowPairTexture.HighCard.Rank
                        || board.River.Rank == lowPairTexture.MiddleCard.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    var cards = new List<Card>() { lowPairTexture.HighCard, lowPairTexture.MiddleCard, board.River };
                    cards.Sort();
                    HighCard = cards[2];
                    SecondCard = cards[1];
                    ThirdCard = cards[0];
                    PairedCard1 = lowPairTexture.PairCard1;
                    PairedCard2 = lowPairTexture.PairCard2;
                    break;
                case TurnBoardRankTextureEnum.Singles:
                    var singlesTexture = new Turn.TurnBoardRankTextures.SinglesRankTexture(TurnBoard);
                    if (board.River.Rank != singlesTexture.LowCard.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    HighCard = singlesTexture.HighCard;
                    SecondCard = singlesTexture.SecondCard;
                    ThirdCard = singlesTexture.ThirdCard;
                    PairedCard1 = singlesTexture.LowCard;
                    PairedCard2 = board.River;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            return outcome == LowPairOutcomeEnum.GoodKicker
                || outcome == LowPairOutcomeEnum.WeakKicker
                || outcome == LowPairOutcomeEnum.NoneKicker;
        }

        public GridHitNewRoundResultEnum HitRiver(RangeGrid grid)
        {
            RankHandGradeEnum turnGrade;
            int turnInvolved;
            switch (TurnBoard.RankTexture)
            {
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

        private Tuple<LowPairOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            /*
        Foursome,
        HighLowFullHouse,
        SecondLowFullHouse,
        ThirdLowFullHouse,
        LowHighFullHouse,
        LowSecondFullHouse,
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
        ThirdTwoPairs,
        TopKicker,
        GoodKicker,*/
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == PairedCard1.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.Foursome, 2);
                if (grid.HighRank == HighCard.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.HighLowFullHouse, 2);
                if (grid.HighRank == SecondCard.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.SecondLowFullHouse, 2);
                if (grid.HighRank == ThirdCard.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.ThirdLowFullHouse, 2);
            }

            if (grid.HighRank == HighCard.Rank && grid.LowRank == PairedCard1.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.LowHighFullHouse, 2);
            if (grid.LowRank == PairedCard1.Rank && grid.HighRank == SecondCard.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.LowSecondFullHouse, 2);
            if (grid.LowRank == PairedCard1.Rank && grid.HighRank == ThirdCard.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.LowThirdFullHouse, 2);

            var boardRanks = new List<RankEnum>() { PairedCard1.Rank, HighCard.Rank, SecondCard.Rank, ThirdCard.Rank };
            var playerRanks = grid.Category == GridCategoryEnum.Paired ? new List<RankEnum>() { grid.HighRank } :
                new List<RankEnum>() { grid.HighRank, grid.LowRank };

            var straightTuple = boardRanks.FindStraight(playerRanks);
            if (straightTuple != null)
            {
                if (straightTuple.Item1 == 2) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.StraightOverTwo, straightTuple.Item2);
                if (straightTuple.Item1 == 1) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.StraightOverOne, straightTuple.Item2);
                return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.StraightOverNone, straightTuple.Item2);
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
                    if (kicker == RankEnum.Ace) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.ThreesomeTopKicker, 1);
                    if (kicker > RankEnum.Ten) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.ThreesomeGoodKicker, 1);
                    return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.ThreesomeWeakKicker, 1);
                }
                return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.ThreesomeNoneKicker, 1);
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
                    if (kicker == RankEnum.Ace) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.HighTwoPairsTopKicker, 1);
                    if (kicker > RankEnum.Ten) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.HighTwoPairsGoodKicker, 1);
                    return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.HighTwoPairsWeakKicker, 1);
                }
                return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.HighTwoPairsNoneKicker, 1);
            }

            if (grid.HighRank == SecondCard.Rank || grid.LowRank == SecondCard.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.SecondTwoPairs, 1);
            if (grid.HighRank == ThirdCard.Rank || grid.LowRank == ThirdCard.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.ThirdTwoPairs, 1);
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank > HighCard.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.OverTwoPairs, 2);
                if (grid.HighRank < HighCard.Rank && grid.HighRank > SecondCard.Rank)
                    return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.OverSecondTwoPairs, 2);
                if (grid.HighRank < SecondCard.Rank && grid.HighRank > ThirdCard.Rank)
                    return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.OverThirdTwoPairs, 2);
                if (grid.HighRank < ThirdCard.Rank && grid.HighRank > PairedCard1.Rank)
                    return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.OverLowTwoPairs, 2);
                if (grid.HighRank < PairedCard1.Rank) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.UnderTwoPairs, 2);
                throw new InvalidOperationException("Should be full house");
            }

            if (grid.HighRank > HighCard.Rank)
            {
                if (grid.HighRank == RankEnum.Ace) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.TopKicker, 0);
                if (grid.HighRank > RankEnum.Ten) return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.GoodKicker, 0);
                return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.WeakKicker, 0);
            }
            return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.NoneKicker, 0);
        }
    }
}
