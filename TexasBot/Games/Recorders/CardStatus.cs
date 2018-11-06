using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasBot.Models;

namespace TexasBot.Games.Recorders
{
    public class CardStatus
    {
        public List<Card> Holes { get; set; }
        public List<Card> Flops { get; set; }
        public Card Turn { get; set; }
        public Card River { get; set; }
    }
}
