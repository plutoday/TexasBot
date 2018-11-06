using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Ranging
{
    /// <summary>
    /// 配合GridRange使用，用來描述該GridRange在一個PlayerRange中的狀態
    /// </summary>
    public class GridStatusInPlayerRange : SuitTieredGridStatus<PlayerRangeGridStatusEnum>
    {
        public GridStatusInPlayerRange(GridCategoryEnum category) : base(category)
        {
            RankWiseStatus = PlayerRangeGridStatusEnum.Included;
        }

        public GridStatusInPlayerRange Intersect(GridStatusInPlayerRange other)
        {
            var gs = new GridStatusInPlayerRange(Category);
            gs.RankWiseStatus = RankWiseStatus.PickWorse(other.RankWiseStatus);
            switch (Category)
            {
                case GridCategoryEnum.Suited:
                    gs.SuitedStatus.HeartStatus = SuitedStatus.HeartStatus.PickWorse(other.SuitedStatus.HeartStatus);
                    gs.SuitedStatus.SpadeStatus = SuitedStatus.SpadeStatus.PickWorse(other.SuitedStatus.SpadeStatus);
                    gs.SuitedStatus.DiamondStatus =
                        SuitedStatus.DiamondStatus.PickWorse(other.SuitedStatus.DiamondStatus);
                    gs.SuitedStatus.ClubStatus = SuitedStatus.ClubStatus.PickWorse(other.SuitedStatus.ClubStatus);
                    break;
                case GridCategoryEnum.Paired:
                    gs.PairedStatus.HeartSpadeStatus =
                        PairedStatus.HeartSpadeStatus.PickWorse(other.PairedStatus.HeartSpadeStatus);
                    gs.PairedStatus.HeartDiamondStatus =
                        PairedStatus.HeartDiamondStatus.PickWorse(other.PairedStatus.HeartDiamondStatus);
                    gs.PairedStatus.HeartClubStatus =
                        PairedStatus.HeartClubStatus.PickWorse(other.PairedStatus.HeartClubStatus);
                    gs.PairedStatus.SpadeDiamondStatus =
                        PairedStatus.SpadeDiamondStatus.PickWorse(other.PairedStatus.SpadeDiamondStatus);
                    gs.PairedStatus.SpadeClubStatus =
                        PairedStatus.SpadeClubStatus.PickWorse(other.PairedStatus.SpadeClubStatus);
                    gs.PairedStatus.DiamondClubStatus =
                        PairedStatus.DiamondClubStatus.PickWorse(other.PairedStatus.DiamondClubStatus);
                    break;
                case GridCategoryEnum.Offsuit:
                    gs.OffsuitStatus.HeartSpadeStatus =
                        OffsuitStatus.HeartSpadeStatus.PickWorse(other.OffsuitStatus.HeartSpadeStatus);
                    gs.OffsuitStatus.HeartDiamondStatus =
                        OffsuitStatus.HeartDiamondStatus.PickWorse(other.OffsuitStatus.HeartDiamondStatus);
                    gs.OffsuitStatus.HeartClubStatus =
                        OffsuitStatus.HeartClubStatus.PickWorse(other.OffsuitStatus.HeartClubStatus);
                    gs.OffsuitStatus.SpadeDiamondStatus =
                        OffsuitStatus.SpadeDiamondStatus.PickWorse(other.OffsuitStatus.SpadeDiamondStatus);
                    gs.OffsuitStatus.SpadeClubStatus =
                        OffsuitStatus.SpadeClubStatus.PickWorse(other.OffsuitStatus.SpadeClubStatus);
                    gs.OffsuitStatus.DiamondClubStatus =
                        OffsuitStatus.DiamondClubStatus.PickWorse(other.OffsuitStatus.DiamondClubStatus);

                    gs.OffsuitStatus.SpadeHeartStatus =
                        OffsuitStatus.SpadeHeartStatus.PickWorse(other.OffsuitStatus.SpadeHeartStatus);
                    gs.OffsuitStatus.DiamondHeartStatus =
                        OffsuitStatus.DiamondHeartStatus.PickWorse(other.OffsuitStatus.DiamondHeartStatus);
                    gs.OffsuitStatus.ClubHeartStatus =
                        OffsuitStatus.ClubHeartStatus.PickWorse(other.OffsuitStatus.ClubHeartStatus);
                    gs.OffsuitStatus.DiamondSpadeStatus =
                        OffsuitStatus.DiamondSpadeStatus.PickWorse(other.OffsuitStatus.DiamondSpadeStatus);
                    gs.OffsuitStatus.ClubSpadeStatus =
                        OffsuitStatus.ClubSpadeStatus.PickWorse(other.OffsuitStatus.ClubSpadeStatus);
                    gs.OffsuitStatus.ClubDiamondStatus =
                        OffsuitStatus.ClubDiamondStatus.PickWorse(other.OffsuitStatus.ClubDiamondStatus);
                    break;
            }

            return gs;
        }

        public bool GridIsAlive => GridIsLiveByRank || GridIsLiveBySuit;

        public bool GridIsLiveByRank => (RankWiseStatus != PlayerRangeGridStatusEnum.Excluded);

        public bool GridIsLiveBySuit
        {
            get
            {
                switch (Category)
                {
                    case GridCategoryEnum.Suited:
                        if (SuitedStatus.HeartStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (SuitedStatus.HeartStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (SuitedStatus.HeartStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (SuitedStatus.HeartStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        return false;
                    case GridCategoryEnum.Paired:
                        if (PairedStatus.HeartSpadeStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (PairedStatus.HeartDiamondStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (PairedStatus.HeartClubStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (PairedStatus.SpadeDiamondStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (PairedStatus.SpadeClubStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (PairedStatus.DiamondClubStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        return false;
                    case GridCategoryEnum.Offsuit:
                        if (OffsuitStatus.HeartSpadeStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (OffsuitStatus.HeartDiamondStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (OffsuitStatus.HeartClubStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (OffsuitStatus.SpadeDiamondStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (OffsuitStatus.SpadeClubStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (OffsuitStatus.DiamondClubStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (OffsuitStatus.SpadeHeartStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (OffsuitStatus.DiamondHeartStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (OffsuitStatus.ClubHeartStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (OffsuitStatus.DiamondSpadeStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (OffsuitStatus.ClubSpadeStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        if (OffsuitStatus.ClubDiamondStatus != PlayerRangeGridStatusEnum.Excluded) return true;
                        return false;
                    default:
                        throw new InvalidOperationException();
                }

            }
        }

        public GridStatusInPlayerRange Clone()
        {
            var cloned = new GridStatusInPlayerRange(Category);
            cloned.RankWiseStatus = this.RankWiseStatus;
            switch (Category)
            {
                case GridCategoryEnum.Suited:
                    cloned.SuitedStatus = SuitedStatus.Clone();
                    break;
                case GridCategoryEnum.Paired:
                    cloned.PairedStatus = PairedStatus.Clone();
                    break;
                case GridCategoryEnum.Offsuit:
                    cloned.OffsuitStatus = OffsuitStatus.Clone();
                    break;
            }

            return cloned;
        }
    }
}
