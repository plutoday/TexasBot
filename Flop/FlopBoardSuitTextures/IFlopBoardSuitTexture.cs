using System;
using System.Collections.Generic;
using Models;
using Models.Ranging;

namespace Flop.FlopBoardSuitTextures
{
    public interface IFlopBoardSuitTexture
    {
        Dictionary<Tuple<SuitEnum, SuitEnum>, bool> ShouldAGridFoldToBet(RangeGrid grid);
        SuitedStatus<BoardRangeGridStatusEnum> TestSuitedGrid(RangeGrid grid);
        PairedStatus<BoardRangeGridStatusEnum> TestPairedGrid(RangeGrid grid);
        OffsuitStatus<BoardRangeGridStatusEnum> TestOffsuitGrid(RangeGrid grid);
    }
}
