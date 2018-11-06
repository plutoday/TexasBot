using Models;
using Strategy;

namespace Terminal.Deal
{
    public class PlayerProxy : IPlayerProxy
    {
        private readonly Brain _brain;

        private RoundSetup _roundSetup;

        public PlayerProxy()
        {
            _brain = new Brain();
        }

        public void DealHoles(int index, Card hole1, Card hole2)
        {
            _roundSetup = new RoundSetup();
            _roundSetup.HeroIndex = index;
            _roundSetup.Hole1 = hole1;
            _roundSetup.Hole2 = hole2;
        }

        public Decision GetDecision(Round round)
        {
            return _brain.GetDecision(round, _roundSetup);
        }
    }
}
