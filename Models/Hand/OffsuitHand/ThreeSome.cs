namespace Models.Hand.OffsuitHand
{
    public class ThreeSome : OffsuitHandBase
    {
        public override int HandRank => 4;

        public RankEnum TriRank { get; set; }
        public RankEnum Kicker1Rank { get; set; }
        public RankEnum Kicker2Rank { get; set; }

        public ThreeSome(RankEnum triRank, RankEnum kicker1, RankEnum kicker2)
        {
            TriRank = triRank;
            Kicker1Rank = kicker1;
            Kicker2Rank = kicker2;
        }

        public override int CompareToOffsuitHand(OffsuitHandBase other)
        {
            var otherThreeSome = other as ThreeSome;
            if (otherThreeSome == null)
            {
                return HandRank.CompareTo(other.HandRank);
            }

            if (TriRank != otherThreeSome.TriRank)
            {
                return TriRank.CompareTo(otherThreeSome.TriRank);
            }

            if (Kicker1Rank != otherThreeSome.Kicker1Rank)
            {
                return Kicker1Rank.CompareTo(otherThreeSome.Kicker1Rank);
            }

            return Kicker2Rank.CompareTo(otherThreeSome.Kicker2Rank);
        }
    }
}
