using Models;
using Models.Ranging;

namespace Turn.TurnBoardRankTextures
{
    public interface ITurnFlopBoardRankTexture
    {
        BoardRangeGridStatusEnum TestGridAgainstFlopBoard(RangeGrid grid);
        bool ShouldAGridFoldToBet(RangeGrid grid);
        GridHitNewRoundResultEnum HitTurn(RangeGrid grid);
    }
}
