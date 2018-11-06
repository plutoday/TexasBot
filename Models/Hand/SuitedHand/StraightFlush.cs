namespace Models.Hand.SuitedHand
{
    public class StraightFlush : SuitedHandBase
    {
        public override int HandRank => 9;
        public RankEnum TopRank { get; set; }

        public StraightFlush(RankEnum topRank)
        {
            TopRank = topRank;
        }

        public override int CompareToSuitedHand(SuitedHandBase other)
        {
            var otherStraightFlush = other as StraightFlush;
            if (otherStraightFlush == null)
            {
                return HandRank.CompareTo(other.HandRank);
            }

            return TopRank.CompareTo(otherStraightFlush.TopRank);
        }
    }
}
