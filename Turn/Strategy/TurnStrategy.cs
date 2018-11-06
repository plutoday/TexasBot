using System;
using Models;
using Turn.Strategy.Headsup;
using Turn.Strategy.Multiway;

namespace Turn.Strategy
{
    public class TurnStrategy
    {
        private HeadsupTurnBettingStrategy _bettingStrategy;
        private HeadsupTurnCallingStrategy _callingStrategy;
        private TurnMultiwayBettingStrategy _multiwayBettingStrategy;
        private TurnMultiwayCallingStrategy _multiwayCallingStrategy;

        private bool _initialized = false;

        private void Init(TurnDecisionContext context)
        {
            _bettingStrategy = new HeadsupTurnBettingStrategy(context.TurnBoard, context.HeroHoles);
            _callingStrategy = new HeadsupTurnCallingStrategy();
            _multiwayBettingStrategy = new TurnMultiwayBettingStrategy(context.TurnBoard, context.HeroHoles);
            _multiwayCallingStrategy = new TurnMultiwayCallingStrategy(context.TurnBoard, context.HeroHoles);
            _initialized = true;
        }

        public Decision MakeDecision(TurnDecisionContext context)
        {
            if (!_initialized)
            {
                Init(context);
            }

            if (context.IsHeadsUp)
            {
                if (context.IsRaised)
                {
                    return _callingStrategy.MakeDecision(context);
                }
                else
                {
                    return _bettingStrategy.MakeDecision(context);
                }
            }
            else
            {
                if (context.IsRaised)
                {
                    return _multiwayCallingStrategy.MakeDecision(context);
                }
                else
                {
                    return _multiwayBettingStrategy.MakeDecision(context);
                }
            }
        }
    }
}
