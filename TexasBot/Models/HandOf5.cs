using System;
using System.Collections.Generic;
using System.Linq;

namespace TexasBot.Models
{
    public class HandOf5 : IComparable<HandOf5>
    {
        public List<Card> Cards { get; set; }

        public HandValue HandValue { get; set; }

        public int Score { get; set; }

        public HandOf5(List<Card> cards)
        {
            Cards = cards;
        }

        public override string ToString()
        {
            return string.Join(",", Cards.Select(c => c.ToString()));
        }

        public int CompareTo(HandOf5 other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return HandValue.CompareTo(other.HandValue);
        }
    }
}
