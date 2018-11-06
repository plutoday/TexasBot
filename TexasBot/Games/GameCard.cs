using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasBot.Models;

namespace TexasBot.Games
{
    public class GameCard
    {
        public bool Known { get; set; }
        public List<Card> Excluded { get; set; }
        public List<Card> PossibleCards { get; set; }

        public Card Card { get; set; }

        public void SetCard(Card card)
        {
            Card = card;
            Known = true;
        }
    }
}
