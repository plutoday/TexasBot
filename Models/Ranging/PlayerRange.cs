using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Ranging
{
    public class PlayerRange
    {
        public PlayerRangeGrid[,] Grids;

        public PlayerRange()
        {
            Grids = new PlayerRangeGrid[13, 13];
            RankEnum[] ranks = new[]
            {
                RankEnum.Ace, RankEnum.King, RankEnum.Queen, RankEnum.Jack, RankEnum.Ten, RankEnum.Nine, RankEnum.Eight,
                RankEnum.Seven, RankEnum.Six, RankEnum.Five, RankEnum.Four, RankEnum.Three, RankEnum.Two
            };

            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    var playerRangeGrid = new PlayerRangeGrid(new RangeGrid(ranks[i], ranks[j], j > i));
                    Grids[i, j] = playerRangeGrid;
                }
            }
        }

        public PlayerRange Clone()
        {
            var cloned = new PlayerRange();
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    cloned.Grids[i, j].Grid = this.Grids[i, j].Grid.Clone();
                    cloned.Grids[i, j].PlayerRangeGridStatus = this.Grids[i, j].PlayerRangeGridStatus.Clone();
                }
            }

            return cloned;
        }

        public VillainPkRange CloneToPkRange()
        {
            var cloned = new VillainPkRange();
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    cloned.Grids[i, j] = Grids[i, j].ClonePkGrid();
                }
            }

            return cloned;
        }

        public PlayerRange Intersect(PlayerRange other)
        {
            var newRange = this.Clone();
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    var grid1 = Grids[i, j];
                    var grid2 = other.Grids[i, j];
                    newRange.Grids[i, j] = grid1.Intersect(grid2);
                }
            }

            return newRange;
        }

        public void Init(string[] grids)
        {
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    if (grids[i][j] == '1')
                    {
                        Grids[i, j].PlayerRangeGridStatus.RankWiseStatus = PlayerRangeGridStatusEnum.Included;
                    }
                    else if (grids[i][j] == '0' || grids[i][j] == ' ')
                    {
                        Grids[i, j].PlayerRangeGridStatus.RankWiseStatus = PlayerRangeGridStatusEnum.Excluded;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unrecognized character {grids[i][j]}");
                    }
                }
            }
        }

        public void Init(int[,] grids)
        {

            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    if (grids[i, j] == 1)
                    {
                        Grids[i, j].PlayerRangeGridStatus.RankWiseStatus = PlayerRangeGridStatusEnum.Included;
                    }
                    if (grids[i, j] == 0)
                    {
                        Grids[i, j].PlayerRangeGridStatus.RankWiseStatus = PlayerRangeGridStatusEnum.Excluded;
                    }
                }
            }
        }

        public void Init(PlayerRangeGridStatusEnum sameStatus)
        {
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    Grids[i, j].PlayerRangeGridStatus.RankWiseStatus = sameStatus;
                }
            }
        }

        public List<PlayerRangeGrid> GetAliveGrids()
        {
            return Grids.Cast<PlayerRangeGrid>().Where(rangeGrid => rangeGrid.PlayerRangeGridStatus.GridIsAlive).ToList();
        }

        public List<PlayerRangeGrid> GetAliveGridsRankWise()
        {
            return Grids.Cast<PlayerRangeGrid>()
                .Where(rangeGrid => rangeGrid.PlayerRangeGridStatus.GridIsLiveByRank).ToList();
        }

        public List<PlayerRangeGrid> GetAliveGridsSuitWise()
        {
            return Grids.Cast<PlayerRangeGrid>()
                .Where(rangeGrid => rangeGrid.PlayerRangeGridStatus.GridIsLiveBySuit).ToList();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    sb.Append(Grids[i, j].PlayerRangeGridStatus.RankWiseStatus != PlayerRangeGridStatusEnum.Excluded
                        ? 1
                        : 0);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
