using System;
using Infra;
using Models;
using Models.Ranging;

namespace Flop.FlopBoardRankTextures
{
    public class ThreeSomeRankTexture : IFlopBoardRankTexture
    {
        public Card ThreesomeCard1 { get; set; }
        public Card ThreesomeCard2 { get; set; }
        public Card ThreesomeCard3 { get; set; }
        public ThreeSomeRankTexture(FlopBoard flopBoard)
        {
            if (flopBoard.Flop1.Rank != flopBoard.Flop2.Rank || flopBoard.Flop2.Rank != flopBoard.Flop3.Rank)
            {
                throw new InvalidOperationException();
            }

            ThreesomeCard1 = flopBoard.Flop1;
            ThreesomeCard2 = flopBoard.Flop2;
            ThreesomeCard3 = flopBoard.Flop3;
        }

        public BoardRangeGridStatusEnum TestGridAgainstFlopBoard(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            BoardRangeGridStatusEnum result;
            switch (outcome)
            {
                case ThreesomeOutcomeEnum.FourSome:
                    result = BoardRangeGridStatusEnum.Nuts;
                    break;
                case ThreesomeOutcomeEnum.TopFullHouse:
                case ThreesomeOutcomeEnum.GoodFullHouse:
                    result = BoardRangeGridStatusEnum.Elite;
                    break;
                case ThreesomeOutcomeEnum.WeakFullHouse:
                    result = BoardRangeGridStatusEnum.Good;
                    break;
                case ThreesomeOutcomeEnum.ThreeSomeGoodKicker:
                case ThreesomeOutcomeEnum.ThreeSomeWeakKicker:
                    result = BoardRangeGridStatusEnum.Trash;
                    break;
                default:
                    result = BoardRangeGridStatusEnum.Marginal;
                    break;
            }

            Logger.Instance.Log($"{grid.ToString()} tested on Threesome flop, outcome:{outcome}|result:{result}");
            return result;
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            //todo consider betSize later

            switch (outcome)
            {
                case ThreesomeOutcomeEnum.ThreeSomeGoodKicker:
                case ThreesomeOutcomeEnum.ThreeSomeWeakKicker:
                    return true;
            }

            return false;
        }

        public Tuple<ThreesomeOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            /*
        FourSomeTopKicker, //AJ
        FourSomeGoodKicker,    //QJ
        FourSomeWeakKicker,    //J7
        TopFullHouse,   //AA
        GoodFullHouse,  //QQ
        WeakFullHouse, //88
        ThreeSomeTopKicker,  //AQ
        ThreeSomeGoodKicker,    //QT
        ThreeSomeWeakKicker,    //98
       */

            if (grid.HighRank == ThreesomeCard1.Rank || grid.LowRank == ThreesomeCard1.Rank)
            {
                return new Tuple<ThreesomeOutcomeEnum, int>(ThreesomeOutcomeEnum.FourSome, 1);
            }

            if (grid.HighRank == grid.LowRank)
            {
                //full house
                if (grid.HighRank == RankEnum.Ace)
                {
                    return new Tuple<ThreesomeOutcomeEnum, int>(ThreesomeOutcomeEnum.TopFullHouse, 2);
                }
                if (grid.HighRank > ThreesomeCard1.Rank)
                {
                    return new Tuple<ThreesomeOutcomeEnum, int>(ThreesomeOutcomeEnum.GoodFullHouse, 2);
                }
                return new Tuple<ThreesomeOutcomeEnum, int>(ThreesomeOutcomeEnum.WeakFullHouse, 2);
            }

            if (grid.HighRank == RankEnum.Ace)
            {
                return new Tuple<ThreesomeOutcomeEnum, int>(ThreesomeOutcomeEnum.ThreeSomeTopKicker, 0);
            }
            if (grid.HighRank > RankEnum.Ten)
            {
                return new Tuple<ThreesomeOutcomeEnum, int>(ThreesomeOutcomeEnum.ThreeSomeGoodKicker, 0);
            }

            return new Tuple<ThreesomeOutcomeEnum, int>(ThreesomeOutcomeEnum.ThreeSomeWeakKicker, 0);
        }
    }
}
