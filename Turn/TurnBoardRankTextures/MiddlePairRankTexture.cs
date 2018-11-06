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
    public class MiddlePairRankTexture : ITurnFlopBoardRankTexture
    {
        public FlopBoard FlopBoard { get; set; }
        public Card HighCard { get; set; }
        public Card PairCard1 { get; set; }
        public Card PairCard2 { get; set; }
        public Card LowCard { get; set; }

        public MiddlePairRankTexture(TurnBoard board)
        {
            FlopBoard = board.FlopBoard;

            var cards = new List<Card>() { FlopBoard.Flop1, FlopBoard.Flop2, FlopBoard.Flop3, board.TurnCard };
            cards.Sort();
            if (cards[1].Rank != cards[2].Rank)
            {
                throw new InvalidOperationException();
            }
            if (cards[0].Rank == cards[1].Rank || cards[2].Rank == cards[3].Rank)
            {
                throw new InvalidOperationException();
            }

            PairCard1 = cards[1];
            PairCard2 = cards[2];
            LowCard = cards[0];
            HighCard = cards[3];
        }
        public BoardRangeGridStatusEnum TestGridAgainstFlopBoard(RangeGrid grid)
        {
            throw new NotImplementedException();
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            return TestGridAgainstBoard(grid).Item1.Grade() <= RankHandGradeEnum.OnePair;
        }

        public GridHitNewRoundResultEnum HitTurn(RangeGrid grid)
        {
            RankHandGradeEnum flopGrade;
            int flopInvolved;
            switch (FlopBoard.RankTexture)
            {
                case FlopBoardRankTextureEnum.HighPair:
                    var highPairTuple = new Flop.FlopBoardRankTextures.HighPairRankTexture(FlopBoard).TestGridAgainstBoard(grid);
                    flopGrade = highPairTuple.Item1.Grade();
                    flopInvolved = highPairTuple.Item2;
                    break;
                case FlopBoardRankTextureEnum.LowPair:
                    var lowPairTuple = new Flop.FlopBoardRankTextures.LowPairRankTexture(FlopBoard).TestGridAgainstBoard(grid);
                    flopGrade = lowPairTuple.Item1.Grade();
                    flopInvolved = lowPairTuple.Item2;
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

        public Tuple<MiddlePairOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {/*
        Foursome,
        HighFullHouse,
        MiddleFullHouseHighPair,
        MiddleFullHouseLowPair,
        LowFullHouse,
        StraightOverTwo,
        StraightOverOne,
        StraightOverNone,
        ThreesomeTopKicker,
        ThreesomeOverGoodKicker,
        ThreesomeOverWeakKicker,
        ThreesomeNoneKicker,*/
            if (grid.Category == GridCategoryEnum.Paired && grid.HighRank == PairCard1.Rank)
                return new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.Foursome, 2);
            if (grid.Category == GridCategoryEnum.Paired && grid.HighRank == HighCard.Rank)
                return new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.HighFullHouse,2);
            if (grid.Category == GridCategoryEnum.Paired && grid.HighRank == LowCard.Rank)
                return new Tuple<MiddlePairOutcomeEnum, int>( MiddlePairOutcomeEnum.LowFullHouse,2);
            if (grid.HighRank == HighCard.Rank && grid.LowRank == PairCard1.Rank)
                return new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.MiddleFullHouseHighPair, 2);
            if (grid.HighRank == PairCard1.Rank && grid.LowRank == LowCard.Rank)
                return new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.MiddleFullHouseLowPair, 2);

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
                if (kicker == RankEnum.Ace)
                    return new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.ThreesomeTopKicker, 1);
                if (kicker > HighCard.Rank)
                    return kicker > RankEnum.Ten
                        ? new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.ThreesomeOverGoodKicker, 1)
                        : new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.ThreesomeOverWeakKicker,1);
                return new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.ThreesomeNoneKicker,1);
            }

            /*        
        OverTwoPairs,
        HighTwoPairs,
        AboveMiddleTwoPairs,
        AboveLowTwoPairs,
        LowTwoPairs,
        UnderTwoPairs,
        TopKicker,
        OverGoodKicker,
        OverWeakKicker,
        NoneKicker,*/

            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank > HighCard.Rank)
                    return new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.OverTwoPairs, 2);
                if (grid.HighRank > PairCard1.Rank)
                    return new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.AboveMiddleTwoPairs,2);
                if (grid.HighRank > LowCard.Rank)
                    return new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.AboveLowTwoPairs, 2);
                return new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.UnderTwoPairs, 2);
            }

            if (grid.HighRank == HighCard.Rank || grid.LowRank == HighCard.Rank)
                return new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.HighTwoPairs, 1);
            if (grid.HighRank == LowCard.Rank || grid.LowRank == LowCard.Rank)
                return new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.LowTwoPairs, 1);
            
            var ranks = new List<RankEnum>() { PairCard1.Rank, HighCard.Rank, LowCard.Rank };
            if (ranks.All(r => r != grid.HighRank)) ranks.Add(grid.HighRank);
            if (ranks.All(r => r != grid.LowRank)) ranks.Add(grid.LowRank);
            if (ranks.Count == 5)
            {
                ranks.Sort();
                if (ranks[4] == ranks[0] + 4)
                {
                    if (ranks[4] == PairCard1.Rank + 2)
                        return new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.StraightOverTwo,2);
                    if (ranks[4] == PairCard1.Rank + 1)
                        return new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.StraightOverOne,2);
                    if (ranks[4] == PairCard1.Rank + 0)
                        return new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.StraightOverNone, 2);
                    throw new InvalidOperationException();
                }
            }

            if (grid.HighRank == RankEnum.Ace)
                return new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.TopKicker,0);
            if (grid.HighRank > PairCard1.Rank)
                return grid.HighRank > RankEnum.Ten
                    ? new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.OverGoodKicker,0)
                    :  new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.OverWeakKicker,0);

            return new Tuple<MiddlePairOutcomeEnum, int>(MiddlePairOutcomeEnum.NoneKicker,0);
        }
    }
}
