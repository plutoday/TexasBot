
using Models.Hand.OffsuitHand;

namespace Models.Hand.SuitedHand
{
    public abstract class SuitedHandBase : IHand
    {
        public abstract int HandRank { get; }
        public abstract int CompareToSuitedHand(SuitedHandBase other);

        public int CompareTo(IHand other)
        {
            if (other is OffsuitHandBase)
            {
                return HandRank.CompareTo(other.HandRank);
            }
            else
            {
                return CompareToSuitedHand(other as SuitedHandBase);
            }
        }
    }
}
