using TexasBot.Models.HandSummaries;

namespace TexasBot.Models
{
    public class HolesPairCompareResult
    {
        public HolesSummarySlimRecord Holes1 { get; set; }
        public HolesSummarySlimRecord Holes2 { get; set; }
        public CompareResult CompareResult { get; set; }

        public HolesPairCompareResult(HolesSummarySlimRecord holes1, HolesSummarySlimRecord holes2,
            CompareResult result)
        {
            Holes1 = holes1;
            Holes2 = holes2;
            CompareResult = result;
        }
    }
}
