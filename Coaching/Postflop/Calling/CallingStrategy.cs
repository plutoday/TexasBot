using Coaching.Postflop.Boards.BoardSpectrums;
using Coaching.Postflop.Ranging;
using Models;

namespace Coaching.Postflop.Calling
{
    public class CallingStrategy
    {
        private readonly RangeEstimator _rangeEstimator = new RangeEstimator();
        private readonly BoardSpectrumMaker _boardSpectrumMaker = new BoardSpectrumMaker();
        private readonly BoardSpectrumFilter _boardSpectrumFilter = new BoardSpectrumFilter();

        public Decision MakeDecision(PostflopStatusSummary statusSummary)
        {
            var requiredEquity = CalculateRequiredEquity(statusSummary);

            var villain = statusSummary.Raiser;
            var villainRange = _rangeEstimator.EstimateRange(statusSummary, villain);
            var boardSpectrum = _boardSpectrumMaker.MakeSpectrum(statusSummary.BoardStatus);
            var villainBoardSpectrum = _boardSpectrumFilter.FilterAgainstRange(boardSpectrum, villainRange);

            var heroEquityAgainstVillainRange = villainBoardSpectrum.GetEquity(statusSummary.Me.Holes);

            return MakeDecisionBasedOnEquityComparison(requiredEquity, heroEquityAgainstVillainRange, statusSummary);
        }

        private double CalculateRequiredEquity(PostflopStatusSummary statusSummary)
        {
            // calculate the required equity to call the raise
            // based on the PotSize
            // PotOdds
            int currentPotSize = statusSummary.PotSize;
            int chipsToCall = statusSummary.ChipsToCall;
            double potOdds = (double) chipsToCall/(currentPotSize + chipsToCall);

            //todo consider the implied odds?
            return potOdds;
        }

        private Decision MakeDecisionBasedOnEquityComparison(double requiredEquity, double myEquity, PostflopStatusSummary statusSummary)
        {
            if (myEquity < requiredEquity)
            {
                return new Decision(DecisionType.Fold, 0);
            }

            return new Decision(DecisionType.Call, statusSummary.ChipsToCall);

            //todo: implement the re-raise logic
        }
    }
}
