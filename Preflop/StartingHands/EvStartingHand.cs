using Models;

namespace Preflop.StartingHands
{
    public class EvStartingHand : StartingHand
    {
        public double Ev { get; set; }
        public EvStartingHand(StartingHandTypeEnum type, RankEnum rank1, RankEnum rank2) : base(type, rank1, rank2)
        {
            Ev = StartingHandEvs.GetEv(Name);
        }
    }
}
