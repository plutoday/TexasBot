using Models;

namespace Terminal.Deal
{
    public interface IPlayerProxy
    {
        void DealHoles(int index, Card hole1, Card hole2);

        Decision GetDecision(Round round);
    }
}
