using System;

namespace Models.Ranging
{
    public class GridPkStatus : SuitTieredGridStatus<PlayerGridPkStatusEnum>
    {
        public GridPkStatus(GridCategoryEnum category) : base(category)
        {
        }

        public bool GridIsAlive => GridIsLiveByRank || GridIsLiveBySuit;

        public bool GridIsLiveByRank => (RankWiseStatus != PlayerGridPkStatusEnum.NotAvailable);

        public bool GridIsLiveBySuit
        {
            get
            {
                switch (Category)
                {
                    case GridCategoryEnum.Suited:
                        if (SuitedStatus.HeartStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (SuitedStatus.HeartStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (SuitedStatus.HeartStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (SuitedStatus.HeartStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        return false;
                    case GridCategoryEnum.Paired:
                        if (PairedStatus.HeartSpadeStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (PairedStatus.HeartDiamondStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (PairedStatus.HeartClubStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (PairedStatus.SpadeDiamondStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (PairedStatus.SpadeClubStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (PairedStatus.DiamondClubStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        return false;
                    case GridCategoryEnum.Offsuit:
                        if (OffsuitStatus.HeartSpadeStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (OffsuitStatus.HeartDiamondStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (OffsuitStatus.HeartClubStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (OffsuitStatus.SpadeDiamondStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (OffsuitStatus.SpadeClubStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (OffsuitStatus.DiamondClubStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (OffsuitStatus.SpadeHeartStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (OffsuitStatus.DiamondHeartStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (OffsuitStatus.ClubHeartStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (OffsuitStatus.DiamondSpadeStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (OffsuitStatus.ClubSpadeStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        if (OffsuitStatus.ClubDiamondStatus != PlayerGridPkStatusEnum.NotAvailable) return true;
                        return false;
                    default:
                        throw new InvalidOperationException();
                }

            }
        }
    }
}
