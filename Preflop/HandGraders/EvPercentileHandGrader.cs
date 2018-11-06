using Preflop.StartingHands;

namespace Preflop.HandGraders
{
    public class EvPercentileHandGrader : IHandGrader
    {
        private readonly HandRangePercentiles _percentiles;

        public EvPercentileHandGrader(HandRangePercentiles percentiles)
        {
            _percentiles = percentiles;
        }

        public HandValueGradeEnum GradeAHand(StartingHand startingHand)
        {
            var percentile = StartingHandEvs.GetPercentila(startingHand.Name);
            if (percentile <= _percentiles.AllInPercentile)
            {
                return HandValueGradeEnum.AllIn;
            }

            if (percentile <= _percentiles.ValuePercentile)
            {
                return HandValueGradeEnum.BetForValue;
            }

            if (percentile <= _percentiles.FlatPercentile)
            {
                return HandValueGradeEnum.Flat;
            }

            if (percentile <= _percentiles.BluffPercentile)
            {
                return HandValueGradeEnum.BetForBluff;
            }

            return HandValueGradeEnum.Fold;
        }
    }
}
