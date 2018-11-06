using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Coaching.Postflop
{
    public class SevenCardsHand
    {
        public Card Hole1 { get; set; }
        public Card Hole2 { get; set; }
        public Card Flop1 { get; set; }
        public Card Flop2 { get; set; }
        public Card Flop3 { get; set; }
        public Card Turn { get; set; }
        public Card River { get; set; }

        public List<Card> Cards { get; set; } 

        public SevenCardsHand(Card hole1, Card hole2, Card flop1, Card flop2, Card flop3, Card turn, Card river)
        {
            Hole1 = hole1;
            Hole2 = hole2;
            Flop1 = flop1;
            Flop2 = flop2;
            Flop3 = flop3;
            Turn = turn;
            River = river;

            Cards = new List<Card>() {Hole1, Hole2, Flop1, Flop2, Flop3, Turn, River};

            Descriptor = Utils.GetDescriptorForSevenCardsHand(this);
        }

        public string Descriptor { get; set; }
    }
}
