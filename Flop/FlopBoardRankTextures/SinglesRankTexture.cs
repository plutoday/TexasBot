using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Models;
using Models.Ranging;

namespace Flop.FlopBoardRankTextures
{
    public class SinglesRankTexture : IFlopBoardRankTexture
    {
        public Card HighCard { get; set; }
        public Card MiddleCard { get; set; }
        public Card LowCard { get; set; }

        public SinglesRankTexture(FlopBoard board)
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
            switch (outcome.Item1)
            {
                case SinglesOutcomeEnum.TwoUsedStraightTwoOver:
                    return BoardRangeGridStatusEnum.Nuts;
                case SinglesOutcomeEnum.TwoUsedStraightOneOver:
                    return HighCard.Rank == LowCard.Rank + 3
                        ? BoardRangeGridStatusEnum.Nuts
                        : BoardRangeGridStatusEnum.Elite;
                case SinglesOutcomeEnum.TwoUsedStraightNoneOver:
                    return HighCard.Rank == LowCard.Rank + 4
                        ? BoardRangeGridStatusEnum.Nuts
                        : BoardRangeGridStatusEnum.Elite;
                case SinglesOutcomeEnum.HighSet:
                case SinglesOutcomeEnum.MiddleSet:
                case SinglesOutcomeEnum.LowSet:
                case SinglesOutcomeEnum.TopTwoPairs:
                case SinglesOutcomeEnum.TopBottomTwoPairs:
                case SinglesOutcomeEnum.BottomTwoPairs:
                    return BoardRangeGridStatusEnum.Elite;
                case SinglesOutcomeEnum.OverPair:
                    return BoardRangeGridStatusEnum.Good;
                case SinglesOutcomeEnum.TopHighCard:
                case SinglesOutcomeEnum.OverHighCard:
                case SinglesOutcomeEnum.HighCard:
                    return ShouldRankDrawFold(grid) ? BoardRangeGridStatusEnum.Trash : BoardRangeGridStatusEnum.Marginal;
                default:
                    return BoardRangeGridStatusEnum.Marginal;
            }
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;

            if (outcome.Grade() == RankHandGradeEnum.HighCard)
            {
                return ShouldRankDrawFold(grid);
            }

            return false;
        }

        private bool ShouldRankDrawFold(RangeGrid grid)
        {
            var rankDraw =
                grid.TestRankDrawOnFlop(new List<RankEnum>() { HighCard.Rank, MiddleCard.Rank, LowCard.Rank });
            switch (rankDraw)
            {
                case RankDrawEnum.Nothing:
                    return true;
                case RankDrawEnum.OneUsedCloseDrawNoneOver:
                case RankDrawEnum.OneUsedCloseDrawOneOver:
                default:
                    return false;
            }
        }

        /// <summary>
        /// int means how many cards in holes are used to form the hand
        /// kicker not counted
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public Tuple<SinglesOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            /*
        TwoUsedStraightTwoOver,
        TwoUsedStraightOneOver,
        TwoUsedStraightNoneOver,

        HighSet,
        MiddleSet,
        LowSet,
        TopTwoPairs,
        TopBottomTwoPairs,
        BottomTwoPairs,

        TwoUsedOpenDrawTwoOver,
        TwoUsedOpenDrawOneOver,
        TwoUsedOpenDrawNoneOver,
        OneUsedOpenDrawOneOver,
        OneUsedOpenDrawNoneOver,

        OverPair,
        TopPair,
        MiddlePair,
        LowPair,
        UnderPair,

        TwoUsedCloseDrawTwoOver,
        TwoUsedCloseDrawOneOver,
        TwoUsedCloseDrawNoneOver,
        OneUsedCloseDrawOneOver,
        OneUsedCloseDrawNoneOver,
             */

            var ranks = new List<RankEnum>();
            ranks.Add(grid.HighRank);
            if (ranks.All(r => r != grid.LowRank)) ranks.Add(grid.LowRank);
            if (ranks.All(r => r != HighCard.Rank)) ranks.Add(HighCard.Rank);
            if (ranks.All(r => r != MiddleCard.Rank)) ranks.Add(MiddleCard.Rank);
            if (ranks.All(r => r != LowCard.Rank)) ranks.Add(LowCard.Rank);

            ranks.Sort();

            if (ranks.Count == 5 && ranks[0] + 4 == ranks[4])
            {
                //straight
                if (ranks[4] == HighCard.Rank + 2) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.TwoUsedStraightTwoOver, 2);
                if (ranks[4] == HighCard.Rank + 1) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.TwoUsedStraightOneOver, 2);
                if (ranks[4] == HighCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.TwoUsedStraightNoneOver, 2);

                throw new InvalidOperationException("Impossible to reach here");
            }

            if (grid.Category == GridCategoryEnum.Paired)
            {
                //could be set
                if (grid.HighRank == HighCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.HighSet, 2);
                if (grid.HighRank == MiddleCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.MiddleSet, 2);
                if (grid.HighRank == LowCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.LowSet, 2);
            }

            if (grid.HighRank == HighCard.Rank && grid.LowRank == MiddleCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.TopTwoPairs, 2);
            if (grid.HighRank == HighCard.Rank && grid.LowRank == LowCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.TopBottomTwoPairs, 2);
            if (grid.HighRank == MiddleCard.Rank && grid.LowRank == LowCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.BottomTwoPairs, 2);

            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank > HighCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.OverPair, 2);
                if (grid.HighRank < LowCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.UnderPair, 2);
                if (grid.HighRank > MiddleCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.OverMiddlePair, 2);
                return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.UnderMiddlePair, 2);
            }

            if (grid.HighRank == HighCard.Rank || grid.LowRank == HighCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.TopPair, 1);
            if (grid.HighRank == MiddleCard.Rank || grid.LowRank == MiddleCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.MiddlePair, 1);
            if (grid.HighRank == LowCard.Rank || grid.LowRank == LowCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.LowPair, 1);

            //high card

            if (grid.HighRank == RankEnum.Ace) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.TopHighCard, 1);
            if (grid.HighRank > HighCard.Rank) return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.OverHighCard, 1);
            return new Tuple<SinglesOutcomeEnum, int>(SinglesOutcomeEnum.HighCard, 0);
        }
    }
}
