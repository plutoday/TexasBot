using Models;

namespace Coaching.Postflop
{
    public class FlopHand
    {
        public Card Hole1 { get; set; }
        public Card Hole2 { get; set; }
        public Card Flop1 { get; set; }
        public Card Flop2 { get; set; }
        public Card Flop3 { get; set; }

        public FlopHand(Card hole1, Card hole2, Card flop1, Card flop2, Card flop3)
        {
            Hole1 = hole1;
            Hole2 = hole2;
            Flop1 = flop1;
            Flop2 = flop2;
            Flop3 = flop3;
        }
    }
}
