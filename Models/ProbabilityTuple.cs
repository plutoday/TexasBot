namespace Models
{
    public class ProbabilityTuple
    {
        public string VillainName { get; set; }
        public ProbabilityEnum ProbabilityCategory { get; set; }
        public double Probability { get; set; }

        public ProbabilityTuple(string villainName, ProbabilityEnum category, double probability)
        {
            VillainName = villainName;
            ProbabilityCategory = category;
            Probability = probability;
        }
    }
}
