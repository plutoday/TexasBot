using Models.Hand.SuitedHand;

namespace Models.Hand.OffsuitHand
{
    public abstract class OffsuitHandBase : IHand
    {
        public abstract int HandRank { get; }

        public abstract int CompareToOffsuitHand(OffsuitHandBase other);

        public int CompareTo(IHand suitedHand)
        {
            if (suitedHand is SuitedHandBase)
            {
                return HandRank.CompareTo(suitedHand.HandRank);
            }
            else
            {
                return CompareToOffsuitHand(suitedHand as OffsuitHandBase);
            }
        }
    }
}
