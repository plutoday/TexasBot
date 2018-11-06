namespace Models.Hand.OffsuitHand
{
    public class OnePair : OffsuitHandBase
    {
        public RankEnum PairRank { get; set; }
        public RankEnum Kicker1 { get; set; }
        public RankEnum Kicker2 { get; set; }
        public RankEnum Kicker3 { get; set; }

        public OnePair(RankEnum pairRank, RankEnum kicker1, RankEnum kicker2, RankEnum kicker3)
        {
            PairRank = pairRank;
            Kicker1 = kicker1;
            Kicker2 = kicker2;
            Kicker3 = kicker3;
        }

        public override int HandRank => 2;
        public override int CompareToOffsuitHand(OffsuitHandBase other)
        {
            var otherOnePair = other as OnePair;
            if (otherOnePair == null)
            {
                return HandRank.CompareTo(other.HandRank);
            }

            if (PairRank != otherOnePair.PairRank)
            {
                return PairRank.CompareTo(otherOnePair.PairRank);
            }
            if (Kicker1 == otherOnePair.Kicker1)
            {
                return Kicker1.CompareTo(otherOnePair.Kicker1);
            }
            if (Kicker2 == otherOnePair.Kicker2)
            {
                return Kicker2.CompareTo(otherOnePair.Kicker2);
            }

            return Kicker3.CompareTo(otherOnePair.Kicker3);
        }
    }
}
