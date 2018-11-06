using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Ranging;

namespace Flop.FlopBoardRankTextures
{
    /*
    public class CloseStraightableRankTexture : IFlopBoardRankTexture
    {
        public Card HighCard { get; set; }
        public Card MiddleCard { get; set; }
        public Card LowCard { get; set; }
        public CloseStraightableRankTexture(FlopBoard board)
        {
            var cards = new List<Card>() { board.Flop1, board.Flop2, board.Flop3 };
            cards.Sort();
            LowCard = cards[0];
            MiddleCard = cards[1];
            HighCard = cards[2];
            if (HighCard.Rank - LowCard.Rank != 4)
            {
                throw new InvalidOperationException("");
            }
        }

        public BoardRangeGridStatusEnum TestGridAgainstFlopBoard(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid);
            Models.Utils.Log($"Testing {grid.ToString()} against CloseStrable flop, outcome is {outcome}");
            switch (outcome)
            {
                case CloseStraightableOutcomeEnum.Straight:
                    return BoardRangeGridStatusEnum.Nuts;
                case CloseStraightableOutcomeEnum.TopSet:
                case CloseStraightableOutcomeEnum.MiddleSet:
                case CloseStraightableOutcomeEnum.BottomSet:
                case CloseStraightableOutcomeEnum.TopTwoPairs:
                case CloseStraightableOutcomeEnum.TopBottomTwoPairs:
                case CloseStraightableOutcomeEnum.BottomTwoPairs:
                    return BoardRangeGridStatusEnum.Elite;
                case CloseStraightableOutcomeEnum.OverCard:
                case CloseStraightableOutcomeEnum.HighCard:
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
                case CloseStraightableOutcomeEnum.HighCard:
                case CloseStraightableOutcomeEnum.OverCard:
                    return true;
            }

            return false;
        }

        private CloseStraightableOutcomeEnum TestGridAgainstBoard(RangeGrid grid)
        {
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == HighCard.Rank)
                {
                    return CloseStraightableOutcomeEnum.TopSet;
                }
                if (grid.HighRank == MiddleCard.Rank)
                {
                    return CloseStraightableOutcomeEnum.MiddleSet;
                }
                if (grid.HighRank == LowCard.Rank)
                {
                    return CloseStraightableOutcomeEnum.BottomSet;
                }
            }

            var ranks = new List<RankEnum>() { HighCard.Rank, MiddleCard.Rank, LowCard.Rank };
            if (ranks.All(r => r != grid.HighRank))
            {
                ranks.Add(grid.HighRank);
            }
            if (ranks.All(r => r != grid.LowRank))
            {
                ranks.Add(grid.LowRank);
            }

            ranks.Sort();

            if (ranks.Count == 5 && ranks[4] - ranks[0] == 4)
            {
                return CloseStraightableOutcomeEnum.Straight;
            }

            if (grid.HighRank == HighCard.Rank)
            {
                if (grid.LowRank == MiddleCard.Rank)
                {
                    return CloseStraightableOutcomeEnum.TopTwoPairs;
                }
                if (grid.LowRank == LowCard.Rank)
                {
                    return CloseStraightableOutcomeEnum.TopBottomTwoPairs;
                }
            }

            if (grid.HighRank == MiddleCard.Rank && grid.LowRank == LowCard.Rank)
            {
                return CloseStraightableOutcomeEnum.BottomTwoPairs;
            }

            for (int i = 0; i + 3 < ranks.Count; i++)
            {
                if (ranks[i + 3] - ranks[i] == 4)
                {
                    //close draw
                    RankEnum pairedRank = RankEnum.Undefined;
                    if (grid.Category == GridCategoryEnum.Paired)
                    {
                        pairedRank = grid.HighRank;
                    }
                    if (grid.HighRank == HighCard.Rank || grid.HighRank == MiddleCard.Rank ||
                        grid.HighRank == LowCard.Rank)
                    {
                        pairedRank = grid.HighRank;
                    }
                    if (grid.LowRank == HighCard.Rank || grid.LowRank == MiddleCard.Rank ||
                        grid.LowRank == LowCard.Rank)
                    {
                        pairedRank = grid.LowRank;
                    }
                    if (pairedRank == RankEnum.Undefined)
                    {
                        return CloseStraightableOutcomeEnum.CloseStraightDraw;
                    }
                    if (pairedRank == HighCard.Rank)
                    {
                        return CloseStraightableOutcomeEnum.TopPairCloseDraw;
                    }
                    if (pairedRank == LowCard.Rank)
                    {
                        return CloseStraightableOutcomeEnum.BottomPairCloseDraw;
                    }
                    return CloseStraightableOutcomeEnum.MiddlePairCloseDraw;
                }
            }

            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == RankEnum.Ace)
                {
                    return CloseStraightableOutcomeEnum.TopOverPair;
                }

                if (grid.HighRank > HighCard.Rank)
                {
                    return CloseStraightableOutcomeEnum.OverPair;
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
                        return CloseStraightableOutcomeEnum.TopPairTopKicker;
                    }
                    if (kicker > RankEnum.Ten)
                    {
                        return CloseStraightableOutcomeEnum.TopPairGoodKicker;
                    }
                    return CloseStraightableOutcomeEnum.TopPairWeakKicker;
                }

                if (pairedGridRank == MiddleCard.Rank)
                {
                    return CloseStraightableOutcomeEnum.MiddlePair;
                }

                if (pairedGridRank == LowCard.Rank)
                {
                    return CloseStraightableOutcomeEnum.BottomPair;
                }
            }

            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank > LowCard.Rank)
                {
                    return CloseStraightableOutcomeEnum.MiddlePair;
                }
                if (grid.HighRank < LowCard.Rank)
                {
                    return CloseStraightableOutcomeEnum.UnderPair;
                }
            }

            if (grid.HighRank > HighCard.Rank)
            {
                return CloseStraightableOutcomeEnum.OverCard;
            }

            return CloseStraightableOutcomeEnum.HighCard;
        }
    }*/
}
