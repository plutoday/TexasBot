using System.Collections.Generic;

namespace Models
{
    public class VillainProbabilityResult
    {
        public string VillainName { get; set; }

        public  Dictionary<ProbabilityEnum, double> Probabilities { get; set; }

        public VillainProbabilityResult(Dictionary<ProbabilityEnum, double> probabilities, string villainName)
        {
            VillainName = villainName;
            Probabilities = new Dictionary<ProbabilityEnum, double>(probabilities);
        }
    }

    public enum ProbabilityEnum
    {
        Fold,
        CallWin,
        CallTie,
        CallLose,
        Reraise,    //reserved for future optimization
    }
}
