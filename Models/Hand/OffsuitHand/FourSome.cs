namespace Models.Hand.OffsuitHand
{
    public class FourSome : OffsuitHandBase
    {
        public override int HandRank => 8;

        public RankEnum Kicker { get; set; }

        public FourSome(RankEnum kicker)
        {
            Kicker = kicker;
        }

        public override int CompareToOffsuitHand(OffsuitHandBase other)
        {
            var otherFourSome = other as FourSome;
            if (otherFourSome == null)
            {
                return HandRank.CompareTo(other.HandRank);
            }

            return Kicker.CompareTo(otherFourSome.Kicker);
        }
    }
}
