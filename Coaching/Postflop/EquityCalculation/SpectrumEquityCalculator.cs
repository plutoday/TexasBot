using Coaching.Postflop.Boards.BoardSpectrums;
using Coaching.Postflop.Ranging;
using Models;
using Models.Ranging;

namespace Coaching.Postflop.EquityCalculation
{
    public class SpectrumEquityCalculator
    {
        private readonly BoardSpectrumMaker _boardSpectrumMaker = new BoardSpectrumMaker();
        private readonly BoardSpectrumFilter _boardSpectrumFilter = new BoardSpectrumFilter();
        
        public double CalculateEquityAgainstARange(HoldingHoles heroHoles, PlayerRange villainPlayerRange, BoardSpectrum boardSpectrum)
        {
            var heroEquityAgainstVillainRange = boardSpectrum.GetEquity(heroHoles);

            return heroEquityAgainstVillainRange;
        }
    }
}
