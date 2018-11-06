using System.Collections.Generic;
using System.Linq;

namespace Models.Ranging
{
    public class VillainPkRange
    {
        public PlayerRangePkGrid[,] Grids;

        public VillainPkRange()
        {
            Grids = new PlayerRangePkGrid[13, 13];
            RankEnum[] ranks = new[]
            {
                RankEnum.Ace, RankEnum.King, RankEnum.Queen, RankEnum.Jack, RankEnum.Ten, RankEnum.Nine, RankEnum.Eight,
                RankEnum.Seven, RankEnum.Six, RankEnum.Five, RankEnum.Four, RankEnum.Three, RankEnum.Two
            };

            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    var playerRangeGrid = new PlayerRangePkGrid(new RangeGrid(ranks[i], ranks[j], j > i));
                    Grids[i, j] = playerRangeGrid;
                }
            }
        }

        public IEnumerable<PlayerRangePkGrid> GetAliveGrids()
        {
            foreach (var playerRangePkGrid in Grids)
            {
                if (playerRangePkGrid.GridPkStatus.GridIsAlive)
                {
                    yield return playerRangePkGrid;
                }
            }
        }
    }
}
