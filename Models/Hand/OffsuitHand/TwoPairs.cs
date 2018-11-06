namespace Models.Hand.OffsuitHand
{
    public class TwoPairs : OffsuitHandBase
    {
        public override int HandRank => 3;
        public RankEnum Pair1Rank { get; set; }
        public RankEnum Pair2Rank { get; set; }
        public RankEnum KickerRank { get; set; }

        public TwoPairs(RankEnum pair1Rank, RankEnum pair2Rank, RankEnum kicker)
        {
            Pair1Rank = pair1Rank;
            Pair2Rank = pair2Rank;
            KickerRank = kicker;
        }

        public override int CompareToOffsuitHand(OffsuitHandBase other)
        {
            var otherTwoPairs = other as TwoPairs;
            if (otherTwoPairs == null)
            {
                return HandRank.CompareTo(other.HandRank);
            }

            if (Pair1Rank != otherTwoPairs.Pair1Rank)
            {
                return Pair1Rank.CompareTo(otherTwoPairs.Pair1Rank);
            }

            if (Pair2Rank != otherTwoPairs.Pair2Rank)
            {
                return Pair2Rank.CompareTo(otherTwoPairs.Pair2Rank);
            }

            return KickerRank.CompareTo(otherTwoPairs.KickerRank);
        }
    }
}
