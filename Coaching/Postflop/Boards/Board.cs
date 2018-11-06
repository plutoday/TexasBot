using Models;

namespace Coaching.Postflop.Boards
{
    public class Board
    {
        public Card Flop1 { get; set; }
        public Card Flop2 { get; set; }
        public Card Flop3 { get; set; }
        public Card Turn { get; set; }
        public Card River { get; set; }

        public Board(Card flop1, Card flop2, Card flop3, Card turn, Card river)
        {
            Flop1 = flop1;
            Flop2 = flop2;
            Flop3 = flop3;
            Turn = turn;
            River = river;
        }
    }
}
