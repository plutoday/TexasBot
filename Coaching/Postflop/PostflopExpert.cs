using System;
using System.Linq;
using Coaching.Postflop.Betting;
using Coaching.Postflop.Calling;
using Models;

namespace Coaching.Postflop
{
    public class PostflopExpert
    {
        private readonly BettingStrategy _bettingStrategy = new BettingStrategy();
        private readonly CallingStrategy _callingStrategy = new CallingStrategy();

        public Decision GetPostflopDecision(Round round)
        {
            var statusSummary = Utils.GeneratePostflopStatusSummary(round);
            if (statusSummary.IsRaised)
            {
                return _callingStrategy.MakeDecision(statusSummary);
            }
            else
            {
                return _bettingStrategy.MakeDecision(statusSummary);
            }
        }
    }
}
