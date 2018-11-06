namespace Models.Hand.SuitedHand
{
    public class RoyalFlush : SuitedHandBase
    {
        public override int HandRank => 10;
        public override int CompareToSuitedHand(SuitedHandBase other)
        {
            var otherRoyalFlush = other as RoyalFlush;
            if (otherRoyalFlush == null)
            {
                return HandRank.CompareTo(other.HandRank);
            }

            return 0;
        }
    }
}
