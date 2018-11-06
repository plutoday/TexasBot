using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Ranging
{
    /// <summary>
    /// 用來描述一個Grid在PlayerRange中的狀態
    /// </summary>
    public class PlayerRangeGrid
    {
        public PlayerRangeGrid(RangeGrid grid)
        {
            Grid = grid;
            PlayerRangeGridStatus = new GridStatusInPlayerRange(grid.Category);
        }

        public PlayerRangeGrid Intersect(PlayerRangeGrid other)
        {
            if (!Grid.Equals(other.Grid))
            {
                throw new InvalidOperationException();
            }

            var prGrid = new PlayerRangeGrid(Grid);
            prGrid.Grid.Card1AvaliableSuits = Grid.Card1AvaliableSuits.Intersect(other.Grid.Card1AvaliableSuits).ToList();
            prGrid.Grid.Card2AvaliableSuits = Grid.Card2AvaliableSuits.Intersect(other.Grid.Card2AvaliableSuits).ToList();
            prGrid.PlayerRangeGridStatus = PlayerRangeGridStatus.Intersect(other.PlayerRangeGridStatus);

            return prGrid;
        }

        public RangeGrid Grid { get; set; }

        public GridStatusInPlayerRange PlayerRangeGridStatus { get; set; }

        public PlayerRangePkGrid ClonePkGrid()
        {
            var pkGrid = new PlayerRangePkGrid(Grid.Clone());
            pkGrid.GridPkStatus = new GridPkStatus(Grid.Category);
            if (PlayerRangeGridStatus.RankWiseStatus == PlayerRangeGridStatusEnum.Excluded)
            {
                pkGrid.GridPkStatus.RankWiseStatus = PlayerGridPkStatusEnum.NotAvailable;
            }
            SetToPkGrid(pkGrid);

            return pkGrid;
        }

        private void SetToPkGrid(PlayerRangePkGrid pkGrid)
        {
            switch (PlayerRangeGridStatus.Category)
            {
                case GridCategoryEnum.Suited:
                    if (PlayerRangeGridStatus.SuitedStatus.HeartStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.SuitedStatus.HeartStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.SuitedStatus.SpadeStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.SuitedStatus.SpadeStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.SuitedStatus.DiamondStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.SuitedStatus.DiamondStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.SuitedStatus.ClubStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.SuitedStatus.ClubStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    break;
                case GridCategoryEnum.Paired:
                    if (PlayerRangeGridStatus.PairedStatus.HeartSpadeStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.PairedStatus.HeartSpadeStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.PairedStatus.HeartDiamondStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.PairedStatus.HeartDiamondStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.PairedStatus.HeartClubStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.PairedStatus.HeartClubStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.PairedStatus.SpadeDiamondStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.PairedStatus.SpadeDiamondStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.PairedStatus.SpadeClubStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.PairedStatus.SpadeClubStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.PairedStatus.DiamondClubStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.PairedStatus.DiamondClubStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    break;
                case GridCategoryEnum.Offsuit:
                    if (PlayerRangeGridStatus.OffsuitStatus.HeartSpadeStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.OffsuitStatus.HeartSpadeStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.OffsuitStatus.HeartDiamondStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.OffsuitStatus.HeartDiamondStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.OffsuitStatus.HeartClubStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.OffsuitStatus.HeartClubStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.OffsuitStatus.SpadeHeartStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.OffsuitStatus.SpadeHeartStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.OffsuitStatus.SpadeDiamondStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.OffsuitStatus.SpadeDiamondStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.OffsuitStatus.SpadeClubStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.OffsuitStatus.SpadeClubStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.OffsuitStatus.DiamondHeartStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.OffsuitStatus.DiamondHeartStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.OffsuitStatus.DiamondSpadeStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.OffsuitStatus.DiamondSpadeStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.OffsuitStatus.DiamondClubStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.OffsuitStatus.DiamondClubStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.OffsuitStatus.ClubHeartStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.OffsuitStatus.ClubHeartStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.OffsuitStatus.ClubSpadeStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.OffsuitStatus.ClubSpadeStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    if (PlayerRangeGridStatus.OffsuitStatus.ClubDiamondStatus == PlayerRangeGridStatusEnum.Excluded)
                    {
                        pkGrid.GridPkStatus.OffsuitStatus.ClubDiamondStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    break;
            }
        }
    }
}
