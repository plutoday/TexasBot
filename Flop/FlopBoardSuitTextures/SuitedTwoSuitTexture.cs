using System;
using System.Collections.Generic;
using Common;
using Flop.FlopBoardRankTextures;
using Models;
using Models.Ranging;

namespace Flop.FlopBoardSuitTextures
{
    public class SuitedTwoSuitTexture : SuitedTextureBase
    {
        public Card SuitedCard1 { get; set; }
        public Card SuitedCard2 { get; set; }
        public Card OffsuitCard { get; set; }
        public SuitedTwoSuitTexture(FlopBoard flopBoard)
        {
            if (flopBoard.Flop1.Suit == flopBoard.Flop2.Suit)
            {
                SuitedCard1 = flopBoard.Flop1;
                SuitedCard2 = flopBoard.Flop2;
                OffsuitCard = flopBoard.Flop3;
            }
            else if (flopBoard.Flop1.Suit == flopBoard.Flop3.Suit)
            {
                SuitedCard1 = flopBoard.Flop1;
                SuitedCard2 = flopBoard.Flop3;
                OffsuitCard = flopBoard.Flop2;
            }
            else if (flopBoard.Flop2.Suit == flopBoard.Flop3.Suit)
            {
                SuitedCard1 = flopBoard.Flop2;
                SuitedCard2 = flopBoard.Flop3;
                OffsuitCard = flopBoard.Flop1;
            }
            else
            {
                throw new InvalidOperationException("2 suited, i offsuit");
            }
        }

        public override Dictionary<Tuple<SuitEnum, SuitEnum>, bool> ShouldAGridFoldToBet(RangeGrid grid)
        {
            var result = new Dictionary<Tuple<SuitEnum, SuitEnum>, bool>();

            foreach (var tuple in grid.EnumerateAllPossibleHoles())
            {
                var hole1 = tuple.Item1;
                var hole2 = tuple.Item2;

                var outcome = TestGridAgainstBoard(hole1, hole2);

                bool shouldFold = outcome.Item1 == SuitTextureOutcomeEnum.Nothing;

                result.Add(new Tuple<SuitEnum, SuitEnum>(hole1.Suit, hole2.Suit), shouldFold);
            }

            return result;
        }

        public override Tuple<SuitTextureOutcomeEnum, int> TestGridAgainstBoard(Card hole1, Card hole2)
        {
            if (hole1.Suit == SuitedCard1.Suit && hole2.Suit == SuitedCard1.Suit)
            {
                return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushDraw, 2);
            }
            return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.Nothing, 1);
        }

        protected override BoardRangeGridStatusEnum TranslateOutcomeToGridStatus(SuitTextureOutcomeEnum outcome)
        {
            //todo: StraightFlushDraw and RoyalFlushDraw
            switch (outcome)
            {
                case SuitTextureOutcomeEnum.FlushDraw:
                    return BoardRangeGridStatusEnum.Marginal;
                case SuitTextureOutcomeEnum.Nothing:
                    return BoardRangeGridStatusEnum.Trash;
                default:
                    throw new InvalidOperationException("impossible to reach here");
            }
        }
    }
}
