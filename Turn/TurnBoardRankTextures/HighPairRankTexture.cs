using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flop;
using Models;
using Models.Ranging;

namespace Turn.TurnBoardRankTextures
{
    public class HighPairRankTexture : ITurnFlopBoardRankTexture
    {
        public FlopBoard FlopBoard { get; set; }
        public Card PairCard1 { get; set; }
        public Card PairCard2 { get; set; }
        public Card MiddleCard { get; set; }
        public Card LowCard { get; set; }

        public HighPairRankTexture(TurnBoard board)
        {
            FlopBoard = board.FlopBoard;
            var cards = new List<Card>() { FlopBoard.Flop1, FlopBoard.Flop2, FlopBoard.Flop3, board.TurnCard };
            cards.Sort();
            if (cards[2].Rank != cards[3].Rank)
            {
                throw new InvalidOperationException();
            }
            if (cards[0].Rank == cards[1].Rank || cards[1].Rank == cards[2].Rank)
            {
                throw new InvalidOperationException();
            }

            PairCard1 = cards[2];
            PairCard2 = cards[3];
            LowCard = cards[0];
            MiddleCard = cards[1];
        }

        public BoardRangeGridStatusEnum TestGridAgainstFlopBoard(RangeGrid grid)
        {
            throw new NotImplementedException();
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            
            return outcome.Grade() <= RankHandGradeEnum.OnePair;
        }

        public GridHitNewRoundResultEnum HitTurn(RangeGrid grid)
        {
            RankHandGradeEnum flopGrade;
            int flopInvolved;
            switch(FlopBoard.RankTexture)
            {
                case FlopBoardRankTextureEnum.HighPair:
                    var highPairTuple = new Flop.FlopBoardRankTextures.HighPairRankTexture(FlopBoard).TestGridAgainstBoard(grid);
                    flopGrade = highPairTuple.Item1.Grade();
                    flopInvolved = highPairTuple.Item2;
                    break;
                case FlopBoardRankTextureEnum.Singles:
                    var singlesTuple = new Flop.FlopBoardRankTextures.SinglesRankTexture(FlopBoard).TestGridAgainstBoard(grid);
                    flopGrade = singlesTuple.Item1.Grade();
                    flopInvolved = singlesTuple.Item2;
                    break;
                default:
                    throw new InvalidOperationException($"Impossible to have {FlopBoard.RankTexture}");
            }
            var turnTuple = TestGridAgainstBoard(grid);
            var turnOutcomeGrade = turnTuple.Item1.Grade();
            var turnInvolved = turnTuple.Item2;

            if (turnOutcomeGrade > flopGrade)
            {
                return (turnInvolved > flopInvolved) ? GridHitNewRoundResultEnum.Promoted : GridHitNewRoundResultEnum.Enhanced;
            }
            return GridHitNewRoundResultEnum.None;
        }

        /// <summary>
        /// int means how many cards got involved, kicker not counted
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public Tuple<HighPairOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {/*
        Foursome,
        HighFullHouseMiddlePair,
        HighFullHouseLowPair,
        MiddleFullHouse,
        LowFullHouse,
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        ThreesomeTopKicker,
        ThreesomeOverGoodKicker,
        ThreesomeOverWeakKicker,
        ThreesomeNoneKicker,*/
            if (grid.Category == GridCategoryEnum.Paired && grid.HighRank == PairCard1.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.Foursome, 2);
            if (grid.HighRank == PairCard1.Rank && grid.LowRank == MiddleCard.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.HighFullHouseMiddlePair, 2);
            if (grid.HighRank == PairCard1.Rank && grid.LowRank == LowCard.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.HighFullHouseLowPair, 2);
            if (grid.Category == GridCategoryEnum.Paired && grid.HighRank == MiddleCard.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.MiddleFullHouse, 2);
            if (grid.Category == GridCategoryEnum.Paired && grid.HighRank == LowCard.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.LowFullHouse, 2);

            RankEnum triRank = RankEnum.Undefined, kicker = RankEnum.Undefined;
            if (grid.HighRank == PairCard1.Rank)
            {
                triRank = grid.HighRank;
                kicker = grid.LowRank;
            }
            else if (grid.LowRank == PairCard1.Rank)
            {
                triRank = grid.LowRank;
                kicker = grid.HighRank;
            }

            if (triRank != RankEnum.Undefined && kicker != RankEnum.Undefined)
            {
                if (kicker == RankEnum.Ace) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.ThreesomeTopKicker, 1);
                if (kicker > MiddleCard.Rank)
                    return kicker > RankEnum.Ten
                        ? new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.ThreesomeOverGoodKicker, 1)
                        : new Tuple<HighPairOutcomeEnum, int>( HighPairOutcomeEnum.ThreesomeOverWeakKicker, 1);
                return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.ThreesomeNoneKicker, 1);
            }

            /*
        OverTwoPairs,
        AboveMiddleTwoPairs,
        MiddleTwoPairs,
        AboveLowTwoPairs,
        LowTwoPairs,
        UnderTwoPairs,
        TopKicker,
        OverGoodKicker,
        OverWeakKicker,
        NoneKicker,*/

            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank > PairCard1.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.OverTwoPairs, 2);
                if (grid.HighRank > MiddleCard.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.AboveMiddleTwoPairs, 2);
                if (grid.HighRank > LowCard.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.AboveLowTwoPairs, 2);
                return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.UnderTwoPairs, 2);
            }

            if (grid.HighRank == MiddleCard.Rank || grid.LowRank == MiddleCard.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.MiddleTwoPairs, 1);
            if (grid.HighRank == LowCard.Rank || grid.LowRank == LowCard.Rank) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.LowTwoPairs, 1);

            var ranks = new List<RankEnum>() { PairCard1.Rank, MiddleCard.Rank, LowCard.Rank };
            if (ranks.All(r => r != grid.HighRank)) ranks.Add(grid.HighRank);
            if (ranks.All(r => r != grid.LowRank)) ranks.Add(grid.LowRank);
            if (ranks.Count == 5)
            {
                ranks.Sort();
                if (ranks[4] == ranks[0] + 4)
                {
                    if (ranks[4] == PairCard1.Rank + 2) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.StraightOverTwo, 2);
                    if (ranks[4] == PairCard1.Rank + 1) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.StraightOverOne, 2);
                    if (ranks[4] == PairCard1.Rank + 0) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.StraightOverNone, 2);
                    throw new InvalidOperationException();
                }
            }

            if (grid.HighRank == RankEnum.Ace) return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.TopKicker, 0);
            if (grid.HighRank > PairCard1.Rank)
                return grid.HighRank > RankEnum.Ten
                    ? new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.OverGoodKicker, 0)
                    : new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.OverWeakKicker, 0);

            return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.NoneKicker, 0);
        }
    }
}
