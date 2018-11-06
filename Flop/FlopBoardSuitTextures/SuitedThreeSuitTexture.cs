using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Common;
using Models;
using Models.Ranging;

namespace Flop.FlopBoardSuitTextures
{
    public class SuitedThreeSuitTexture : SuitedTextureBase
    {
        public Card Card1 { get; set; }
        public Card Card2 { get; set; }
        public Card Card3 { get; set; }

        public SuitEnum Suit { get; set; }

        public SuitedThreeSuitTexture(FlopBoard flopBoard)
        {
            if (flopBoard.Flop1.Suit != flopBoard.Flop2.Suit || flopBoard.Flop2.Suit != flopBoard.Flop3.Suit)
            {
                throw new InvalidOperationException("Off suit");
            }
            Card1 = flopBoard.Flop1;
            Card2 = flopBoard.Flop2;
            Card3 = flopBoard.Flop3;
            Suit = Card1.Suit;
        }

        public override Dictionary<Tuple<SuitEnum, SuitEnum>, bool> ShouldAGridFoldToBet(RangeGrid grid)
        {
            var result = new Dictionary<Tuple<SuitEnum, SuitEnum>, bool>();
            
            foreach (var tuple in grid.EnumerateAllPossibleHoles())
            {
                var hole1 = tuple.Item1;
                var hole2 = tuple.Item2;

                var outcome = TestGridAgainstBoard(hole1, hole2).Item1;

                bool shouldFold = outcome == SuitTextureOutcomeEnum.FlushDraw ||
                                  outcome == SuitTextureOutcomeEnum.Nothing;

                result.Add(new Tuple<SuitEnum, SuitEnum>(hole1.Suit, hole2.Suit), shouldFold);
            }
            return result;
        }

        protected override BoardRangeGridStatusEnum TranslateOutcomeToGridStatus(SuitTextureOutcomeEnum outcome)
        {
            switch (outcome)
            {
                case SuitTextureOutcomeEnum.RoyalFlush:
                case SuitTextureOutcomeEnum.StraightFlush:
                    return BoardRangeGridStatusEnum.Nuts;
                case SuitTextureOutcomeEnum.FlushWithTopKicker:
                case SuitTextureOutcomeEnum.FlushWithGoodKicker:
                    return BoardRangeGridStatusEnum.Elite;
                case SuitTextureOutcomeEnum.FlushWithWeakKicker:
                case SuitTextureOutcomeEnum.FlushDraw:
                    return BoardRangeGridStatusEnum.Marginal;
                case SuitTextureOutcomeEnum.Nothing:
                    return BoardRangeGridStatusEnum.Trash;
                default:
                    throw new InvalidOperationException("impossible to reach here");
            }
        }

        public override Tuple<SuitTextureOutcomeEnum, int> TestGridAgainstBoard(Card hole1, Card hole2)
        {
            if (hole1.Suit == Suit && hole2.Suit == Suit)
            {
                var ranks = new List<RankEnum>() { hole1.Rank, hole2.Rank, Card1.Rank, Card2.Rank, Card3.Rank };
                var sortedOnRank = Models.Utils.SortRanks(ranks);
                if (sortedOnRank.Count < 5)
                {
                    throw new InvalidOperationException("Impossible as 5 cards are suited.");
                }
                if (sortedOnRank[0].Item1 - sortedOnRank[4].Item1 == 4)
                {
                    if (sortedOnRank[0].Item1 == RankEnum.Ace)
                    {
                        return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.RoyalFlush, 2);
                    }
                    return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.StraightFlush, 2);
                }
                RankEnum kicker = hole1.Rank > hole2.Rank ? hole1.Rank : hole2.Rank;
                switch (kicker)
                {
                    case RankEnum.Ace:
                        return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushWithTopKicker, 2);
                    case RankEnum.King:
                    case RankEnum.Queen:
                    case RankEnum.Jack:
                        return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushWithGoodKicker, 2);
                    default:
                        return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushWithWeakKicker, 2);
                }
            }
            if (hole1.Suit == Suit || hole2.Suit == Suit)
            {
                return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushDraw, 1);
            }
            return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.Nothing, 0);
        }
    }
}
