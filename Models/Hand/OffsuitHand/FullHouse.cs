namespace Models.Hand.OffsuitHand
{
    public class FullHouse : OffsuitHandBase
    {
        public override int HandRank => 7;

        public RankEnum TriRank { get; set; }
        public RankEnum PairRank { get; set; }

        public FullHouse(RankEnum triRank, RankEnum pairRank)
        {
            TriRank = triRank;
            PairRank = pairRank;
        }

        public override int CompareToOffsuitHand(OffsuitHandBase other)
        {
            var otherFullHouse = other as FullHouse;
            if (otherFullHouse == null)
            {
                return HandRank.CompareTo(other.HandRank);
            }

            if (TriRank != otherFullHouse.TriRank)
            {
                return TriRank.CompareTo(otherFullHouse.TriRank);
            }

            return PairRank.CompareTo(otherFullHouse.PairRank);
        }
    }
}
