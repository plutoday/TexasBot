using System;
using System.Collections.Generic;
using Models;
using Models.Ranging;

namespace Turn.TurnBoardSuitTextures
{
    public class OffsuitTexture : ITurnBoardSuitTexture
    {
        public OffsuitTexture(TurnBoard turnBoard)
        {
        }

        public Dictionary<Tuple<SuitEnum, SuitEnum>, bool> ShouldAGridFoldToBet(RangeGrid grid)
        {
            var suits = new List<SuitEnum>() {SuitEnum.Heart, SuitEnum.Spade, SuitEnum.Diamond, SuitEnum.Club};
            var result = new Dictionary<Tuple<SuitEnum, SuitEnum>, bool>();
            foreach (var suit1 in suits)
            {
                foreach (var suit2 in suits)
                {
                    result.Add(new Tuple<SuitEnum,SuitEnum>(suit1, suit2), true);
                }
            }

            return result;
        }

        public Tuple<SuitTextureOutcomeEnum, int> TestGridAgainstTurnBoard(Card hole1, Card hole2)
        {
            return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.Nothing, 0);
        }
    }
}
