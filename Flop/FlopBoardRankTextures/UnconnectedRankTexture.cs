using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Ranging;

namespace Flop.FlopBoardRankTextures
{
    /*
    public class UnconnectedRankTexture : IFlopBoardRankTexture
    {
        public Card HighCard { get; set; }
        public Card MiddleCard { get; set; }
        public Card LowCard { get; set; }
        public UnconnectedRankTexture(FlopBoard board)
        {
            var cards = new List<Card>() { board.Flop1, board.Flop2, board.Flop3 };
            cards.Sort();
            LowCard = cards[0];
            MiddleCard = cards[1];
            HighCard = cards[2];
        }
        public BoardRangeGridStatusEnum TestGridAgainstFlopBoard(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid);
            Models.Utils.Log($"Testing {grid.ToString()} against Unconnected flop, outcome is {outcome}");
            switch (outcome)
            {
                case UnconnectedOutcomeEnum.TopSet:
                    return BoardRangeGridStatusEnum.Nuts;
                case UnconnectedOutcomeEnum.MiddleSet:
                case UnconnectedOutcomeEnum.BottomSet:
                case UnconnectedOutcomeEnum.TopTwoPairs:
                case UnconnectedOutcomeEnum.TopBottomTwoPairs:
                case UnconnectedOutcomeEnum.BottomTwoPairs:
                    return BoardRangeGridStatusEnum.Elite;
                case UnconnectedOutcomeEnum.OverCard:
                case UnconnectedOutcomeEnum.HighCard:
                    return BoardRangeGridStatusEnum.Trash;
                default:
                    return BoardRangeGridStatusEnum.Marginal;
            }
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid);
            //todo consider betSize later

            switch (outcome)
            {
                case UnconnectedOutcomeEnum.HighCard:
                case UnconnectedOutcomeEnum.OverCard:
                    return true;
            }

            return false;
        }

        private UnconnectedOutcomeEnum TestGridAgainstBoard(RangeGrid grid)
        {
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == HighCard.Rank)
                {
                    return UnconnectedOutcomeEnum.TopSet;
                }
                if (grid.HighRank == MiddleCard.Rank)
                {
                    return UnconnectedOutcomeEnum.MiddleSet;
                }
                if (grid.HighRank == LowCard.Rank)
                {
                    return UnconnectedOutcomeEnum.BottomSet;
                }
            }

            if (grid.HighRank == HighCard.Rank)
            {
                if (grid.LowRank == MiddleCard.Rank)
                {
                    return UnconnectedOutcomeEnum.TopTwoPairs;
                }
                if (grid.LowRank == LowCard.Rank)
                {
                    return UnconnectedOutcomeEnum.TopBottomTwoPairs;
                }
            }

            if (grid.HighRank == MiddleCard.Rank && grid.LowRank == LowCard.Rank)
            {
                return UnconnectedOutcomeEnum.BottomTwoPairs;
            }

            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == RankEnum.Ace)
                {
                    return UnconnectedOutcomeEnum.TopOverPair;
                }

                if (grid.HighRank > HighCard.Rank)
                {
                    return UnconnectedOutcomeEnum.OverPair;
                }
            }

            RankEnum kicker = RankEnum.Undefined;
            RankEnum pairedBoardRank = RankEnum.Undefined;

            var boardRanks = new List<RankEnum>() { HighCard.Rank, MiddleCard.Rank, LowCard.Rank };
            pairedBoardRank = boardRanks.FirstOrDefault(r => r == grid.HighRank);
            RankEnum pairedGridRank = RankEnum.Undefined;
            if (pairedBoardRank != RankEnum.Undefined)
            {
                pairedGridRank = grid.HighRank;
                kicker = grid.LowRank;
            }
            else
            {
                pairedBoardRank = boardRanks.FirstOrDefault(r => r == grid.LowRank);
                if (pairedBoardRank != RankEnum.Undefined)
                {
                    pairedGridRank = grid.LowRank;
                    kicker = grid.HighRank;
                }
            }

            if (pairedGridRank != RankEnum.Undefined && kicker != RankEnum.Undefined)
            {
                if (pairedGridRank == HighCard.Rank)
                {
                    //top pair
                    if (kicker == RankEnum.Ace)
                    {
                        return UnconnectedOutcomeEnum.TopPairTopKicker;
                    }
                    if (kicker > RankEnum.Ten)
                    {
                        return UnconnectedOutcomeEnum.TopPairGoodKicker;
                    }
                    return UnconnectedOutcomeEnum.TopPairWeakKicker;
                }

                if (pairedGridRank == MiddleCard.Rank)
                {
                    return UnconnectedOutcomeEnum.MiddlePair;
                }

                if (pairedGridRank == LowCard.Rank)
                {
                    return UnconnectedOutcomeEnum.BottomPair;
                }
            }

            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank > LowCard.Rank)
                {
                    return UnconnectedOutcomeEnum.MiddlePair;
                }
                if (grid.HighRank < LowCard.Rank)
                {
                    return UnconnectedOutcomeEnum.UnderPair;
                }
            }

            if (grid.HighRank > HighCard.Rank)
            {
                return UnconnectedOutcomeEnum.OverCard;
            }

            return UnconnectedOutcomeEnum.HighCard;
        }
    }*/
}
