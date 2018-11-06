using System;
using System.Collections.Generic;
using System.Linq;
using Infra;
using Models;
using Models.Ranging;

namespace Flop.FlopBoardRankTextures
{
    public class HighPairRankTexture : IFlopBoardRankTexture
    {
        public Card PairCard1 { get; set; }
        public Card PairCard2 { get; set; }
        public Card SingleCard { get; set; }

        public HighPairRankTexture(FlopBoard board)
        {
            var cards = new List<Card>() { board.Flop1, board.Flop2, board.Flop3 };
            cards.Sort();
            if (cards[1].Rank != cards[2].Rank)
            {
                throw new InvalidOperationException();
            }

            if (cards[0].Rank == cards[1].Rank)
            {
                throw new InvalidOperationException();
            }

            SingleCard = cards[0];
            PairCard1 = cards[1];
            PairCard2 = cards[2];
        }

        public BoardRangeGridStatusEnum TestGridAgainstFlopBoard(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            BoardRangeGridStatusEnum result;
            switch (outcome)
            {
                case HighPairOutcomeEnum.FourSome:
                case HighPairOutcomeEnum.HighFullHouse:
                    result = BoardRangeGridStatusEnum.Nuts;
                    break;
                case HighPairOutcomeEnum.LowFullHouse:
                case HighPairOutcomeEnum.ThreeSomeTopKicker:
                case HighPairOutcomeEnum.ThreeSomeGoodKicker:
                    result = BoardRangeGridStatusEnum.Elite;
                    break;
                case HighPairOutcomeEnum.ThreeSomeWeakKicker:
                case HighPairOutcomeEnum.OverTwoPairs:
                    result = BoardRangeGridStatusEnum.Good;
                    break;
                case HighPairOutcomeEnum.OnePairTopKicker:
                case HighPairOutcomeEnum.OnePairGoodKicker:
                case HighPairOutcomeEnum.OnePairWeakKicker:
                    result = ShouldRankDrawFold(grid) ? BoardRangeGridStatusEnum.Trash : BoardRangeGridStatusEnum.Marginal;
                    break;
                default:
                    result = BoardRangeGridStatusEnum.Marginal;
                    break;
            }
            Logger.Instance.Log($"{grid.ToString()} tested on HighPair flop, outcome:{outcome}|result:{result}");
            return result;
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            //todo consider betSize later

            switch (outcome)
            {
                case HighPairOutcomeEnum.OnePairTopKicker:
                case HighPairOutcomeEnum.OnePairGoodKicker:
                case HighPairOutcomeEnum.OnePairWeakKicker:
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

        public Tuple<HighPairOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            /*
            JJ7 for example
            a.FourSome,   //JJ,   nuts
            b.HighFullHouse,  //J7,   nuts
            c.LowFullHouse,   //77,   b possible, elite
            d.ThreeSomeTopKicker,  //AJ,  bc possible, elite
            e.ThreeSomeGoodKicker,    //QJ, bcd possible, elite
            f.ThreeSomeWeakKicker,    //J8, bcde possible, elite
            g.OverTwoPairs,   //AA, abcdef possible, marginal
            h.BetweenTwoPairs, //TT, abcdefg possible
            i.BottomTwoPairsTopKicker,    //A7, abcdefgh possible
            j.BottomTwoPairsGoodKicker,  //Q7
            k.BottomTwoPairsWeakKicker,  //75
            l.UnderTwoPairs,  //55, marginal
            m.OpenStraightDraw,   //98, marginal
            n.OnePairTopKicker,   //AK, didn't hit, trash
            o.OnePairGoodKicker,  //QT, didn't hit, trash
            p.OnePairWeakKicker,  //65, didn't hit, trash
             */
            if (grid.HighRank == PairCard1.Rank && grid.LowRank == PairCard2.Rank)
            {
                return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.FourSome, 2);
            }

            if (grid.HighRank == PairCard1.Rank && grid.LowRank == SingleCard.Rank)
            {
                return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.HighFullHouse, 2);
            }

            if (grid.HighRank == SingleCard.Rank && grid.LowRank == SingleCard.Rank)
            {
                return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.LowFullHouse, 2);
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
                    return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.ThreeSomeTopKicker, 1);
                }
                if (kicker > RankEnum.Ten)
                {
                    return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.ThreeSomeGoodKicker, 1);
                }
                return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.ThreeSomeWeakKicker, 1);
            }

            if (grid.Category == GridCategoryEnum.Paired && grid.HighRank > PairCard1.Rank)
            {
                return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.OverTwoPairs, 2);
            }

            if (grid.Category == GridCategoryEnum.Paired && grid.HighRank < PairCard1.Rank && grid.HighRank > SingleCard.Rank)
            {
                return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.BetweenTwoPairs, 2);
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
                    return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.BottomTwoPairsTopKicker, 1);
                }
                if (kicker > RankEnum.Ten)
                {
                    return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.BottomTwoPairsGoodKicker, 1);
                }
                return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.BottomTwoPairsWeakKicker, 1);
            }

            if (grid.Category == GridCategoryEnum.Paired && grid.HighRank < SingleCard.Rank)
            {
                return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.UnderTwoPairs, 2);
            }

            List<RankEnum> ranks = new List<RankEnum>() { PairCard1.Rank, SingleCard.Rank, grid.HighRank, grid.LowRank };

            kicker = ranks.Where(r => r != PairCard1.Rank).Max();
            if (kicker == RankEnum.Ace)
            {
                return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.OnePairTopKicker, 0);
            }
            if (kicker > RankEnum.Ten)
            {
                return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.OnePairGoodKicker, 0);
            }
            return new Tuple<HighPairOutcomeEnum, int>(HighPairOutcomeEnum.OnePairWeakKicker, 0);
        }
    }
}
