using System;

namespace Models.Ranging
{
    /// <summary>
    /// 用来描述一个Grid在和Hero在某个board上PK的结果
    /// Fold?
    /// CallAndWin?
    /// CallAndLose?
    /// </summary>
    public class PlayerRangePkGrid
    {
        public RangeGrid Grid { get; set; }

        public GridPkStatus GridPkStatus { get; set; }

        public PlayerRangePkGrid(RangeGrid grid)
        {
            Grid = grid;
            GridPkStatus = new GridPkStatus(grid.Category);
        }

        public int AvailableCombCount => Math.Max(Grid.AvailableRankCombCount, AvailableSuitCombCount);

        private int AvailableSuitCombCount => GridPkStatus.GetCount(e => e != PlayerGridPkStatusEnum.NotAvailable);

        //If rank should fold, go calculate the suit folds, otherwise, no folding at all, since the rank allow can already stand.
        public int FoldCombCount => GridPkStatus.RankWiseStatus == PlayerGridPkStatusEnum.Fold ? GridPkStatus.GetCount(e => e == PlayerGridPkStatusEnum.Fold) : 0;
    }
}
