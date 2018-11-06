namespace Models.Hand.OffsuitHand
{
    public class Straight : OffsuitHandBase
    {
        public override int HandRank => 5;

        public RankEnum TopCardRank { get; set; }

        public Straight(RankEnum topCardRank)
        {
            TopCardRank = topCardRank;
        }

        public override int CompareToOffsuitHand(OffsuitHandBase other)
        {
            var otherStraight = other as Straight;
            if (otherStraight == null)
            {
                return HandRank.CompareTo(other.HandRank);
            }

            return TopCardRank.CompareTo(otherStraight.TopCardRank);
        }
    }
}
