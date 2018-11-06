using Models;
using Preflop.HandGraders;

namespace Preflop
{
    public class HandRangeConsultant
    {
        private readonly HandRangePercentileConsultant _percentileConsultant;
        private readonly SmallBlindHandRangePercentileConsultant _sbPercentileConsultant;
        private readonly BigBlindHandRangePercentileConsultant _bbPercentileConsultant;

        public HandRangeConsultant()
        {
            _percentileConsultant = new HandRangePercentileConsultant();
            _sbPercentileConsultant = new SmallBlindHandRangePercentileConsultant();
            _bbPercentileConsultant = new BigBlindHandRangePercentileConsultant();
        }

        public IHandGrader GenerateHandGrader(PreflopStatusSummary statusSummary)
        {
            switch (statusSummary.Me.Position)
            {
                case PositionEnum.SmallBlind:
                    return new EvPercentileHandGrader(_sbPercentileConsultant.GetPercentiles(statusSummary));
                case PositionEnum.BigBlind:
                    return new EvPercentileHandGrader(_bbPercentileConsultant.GetPercentiles(statusSummary));
                default:
                    return new EvPercentileHandGrader(_percentileConsultant.GetPercentiles(statusSummary));
            }
        }
    }
}
