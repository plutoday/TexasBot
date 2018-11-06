using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Ranging;
using Turn;

namespace River.RiverBoardRankTextures
{
    public class SinglesRankTexture : IRiverBoardRankTexture
    {
        public Card HighCard { get; set; }
        public Card SecondCard { get; set; }
        public Card ThirdCard { get; set; }
        public Card FourthCard { get; set; }
        public Card LowCard { get; set; }

        public TurnBoard TurnBoard { get; set; }

        public SinglesRankTexture(RiverBoard board)
        {
            TurnBoard = board.TurnBoard;
            if (TurnBoard.RankTexture != TurnBoardRankTextureEnum.Singles)
            {
                throw new InvalidOperationException();
            }

            var singlesTexture = new Turn.TurnBoardRankTextures.SinglesRankTexture(TurnBoard);
            if (board.River.Rank == singlesTexture.HighCard.Rank
                || board.River.Rank == singlesTexture.SecondCard.Rank
                || board.River.Rank == singlesTexture.ThirdCard.Rank
                || board.River.Rank == singlesTexture.LowCard.Rank)
            {
                throw new InvalidOperationException();
            }

            var cards = new List<Card>()
            {
                board.River, singlesTexture.HighCard, singlesTexture.SecondCard, singlesTexture.ThirdCard, singlesTexture.LowCard
            };

            cards.Sort();
            HighCard = cards[4];
            SecondCard = cards[3];
            ThirdCard = cards[2];
            FourthCard = cards[1];
            LowCard = cards[0];
        }


        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            return outcome.Grade() == RankHandGradeEnum.HighCard;
        }

        public GridHitNewRoundResultEnum HitRiver(RangeGrid grid)
        {
            RankHandGradeEnum turnGrade;
            int turnInvolved;
            switch (TurnBoard.RankTexture)
            {
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

        private Tuple<SinglesOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {/*
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        HighSet,
        SecondSet,
        ThirdSet,
        FourthSet
        LowSet,
        HighSecondTwoPairs,
        HighThirdTwoPairs,
        HighFourthTwoPairs,
        HighLowTwoPairs,
        SecondThirdTwoPairs,
        SecondFourthTwoPairs,
        SecondLowTwoPairs,
        ThirdFourthTwoPairs,
        ThirdLowTwoPairs,
        OverTopPair,
        OverGoodPair,
        OverWeakPair,
        HighPair,
        SecondPair,
        ThirdPair,
        LowPair,
        TopKicker,
        GoodKicker,
        WeakKicker,
        NoneKicker*/

            var boardRanks = new List<RankEnum>() { HighCard.Rank, SecondCard.Rank, ThirdCard.Rank, FourthCard.Rank, LowCard.Rank };
            var playerRanks = grid.Category == GridCategoryEnum.Paired ? new List<RankEnum>() { grid.HighRank } :
                new List<RankEnum>() { grid.HighRank, grid.LowRank };

            var straightTuple = boardRanks.FindStraight(playerRanks);
            if (straightTuple != null)
            {
                if (straightTuple.Item1 == 2) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.StraightOverTwo, straightTuple.Item2);
                if (straightTuple.Item1 == 1) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.StraightOverOne, straightTuple.Item2);
                return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.StraightOverNone, straightTuple.Item2);
            }



            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == HighCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.HighSet, 2);
                if (grid.HighRank == SecondCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.SecondSet, 2);
                if (grid.HighRank == ThirdCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.ThirdSet, 2);
                if (grid.HighRank == FourthCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.FourthSet, 2);
                if (grid.HighRank == LowCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.LowSet, 2);
            }

            if (grid.HighRank == HighCard.Rank && grid.LowRank == SecondCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.HighSecondTwoPairs, 2);
            if (grid.HighRank == HighCard.Rank && grid.LowRank == ThirdCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.HighThirdTwoPairs, 2);
            if (grid.HighRank == HighCard.Rank && grid.LowRank == FourthCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.HighFourthTwoPairs, 2);
            if (grid.HighRank == HighCard.Rank && grid.LowRank == LowCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.HighLowTwoPairs, 2);
            if (grid.HighRank == SecondCard.Rank && grid.LowRank == ThirdCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.SecondThirdTwoPairs, 2);
            if (grid.HighRank == SecondCard.Rank && grid.LowRank == FourthCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.SecondFourthTwoPairs, 2);
            if (grid.HighRank == SecondCard.Rank && grid.LowRank == LowCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.SecondLowTwoPairs, 2);
            if (grid.HighRank == ThirdCard.Rank && grid.LowRank == FourthCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.ThirdFourthTwoPairs, 2);
            if (grid.HighRank == ThirdCard.Rank && grid.LowRank == LowCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.ThirdLowTwoPairs, 2);
            if (grid.HighRank == FourthCard.Rank && grid.LowRank == LowCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.FourthLowTwoPairs, 2);

            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank > HighCard.Rank)
                {
                    if (grid.HighRank == RankEnum.Ace) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.OverTopPair, 2);
                    if (grid.HighRank > RankEnum.Ten) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.OverGoodPair, 2);
                    return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.OverWeakPair, 2);
                }
                if (grid.HighRank > SecondCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.OverSecondPair, 2);
                if (grid.HighRank > ThirdCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.OverThirdPair, 2);
                if (grid.HighRank > FourthCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.OverFourthPair, 2);
                if (grid.HighRank > LowCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.OverLowPair, 2);
                return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.UnderPair, 2);
            }

            if (grid.HighRank == HighCard.Rank || grid.LowRank == HighCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.HighPair, 1);
            if (grid.HighRank == SecondCard.Rank || grid.LowRank == SecondCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.SecondPair, 1);
            if (grid.HighRank == ThirdCard.Rank || grid.LowRank == ThirdCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.ThirdPair, 1);
            if (grid.HighRank == FourthCard.Rank || grid.LowRank == FourthCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.FourthPair, 1);
            if (grid.HighRank == LowCard.Rank || grid.LowRank == LowCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.LowPair, 1);

            if (grid.HighRank > HighCard.Rank)
            {
                if (grid.HighRank == RankEnum.Ace) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.TopKicker, 0);
                if (grid.HighRank > RankEnum.Ten) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.GoodKicker, 0);
                return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.WeakKicker, 0);
            }
            return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.NoneKicker, 0);
        }
    }
}
