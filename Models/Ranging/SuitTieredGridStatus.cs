using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Ranging
{
    public class SuitTieredGridStatus<T>
    {
        public T RankWiseStatus { get; set; }

        public GridCategoryEnum Category { get; set; }

        public SuitTieredGridStatus(GridCategoryEnum category)
        {
            Category = category;
            SuitedStatus = new SuitedStatus<T>();
            PairedStatus = new PairedStatus<T>();
            OffsuitStatus = new OffsuitStatus<T>();
        }

        public void SetAllUnless(T set, Func<T, bool> unless)
        {
            switch (Category)
            {
                case GridCategoryEnum.Suited:
                    SuitedStatus.SetAllUnless(set, unless);
                    break;
                case GridCategoryEnum.Offsuit:
                    OffsuitStatus.SetAllUnless(set, unless);
                    break;
                case GridCategoryEnum.Paired:
                    PairedStatus.SetAllUnless(set, unless);
                    break;
            }
        }

        public void SetSuitUnlessForSuitedGrid(SuitEnum suit, T set, Func<T, bool> unless)
        {
            if (Category != GridCategoryEnum.Suited)
            {
                throw new InvalidOperationException();
            }

            SuitedStatus.SetSuitUnless(suit, set, unless);
        }

        public void SetFirstSuitUnlessForOffsuitGrid(SuitEnum suit, T set, Func<T, bool> unless)
        {
            if (Category != GridCategoryEnum.Offsuit)
            {
                throw new InvalidOperationException();
            }

            OffsuitStatus.SetAllUnless(set, unless);
        }

        public int Count(Func<T, bool> t)
        {
            switch (Category)
            {
                case GridCategoryEnum.Suited:
                    return SuitedStatus.Count(t);
                case GridCategoryEnum.Offsuit:
                    return OffsuitStatus.Count(t);
                case GridCategoryEnum.Paired:
                    return PairedStatus.Count(t);
                default:
                    throw new InvalidOperationException();
            }
        }

        public int GetCount(Func<T, bool> func)
        {
            switch (Category)
            {
                case GridCategoryEnum.Suited:
                    return SuitedStatus.Count(func);
                case GridCategoryEnum.Paired:
                    return PairedStatus.Count(func);
                case GridCategoryEnum.Offsuit:
                    return OffsuitStatus.Count(func);
                default:
                    throw new InvalidOperationException();
            }
        }

        public SuitedStatus<T> SuitedStatus { get; set; }
        public PairedStatus<T> PairedStatus { get; set; }
        public OffsuitStatus<T> OffsuitStatus { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"RankWiseStatus={RankWiseStatus},");
            switch (Category)
            {
                case GridCategoryEnum.Suited:
                    sb.Append(
                        $"SuitedStatus={SuitedStatus.ToString()}");
                    break;
                case GridCategoryEnum.Offsuit:
                    sb.Append(
                        $"OffsuitStatus={OffsuitStatus.ToString()}");
                    break;
                case GridCategoryEnum.Paired:
                    sb.Append($"PairedStatus={PairedStatus.ToString()}");
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return sb.ToString();
        }
    }

    public class SuitedStatus<T>
    {
        public T HeartStatus { get; set; }
        public T SpadeStatus { get; set; }
        public T DiamondStatus { get; set; }
        public T ClubStatus { get; set; }

        public int Count(Func<T, bool> t)
        {
            int count = 0;
            if (t.Invoke(HeartStatus)) count++;
            if (t.Invoke(SpadeStatus)) count++;
            if (t.Invoke(DiamondStatus)) count++;
            if (t.Invoke(ClubStatus)) count++;
            return count;
        }

        public void SetAllUnless(T set, Func<T, bool> unless)
        {
            if (!unless.Invoke(HeartStatus)) HeartStatus = set;
            if (!unless.Invoke(SpadeStatus)) SpadeStatus = set;
            if (!unless.Invoke(DiamondStatus)) DiamondStatus = set;
            if (!unless.Invoke(ClubStatus)) ClubStatus = set;
        }

        public void SetSuitUnless(SuitEnum suit, T set, Func<T, bool> unless)
        {
            switch (suit)
            {
                case SuitEnum.Heart:
                    if (!unless.Invoke(HeartStatus)) HeartStatus = set;
                    break;
                case SuitEnum.Spade:
                    if (!unless.Invoke(SpadeStatus)) SpadeStatus = set;
                    break;
                case SuitEnum.Diamond:
                    if (!unless.Invoke(DiamondStatus)) DiamondStatus = set;
                    break;
                case SuitEnum.Club:
                    if (!unless.Invoke(ClubStatus)) ClubStatus = set;
                    break;
            }
        }

        public SuitedStatus<T> Clone()
        {
            var cloned = new SuitedStatus<T>();
            cloned.HeartStatus = HeartStatus;
            cloned.SpadeStatus = SpadeStatus;
            cloned.DiamondStatus = DiamondStatus;
            cloned.ClubStatus = ClubStatus;
            return cloned;
        }

        public override string ToString()
        {
            return $"H={HeartStatus}/S={SpadeStatus}/D={DiamondStatus}/C={ClubStatus}";
        }
    }

    public class PairedStatus<T>
    {
        public T HeartSpadeStatus { get; set; }
        public T HeartDiamondStatus { get; set; }
        public T HeartClubStatus { get; set; }
        public T SpadeDiamondStatus { get; set; }
        public T SpadeClubStatus { get; set; }
        public T DiamondClubStatus { get; set; }

        public int Count(Func<T, bool> t)
        {
            int count = 0;
            if (t.Invoke(HeartSpadeStatus)) count++;
            if (t.Invoke(HeartDiamondStatus)) count++;
            if (t.Invoke(HeartClubStatus)) count++;
            if (t.Invoke(SpadeDiamondStatus)) count++;
            if (t.Invoke(SpadeClubStatus)) count++;
            if (t.Invoke(DiamondClubStatus)) count++;
            return count;
        }

        public void SetAllUnless(T set, Func<T, bool> unless)
        {
            if (!unless.Invoke(HeartSpadeStatus)) HeartSpadeStatus = set;
            if (!unless.Invoke(HeartDiamondStatus)) HeartDiamondStatus = set;
            if (!unless.Invoke(HeartClubStatus)) HeartClubStatus = set;
            if (!unless.Invoke(SpadeDiamondStatus)) SpadeDiamondStatus = set;
            if (!unless.Invoke(SpadeClubStatus)) SpadeClubStatus = set;
            if (!unless.Invoke(DiamondClubStatus)) DiamondClubStatus = set;
        }

        public PairedStatus<T> Clone()
        {
            var cloned = new PairedStatus<T>();
            cloned.HeartSpadeStatus = HeartSpadeStatus;
            cloned.HeartDiamondStatus = HeartDiamondStatus;
            cloned.HeartClubStatus = HeartClubStatus;
            cloned.SpadeDiamondStatus = SpadeDiamondStatus;
            cloned.SpadeClubStatus = SpadeClubStatus;
            cloned.DiamondClubStatus = DiamondClubStatus;

            return cloned;
        }

        public override string ToString()
        {
            return $"HS={HeartSpadeStatus}/HD={HeartDiamondStatus}/HC={HeartClubStatus}/SD={SpadeDiamondStatus}/SC={SpadeClubStatus}/DC={DiamondClubStatus}";
        }
    }

    public class OffsuitStatus<T>
    {
        public T HeartSpadeStatus { get; set; }
        public T HeartDiamondStatus { get; set; }
        public T HeartClubStatus { get; set; }
        public T SpadeDiamondStatus { get; set; }
        public T SpadeClubStatus { get; set; }
        public T DiamondClubStatus { get; set; }
        public T SpadeHeartStatus { get; set; }
        public T DiamondHeartStatus { get; set; }
        public T ClubHeartStatus { get; set; }
        public T DiamondSpadeStatus { get; set; }
        public T ClubSpadeStatus { get; set; }
        public T ClubDiamondStatus { get; set; }

        public int Count(Func<T, bool> t)
        {
            int count = 0;
            if (t.Invoke(HeartSpadeStatus)) count++;
            if (t.Invoke(HeartDiamondStatus)) count++;
            if (t.Invoke(HeartClubStatus)) count++;
            if (t.Invoke(SpadeDiamondStatus)) count++;
            if (t.Invoke(SpadeClubStatus)) count++;
            if (t.Invoke(DiamondClubStatus)) count++;

            if (t.Invoke(SpadeHeartStatus)) count++;
            if (t.Invoke(DiamondHeartStatus)) count++;
            if (t.Invoke(ClubHeartStatus)) count++;
            if (t.Invoke(DiamondSpadeStatus)) count++;
            if (t.Invoke(ClubSpadeStatus)) count++;
            if (t.Invoke(ClubDiamondStatus)) count++;

            return count;
        }

        public void SetAllUnless(T set, Func<T, bool> unless)
        {
            if (!unless.Invoke(HeartSpadeStatus)) HeartSpadeStatus = set;
            if (!unless.Invoke(HeartDiamondStatus)) HeartDiamondStatus = set;
            if (!unless.Invoke(HeartClubStatus)) HeartClubStatus = set;
            if (!unless.Invoke(SpadeDiamondStatus)) SpadeDiamondStatus = set;
            if (!unless.Invoke(SpadeClubStatus)) SpadeClubStatus = set;
            if (!unless.Invoke(DiamondClubStatus)) DiamondClubStatus = set;
            if (!unless.Invoke(SpadeHeartStatus)) SpadeHeartStatus = set;
            if (!unless.Invoke(DiamondHeartStatus)) DiamondHeartStatus = set;
            if (!unless.Invoke(ClubHeartStatus)) ClubHeartStatus = set;
            if (!unless.Invoke(DiamondSpadeStatus)) DiamondSpadeStatus = set;
            if (!unless.Invoke(ClubSpadeStatus)) ClubSpadeStatus = set;
            if (!unless.Invoke(ClubDiamondStatus)) ClubDiamondStatus = set;
        }

        public OffsuitStatus<T> Clone()
        {
            var cloned = new OffsuitStatus<T>();
            cloned.HeartSpadeStatus = HeartSpadeStatus;
            cloned.HeartDiamondStatus = HeartDiamondStatus;
            cloned.HeartClubStatus = HeartClubStatus;
            cloned.SpadeDiamondStatus = SpadeDiamondStatus;
            cloned.SpadeClubStatus = SpadeClubStatus;
            cloned.DiamondClubStatus = DiamondClubStatus;
            cloned.SpadeHeartStatus = SpadeHeartStatus;
            cloned.DiamondHeartStatus = DiamondHeartStatus;
            cloned.ClubHeartStatus = ClubHeartStatus;
            cloned.DiamondSpadeStatus = DiamondSpadeStatus;
            cloned.ClubSpadeStatus = ClubSpadeStatus;
            cloned.ClubDiamondStatus = ClubDiamondStatus;

            return cloned;
        }

        public override string ToString()
        {
            return
                $"HS={HeartSpadeStatus}/HD={HeartDiamondStatus}/HC={HeartClubStatus}/SH={SpadeHeartStatus}/SD={SpadeDiamondStatus}/SC={SpadeClubStatus}" +
                $"/DH={DiamondHeartStatus}/DS={DiamondSpadeStatus}/DC={DiamondClubStatus}/CH={ClubHeartStatus}/CS={ClubSpadeStatus}/CD={ClubDiamondStatus}";
        }
    }
}
