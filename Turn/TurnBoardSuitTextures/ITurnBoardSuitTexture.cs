using System;
using System.Collections.Generic;
using Models;
using Models.Ranging;

namespace Turn.TurnBoardSuitTextures
{
    public interface ITurnBoardSuitTexture
    {
        Dictionary<Tuple<SuitEnum, SuitEnum>, bool> ShouldAGridFoldToBet(RangeGrid grid);
    }
}
