using Preflop.StartingHands;

namespace Preflop.HandGraders
{
    public interface IHandGrader
    {
        HandValueGradeEnum GradeAHand(StartingHand startingHand);
    }
}
