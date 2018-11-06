using Models;
using Models.Ranging;

namespace Common
{
    public class EquityCalculator
    {
        private readonly IFiveCardsEnumerator _fiveCardsEnumerator;

        public EquityCalculator(IFiveCardsEnumerator fiveCardsEnumerator)
        {
            _fiveCardsEnumerator = fiveCardsEnumerator;
        }

        public double CalculateEquity(HoldingHoles heroHoles, PlayerRange villainRange)
        {
            var pkStage = new PkStage(_fiveCardsEnumerator.Enumerate, Utils.EnumerateAvailableHoles);
            var pkResult = pkStage.Pk(heroHoles, villainRange.CloneToPkRange());

            return (double)pkResult.HeroWinScenariosCount /
                   (pkResult.HeroWinScenariosCount + pkResult.VillainWinScenariosCount + pkResult.TiedScenariosCount);
        }
    }
}
