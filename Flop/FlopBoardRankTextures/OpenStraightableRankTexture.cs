using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Ranging;

namespace Flop.FlopBoardRankTextures
{
    /*
    public class OpenStraightableRankTexture : IFlopBoardRankTexture
    {
        public Card HighCard { get; set; }
        public Card MiddleCard { get; set; }
        public Card LowCard { get; set; }
        public OpenStraightableRankTexture(FlopBoard board)
        {
            var cards = new List<Card>() { board.Flop1, board.Flop2, board.Flop3 };
            cards.Sort();
            LowCard = cards[0];
            MiddleCard = cards[1];
            HighCard = cards[2];
            if (HighCard.Rank - LowCard.Rank > 3 || HighCard.Rank - LowCard.Rank < 2)
            {
                throw new InvalidOperationException("");
            }
        }

        public BoardRangeGridStatusEnum TestGridAgainstFlopBoard(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid);
            Models.Utils.Log($"Testing {grid.ToString()} against OpenStrable flop, outcome is {outcome}");
            switch (outcome)
            {
                case OpenStraightableOutcomeEnum.HighStraight:
                    return BoardRangeGridStatusEnum.Nuts;
                case OpenStraightableOutcomeEnum.MiddleStraight:
                case OpenStraightableOutcomeEnum.LowStraight:
                case OpenStraightableOutcomeEnum.TopSet:
                case OpenStraightableOutcomeEnum.MiddleSet:
                case OpenStraightableOutcomeEnum.BottomSet:
                case OpenStraightableOutcomeEnum.TopTwoPairs:
                case OpenStraightableOutcomeEnum.TopBottomTwoPairs:
                case OpenStraightableOutcomeEnum.BottomTwoPairs:
                    return BoardRangeGridStatusEnum.Elite;
                case OpenStraightableOutcomeEnum.OverCard:
                case OpenStraightableOutcomeEnum.HighCard:
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
                case OpenStraightableOutcomeEnum.HighCard:
                case OpenStraightableOutcomeEnum.OverCard:
                    return true;
            }

            return false;
        }

        private OpenStraightableOutcomeEnum TestGridAgainstBoard(RangeGrid grid)
        {
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == HighCard.Rank)
                {
                    return OpenStraightableOutcomeEnum.TopSet;
                }
                if (grid.HighRank == MiddleCard.Rank)
                {
                    return OpenStraightableOutcomeEnum.MiddleSet;
                }
                if (grid.HighRank == LowCard.Rank)
                {
                    return OpenStraightableOutcomeEnum.BottomSet;
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
                //Straight
                if (ranks[0] == LowCard.Rank)
                {
                    return OpenStraightableOutcomeEnum.HighStraight;
                }
                if (ranks[0] == grid.LowRank && ranks[1] == grid.HighRank)
                {
                    return OpenStraightableOutcomeEnum.LowStraight;
                }
                return OpenStraightableOutcomeEnum.MiddleStraight;
            }

            if (grid.HighRank == HighCard.Rank)
            {
                if (grid.LowRank == MiddleCard.Rank)
                {
                    return OpenStraightableOutcomeEnum.TopTwoPairs;
                }
                if (grid.LowRank == LowCard.Rank)
                {
                    return OpenStraightableOutcomeEnum.TopBottomTwoPairs;
                }
            }

            if (grid.HighRank == MiddleCard.Rank && grid.LowRank == LowCard.Rank)
            {
                return OpenStraightableOutcomeEnum.BottomTwoPairs;
            }

            for (int i = 0; i + 3 < ranks.Count; i++)
            {
                if (ranks[i + 3] - ranks[i] == 3)
                {
                    //open draw
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
                        return OpenStraightableOutcomeEnum.OpenStraightDraw;
                    }

                    if (pairedRank > HighCard.Rank)
                    {
                        return OpenStraightableOutcomeEnum.OverPairOpenDraw;
                    }
                    if (pairedRank == HighCard.Rank)
                    {
                        return OpenStraightableOutcomeEnum.TopPairOpenDraw;
                    }
                    if (pairedRank < LowCard.Rank)
                    {
                        return OpenStraightableOutcomeEnum.UnderPairOpenDraw;
                    }
                    if (pairedRank == LowCard.Rank)
                    {
                        return OpenStraightableOutcomeEnum.BottomPairOpenDraw;
                    }
                    return OpenStraightableOutcomeEnum.MiddlePairOpenDraw;
                }
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
                        return OpenStraightableOutcomeEnum.CloseStraightDraw;
                    }

                    if (pairedRank > HighCard.Rank)
                    {
                        return OpenStraightableOutcomeEnum.OverPairCloseDraw;
                    }
                    if (pairedRank == HighCard.Rank)
                    {
                        return OpenStraightableOutcomeEnum.TopPairCloseDraw;
                    }
                    if (pairedRank < LowCard.Rank)
                    {
                        return OpenStraightableOutcomeEnum.UnderPairCloseDraw;
                    }
                    if (pairedRank == LowCard.Rank)
                    {
                        return OpenStraightableOutcomeEnum.BottomPairCloseDraw;
                    }
                    return OpenStraightableOutcomeEnum.MiddlePairCloseDraw;
                }
            }

            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == RankEnum.Ace)
                {
                    return OpenStraightableOutcomeEnum.TopOverPair;
                }

                if (grid.HighRank > HighCard.Rank)
                {
                    return OpenStraightableOutcomeEnum.OverPair;
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
                        return OpenStraightableOutcomeEnum.TopPairTopKicker;
                    }
                    if (kicker > RankEnum.Ten)
                    {
                        return OpenStraightableOutcomeEnum.TopPairGoodKicker;
                    }
                    return OpenStraightableOutcomeEnum.TopPairWeakKicker;
                }

                if (pairedGridRank == MiddleCard.Rank)
                {
                    return OpenStraightableOutcomeEnum.MiddlePair;
                }

                if (pairedGridRank == LowCard.Rank)
                {
                    return OpenStraightableOutcomeEnum.BottomPair;
                }
            }

            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank > LowCard.Rank)
                {
                    throw new InvalidOperationException("Impossible for open straightable texture");
                }
                if (grid.HighRank < LowCard.Rank)
                {
                    return OpenStraightableOutcomeEnum.UnderPair;
                }
            }

            if (grid.HighRank > HighCard.Rank)
            {
                return OpenStraightableOutcomeEnum.OverCard;
            }

            return OpenStraightableOutcomeEnum.HighCard;
        }
    }*/
}
