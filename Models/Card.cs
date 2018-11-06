using System;

namespace Models
{
    public class Card : IComparable<Card>
    {
        public SuitEnum Suit { get; set; }

        public RankEnum Rank { get; set; }

        public Card()
        {
        }

        public Card(SuitEnum suit, RankEnum rank)
        {
            Suit = suit;
            Rank = rank;
        }

        public int CompareTo(Card other)
        {
            if (Rank == other.Rank)
            {
                return Suit.CompareTo(other.Suit);
            }

            return Rank.CompareTo(other.Rank);
        }

        public override bool Equals(object obj)
        {
            var card = obj as Card;
            if (card == null)
            {
                return false;
            }

            return Suit == card.Suit && Rank == card.Rank;
        }
    }
}