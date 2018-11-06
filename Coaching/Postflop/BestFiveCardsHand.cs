using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Coaching.Postflop
{
    public class BestFiveCardsHand
    {
        /// <summary>
        /// Ranked by rank, then by suit in the order of Heart/Spade/Diamond/Club
        /// </summary>
        public List<Card> Cards { get; set; }

        public BestFiveCardsHand(List<Card> cards)
        {
            Cards = new List<Card>(cards);
        }
    }
}
