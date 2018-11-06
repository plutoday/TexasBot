using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Ranging;

namespace Flop.FlopBoardRankTextures
{
    /*
    public class ConnectorRankTexture : IFlopBoardRankTexture
    {
        public Card HighCard { get; set; }
        public Card MiddleCard { get; set; }
        public Card LowCard { get; set; }
        public ConnectorRankTexture(FlopBoard board)
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
            Models.Utils.Log($"Testing {grid.ToString()} against Connector flop, outcome is {outcome}");
            switch (outcome)
            {
                case ConnectorsOutcomeEnum.TopSet:
                    return BoardRangeGridStatusEnum.Nuts;
                case ConnectorsOutcomeEnum.MiddleSet:
                case ConnectorsOutcomeEnum.BottomSet:
                case ConnectorsOutcomeEnum.TopTwoPairs:
                case ConnectorsOutcomeEnum.TopBottomTwoPairs:
                case ConnectorsOutcomeEnum.BottomTwoPairs:
                    return BoardRangeGridStatusEnum.Elite;
                case ConnectorsOutcomeEnum.OverCard:
                case ConnectorsOutcomeEnum.HighCard:
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
                case ConnectorsOutcomeEnum.HighCard:
                case ConnectorsOutcomeEnum.OverCard:
                    return true;
            }

            return false;
        }

        private ConnectorsOutcomeEnum TestGridAgainstBoard(RangeGrid grid)
        {
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == HighCard.Rank)
                {
                    return ConnectorsOutcomeEnum.TopSet;
                }
                if (grid.HighRank == MiddleCard.Rank)
                {
                    return ConnectorsOutcomeEnum.MiddleSet;
                }
                if (grid.HighRank == LowCard.Rank)
                {
                    return ConnectorsOutcomeEnum.BottomSet;
                }
            }

            if (grid.HighRank == HighCard.Rank)
            {
                if (grid.LowRank == MiddleCard.Rank)
                {
                    return ConnectorsOutcomeEnum.TopTwoPairs;
                }
                if (grid.LowRank == LowCard.Rank)
                {
                    return ConnectorsOutcomeEnum.TopBottomTwoPairs;
                }
            }

            if (grid.HighRank == MiddleCard.Rank && grid.LowRank == LowCard.Rank)
            {
                return ConnectorsOutcomeEnum.BottomTwoPairs;
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

            for (int i = 0; i + 3 < ranks.Count; i++)
            {
                if (ranks[i + 3] - ranks[i] == 3)
                {
                    //open draw
                    return ConnectorsOutcomeEnum.OpenDraw;
                }
            }

            for (int i = 0; i + 3 < ranks.Count; i++)
            {
                if (ranks[i + 3] - ranks[i] == 4)
                {
                    //close draw
                    return ConnectorsOutcomeEnum.CloseDraw;
                }
            }

            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank == RankEnum.Ace)
                {
                    return ConnectorsOutcomeEnum.TopOverPair;
                }

                if (grid.HighRank > HighCard.Rank)
                {
                    return ConnectorsOutcomeEnum.OverPair;
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
                        return ConnectorsOutcomeEnum.TopPairTopKicker;
                    }
                    if (kicker > RankEnum.Ten)
                    {
                        return ConnectorsOutcomeEnum.TopPairGoodKicker;
                    }
                    return ConnectorsOutcomeEnum.TopPairWeakKicker;
                }

                if (pairedGridRank == MiddleCard.Rank)
                {
                    return ConnectorsOutcomeEnum.MiddlePair;
                }

                if (pairedGridRank == LowCard.Rank)
                {
                    return ConnectorsOutcomeEnum.BottomPair;
                }
            }

            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank > LowCard.Rank)
                {
                    return ConnectorsOutcomeEnum.MiddlePair;
                }
                if (grid.HighRank < LowCard.Rank)
                {
                    return ConnectorsOutcomeEnum.UnderPair;
                }
            }

            if (grid.HighRank > HighCard.Rank)
            {
                return ConnectorsOutcomeEnum.OverCard;
            }

            return ConnectorsOutcomeEnum.HighCard;
        }
    }*/
}
