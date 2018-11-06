using System;
using System.Collections.Generic;
using System.Linq;
using Infra;
using Models;
using Models.Ranging;

namespace Flop.FlopBoardRankTextures
{
    public class LowPairRankTexture : IFlopBoardRankTexture
    {
        public Card PairCard1 { get; set; }
        public Card PairCard2 { get; set; }
        public Card SingleCard { get; set; }

        public LowPairRankTexture(FlopBoard board)
        {
            var cards = new List<Card>() { board.Flop1, board.Flop2, board.Flop3 };
            cards.Sort();
            if (cards[0].Rank != cards[1].Rank)
            {
                throw new InvalidOperationException();
            }

            if (cards[1].Rank == cards[2].Rank)
            {
                throw new InvalidOperationException();
            }

            PairCard1 = cards[0];
            PairCard2 = cards[1];
            SingleCard = cards[2];
        }

        public BoardRangeGridStatusEnum TestGridAgainstFlopBoard(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            Logger.Instance.Log($"Testing {grid.ToString()} against LowPair flop, outcome is {outcome}");
            switch (outcome)
            {
                case LowPairOutcomeEnum.FourSome:
                case LowPairOutcomeEnum.HighFullHouse:
                    return BoardRangeGridStatusEnum.Nuts;
                case LowPairOutcomeEnum.LowFullHouse:
                case LowPairOutcomeEnum.ThreeSomeTopKicker:
                case LowPairOutcomeEnum.ThreeSomeGoodKicker:
                    return BoardRangeGridStatusEnum.Elite;
                case LowPairOutcomeEnum.ThreeSomeWeakKicker:
                case LowPairOutcomeEnum.OverTwoPairs:
                    return BoardRangeGridStatusEnum.Good;
                case LowPairOutcomeEnum.OnePairTopKicker:
                case LowPairOutcomeEnum.OnePairGoodKicker:
                case LowPairOutcomeEnum.OnePairWeakKicker:
                    return ShouldRankDrawFold(grid) ? BoardRangeGridStatusEnum.Trash : BoardRangeGridStatusEnum.Marginal;
                default:
                    return BoardRangeGridStatusEnum.Marginal;
            }
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            //todo consider betSize later

            switch (outcome)
            {
                case LowPairOutcomeEnum.OnePairTopKicker:
                case LowPairOutcomeEnum.OnePairGoodKicker:
                case LowPairOutcomeEnum.OnePairWeakKicker:
                    return ShouldRankDrawFold(grid);
            }

            return false;
        }

        private bool ShouldRankDrawFold(RangeGrid grid)
        {
            var rankDraw =
                grid.TestRankDrawOnFlop(new List<RankEnum>() { SingleCard.Rank, PairCard1.Rank, PairCard2.Rank });
            switch (rankDraw)
            {
                case RankDrawEnum.Nothing:
                    return true;
                default:
                    return false;
            }
        }

        public Tuple<LowPairOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            /*
           FourSome,   //66
       HighFullHouse,  //JJ
       LowFullHouse,   //J6
       ThreeSomeTopKicker,  //A6
       ThreeSomeGoodKicker,    //Q6
       ThreeSomeWeakKicker,    //76
       OverTwoPairs,   //AA
       TopTwoPairsTopKicker,    //AJ
       TopTwoPairsGoodKicker,  //QJ
       TopTwoPairsWeakKicker,  //J5
       BetweenTwoPairs, //TT
       UnderTwoPairs,  //55
       OpenStraightDraw,   //
       OnePairTopKicker,   //AK
       OnePairGoodKicker,  //QT
       OnePairWeakKicker,  //75
       */

            if (grid.HighRank == PairCard1.Rank && grid.LowRank == PairCard2.Rank)
            {
                return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.FourSome, 2);
            }

            if (grid.HighRank == SingleCard.Rank && grid.LowRank == SingleCard.Rank)
            {
                return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.HighFullHouse, 2);
            }

            if (grid.HighRank == SingleCard.Rank && grid.LowRank == PairCard1.Rank)
            {
                return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.LowFullHouse, 2);
            }

            RankEnum kicker = RankEnum.Undefined;
            if (grid.HighRank == PairCard1.Rank)
            {
                kicker = grid.LowRank;
            }
            else if (grid.LowRank == PairCard1.Rank)
            {
                kicker = grid.HighRank;
            }

            if (kicker != RankEnum.Undefined)
            {
                if (kicker == RankEnum.Ace)
                {
                    return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.ThreeSomeTopKicker, 1);
                }
                if (kicker > RankEnum.Ten)
                {
                    return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.ThreeSomeGoodKicker, 1);
                }
                return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.ThreeSomeWeakKicker, 1);
            }

            if (grid.HighRank == grid.LowRank && grid.HighRank > SingleCard.Rank)
            {
                return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.OverTwoPairs, 2);
            }

            kicker = RankEnum.Undefined;
            if (grid.HighRank == SingleCard.Rank)
            {
                kicker = grid.LowRank;
            }
            else if (grid.LowRank == SingleCard.Rank)
            {
                kicker = grid.HighRank;
            }

            if (kicker != RankEnum.Undefined)
            {
                if (kicker == RankEnum.Ace)
                {
                    return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.TopTwoPairsTopKicker, 1);
                }
                if (kicker > RankEnum.Ten)
                {
                    return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.TopTwoPairsGoodKicker, 1);
                }
                return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.TopTwoPairsWeakKicker, 1);
            }

            if (grid.Category == GridCategoryEnum.Paired && grid.HighRank < SingleCard.Rank && grid.HighRank > PairCard1.Rank)
            {
                return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.BetweenTwoPairs, 2);
            }

            if (grid.Category == GridCategoryEnum.Paired && grid.HighRank < PairCard1.Rank)
            {
                return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.UnderTwoPairs, 2);
            }

            List<RankEnum> ranks = new List<RankEnum>() { PairCard1.Rank, SingleCard.Rank, grid.HighRank, grid.LowRank };

            kicker = ranks.Where(r => r != PairCard1.Rank && r != SingleCard.Rank).Max();
            if (kicker == RankEnum.Ace)
            {
                return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.OnePairTopKicker, 0);
            }
            if (kicker > RankEnum.Ten)
            {
                return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.OnePairGoodKicker, 0);
            }
            return new Tuple<LowPairOutcomeEnum, int>(LowPairOutcomeEnum.OnePairWeakKicker, 0);
        }
    }
}
