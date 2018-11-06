namespace Preflop.HandGraders
{
    public class HandRangePercentiles
    {
        public double AllInPercentile { get; }
        public double ValuePercentile { get; }
        public double FlatPercentile { get; }
        public double BluffPercentile { get; }

        //Fold anything else

        public HandRangePercentiles(double allIn, double value, double flat, double bluff)
        {
            AllInPercentile = allIn;
            ValuePercentile = value;
            FlatPercentile = flat;
            BluffPercentile = bluff;
        }
    }
}
