using Models;
using River.Strategy.Headsup;
using River.Strategy.Multiway;

namespace River.Strategy
{
    public class RiverStrategy
    {
        private HeadsupRiverBettingStrategy _bettingStrategy;
        private HeadsupRiverCallingStrategy _callingStrategy;
        private MultiwayRiverBettingStrategy _multiwayBettingStrategy;
        private MultiwayRiverCallingStrategy _multiwayCallingStrategy;

        private bool _initialized = false;

        private void Init(RiverDecisionContext context)
        {
            _bettingStrategy = new HeadsupRiverBettingStrategy(context.RiverBoard, context.HeroHoles);
            _callingStrategy = new HeadsupRiverCallingStrategy();
            _multiwayBettingStrategy = new MultiwayRiverBettingStrategy(context.RiverBoard, context.HeroHoles);
            _multiwayCallingStrategy = new MultiwayRiverCallingStrategy(context.RiverBoard, context.HeroHoles);
            _initialized = true;
        }

        public Decision MakeDecision(RiverDecisionContext context)
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
