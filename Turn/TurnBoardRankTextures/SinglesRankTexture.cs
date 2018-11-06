using System;
using System.Collections.Generic;
using System.Linq;
using Flop;
using Models;
using Models.Ranging;

namespace Turn.TurnBoardRankTextures
{
    public class SinglesRankTexture : ITurnFlopBoardRankTexture
    {
        public Card HighCard { get; set; } //highest
        public Card SecondCard { get; set; }
        public Card ThirdCard { get; set; }
        public Card LowCard { get; set; }
        public FlopBoard FlopBoard { get; set; }

        public SinglesRankTexture(TurnBoard board)
        {
            FlopBoard = board.FlopBoard;
            if (board.FlopBoard.RankTexture != FlopBoardRankTextureEnum.Singles)
            {
                throw new InvalidOperationException();
            }

            if (board.TurnCard.Rank == FlopBoard.Flop1.Rank
                || board.TurnCard.Rank == FlopBoard.Flop2.Rank
                || board.TurnCard.Rank == FlopBoard.Flop3.Rank)
            {
                throw new InvalidOperationException();
            }

            var cards = new List<Card>() {FlopBoard.Flop1, FlopBoard.Flop2, FlopBoard.Flop3, board.TurnCard};
            cards.Sort();
            cards.Reverse();
            HighCard = cards[0];
            SecondCard = cards[1];
            ThirdCard = cards[2];
            LowCard = cards[3];
        }

        public BoardRangeGridStatusEnum TestGridAgainstFlopBoard(RangeGrid grid)
        {
            throw new NotImplementedException();
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var grade = TestGridAgainstBoard(grid).Item1.Grade();
            var hit = HitTurn(grid);

            return hit == GridHitNewRoundResultEnum.None && grade <= RankHandGradeEnum.OnePair;
        }

        public GridHitNewRoundResultEnum HitTurn(RangeGrid grid)
        {
            RankHandGradeEnum flopGrade;
            int flopInvolved;
            switch (FlopBoard.RankTexture)
            {
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

        public Tuple<SinglesOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            /*
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,*/

            var ranks = new List<RankEnum>() {HighCard.Rank, SecondCard.Rank, ThirdCard.Rank, LowCard.Rank};
            if (ranks.All(r => r != grid.HighRank)) ranks.Add(grid.HighRank);
            if (ranks.All(r => r != grid.LowRank)) ranks.Add(grid.LowRank);

            ranks.Sort();
            if (ranks.Count == 5)
            {
                if (ranks[4] == ranks[0] + 4)
                {
                    //straight
                    if (ranks[4] == HighCard.Rank + 2)
                        throw new InvalidOperationException();
                    if (ranks[4] == HighCard.Rank + 1)
                        return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.StraightOverOne, 1);
                    if (ranks[4] <= HighCard.Rank)
                        return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.StraightOverNone, 1);
                }
            }
            if (ranks.Count == 6)
            {
                int holesUsed = 0;
                if (ranks[5] == ranks[1] + 4)
                {
                    //straight
                    if (ranks[5] >= grid.HighRank && grid.HighRank >= ranks[1]) holesUsed++;
                    if (ranks[5] >= grid.LowRank && grid.LowRank >= ranks[1]) holesUsed++;
                    if (ranks[5] == HighCard.Rank + 2) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.StraightOverTwo, holesUsed);
                    if (ranks[5] == HighCard.Rank + 1) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.StraightOverOne, holesUsed);
                    if (ranks[5] <= HighCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.StraightOverNone, holesUsed);
                }

                if (ranks[4] == ranks[0] + 4)
                {
                    //straight
                    if (ranks[4] >= grid.HighRank && grid.HighRank >= ranks[0]) holesUsed++;
                    if (ranks[4] >= grid.LowRank && grid.LowRank >= ranks[0]) holesUsed++;
                    if (ranks[4] == HighCard.Rank + 2) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.StraightOverTwo, holesUsed);
                    if (ranks[4] == HighCard.Rank + 1) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.StraightOverOne, holesUsed);
                    if (ranks[4] <= HighCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.StraightOverNone, holesUsed);
                }
            }

            /*
             
        HighSet,
        SecondSet,
        ThirdSet,
        LowSet,
        HighSecondTwoPairs,
        HighThirdTwoPairs,
        HighLowTwoPairs,
        SecondThirdTwoPairs,
        SecondLowTwoPairs,
        ThirdLowTwoPairs,
        OverTopPair,
        OverGoodPair,
        OverWeakPair,
        HighPairTopKicker,
        HighPairGoodKicker,
        HighPairWeakKicker,
        SecondPair,
        ThirdPair,
        LowPair,
        HighCard,*/

            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == HighCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.HighSet, 2);
                if (grid.HighRank == SecondCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.SecondSet, 2);
                if (grid.HighRank == ThirdCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.ThirdSet, 2);
                if (grid.HighRank == LowCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.LowSet, 2);
            }

            if (grid.HighRank == HighCard.Rank && grid.LowRank == SecondCard.Rank)
                return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.HighSecondTwoPairs, 2);
            if (grid.HighRank == HighCard.Rank && grid.LowRank ==  ThirdCard.Rank)
                return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.HighThirdTwoPairs, 2);
            if (grid.HighRank == HighCard.Rank && grid.LowRank == LowCard.Rank)
                return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.HighLowTwoPairs,2);
            if (grid.HighRank == SecondCard.Rank && grid.LowRank == ThirdCard.Rank)
                return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.SecondThirdTwoPairs, 2);
            if (grid.HighRank == SecondCard.Rank && grid.LowRank == LowCard.Rank)
                return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.SecondLowTwoPairs,2);
            if (grid.HighRank == ThirdCard.Rank && grid.LowRank == LowCard.Rank)
                return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.ThirdLowTwoPairs,2); 

            if (grid.Category == GridCategoryEnum.Paired && grid.HighRank == RankEnum.Ace)
                return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.OverTopPair, 2);
            if (grid.Category == GridCategoryEnum.Paired && grid.HighRank > RankEnum.Ten)
                return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.OverGoodPair, 2);
            if (grid.Category == GridCategoryEnum.Paired)
                return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.OverWeakPair,2);

            RankEnum pairedRank = RankEnum.Undefined;
            RankEnum kicker =  RankEnum.Undefined;
            if (grid.HighRank == HighCard.Rank || grid.HighRank == SecondCard.Rank || grid.HighRank == ThirdCard.Rank ||
                grid.HighRank == LowCard.Rank)
            {
                pairedRank = grid.HighRank;
                kicker = grid.LowRank;
            }
            if (grid.LowRank == HighCard.Rank || grid.LowRank == SecondCard.Rank || grid.LowRank == ThirdCard.Rank ||
               grid.LowRank == LowCard.Rank)
            {
                pairedRank = grid.LowRank;
                kicker = grid.HighRank;
            }

            if (pairedRank != RankEnum.Undefined && kicker != RankEnum.Undefined)
            {
                if (pairedRank == HighCard.Rank)
                {
                    return kicker == RankEnum.Ace
                        ? new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.HighPairTopKicker,1)
                        :  kicker > RankEnum.Ten
                            ? new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.HighPairGoodKicker, 1)
                             : new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.HighPairWeakKicker, 1);
                }

                if (pairedRank == SecondCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.SecondPair, 1);
                if (pairedRank == ThirdCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.ThirdPair, 1);
                if (pairedRank == LowCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.LowPair, 1);
            }

            return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.HighCard, 0);
        }
    }
}
