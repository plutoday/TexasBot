using System.Collections.Generic;
using System.Linq;
using TexasBot.Tools;

namespace TexasBot.Models
{
    public class HandOf7
    {
        public Card[] Cards;

        public HandOf7(List<Card> cards)
        {
            Cards = cards.ToArray();
        }

        public Card Hole1 => Cards[0];
        public Card Hole2 => Cards[1];
        public Card Flop1 => Cards[2];
        public Card Flop2 => Cards[3];
        public Card Flop3 => Cards[4];
        public Card Turn => Cards[5];
        public Card River => Cards[6];

        public HandOf5 FindBestHandOf5()
        {
            var handsOf5 = Utils.EnumerateAllCombinations(Cards).Select(c => new HandOf5(c)).ToList();
            var handCalculator = new HandCalculator();
            foreach (var handOf5 in handsOf5)
            {
                handOf5.HandValue = handCalculator.CalculateHandValueFor5Cards(handOf5);
                handOf5.Score = Utils.GetScoreFor5Cards(handOf5);
            }

            handsOf5.Sort((h1, h2) => -h1.Score.CompareTo(h2.Score));

            return handsOf5.First();
        }
    }
}
