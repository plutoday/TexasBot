using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace TexasBot.Models
{
    public class Card : IComparable<Card>
    {
        protected bool Equals(Card other)
        {
            return Suit == other.Suit && Rank == other.Rank;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Card) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Suit * 397) ^ (int) Rank;
            }
        }

        public SuitEnum Suit { get;set; }
        public RankEnum Rank { get; set; }

        public bool Determined { get; set; }
        public List<Card> Excluded { get; set; }

        public int CompareTo(Card other)
        {
            int rankDiff = -Rank.CompareTo(other.Rank);
            return rankDiff != 0 ? rankDiff : Suit.CompareTo(other.Suit);
        }

        public override string ToString()
        {
            return $"{Suit}-{Rank}";
        }
    }
}
