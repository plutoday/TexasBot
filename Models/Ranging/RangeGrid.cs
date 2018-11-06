using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Ranging
{
    public class RangeGrid
    {
        /// <summary>
        /// AKs, TT, etc
        /// </summary>
        public string Description { get; set; }

        public RankEnum HighRank { get; set; }
        public RankEnum LowRank { get; set; }   //might be same as HighRank if paired.
        
        public List<SuitEnum> Card1AvaliableSuits { get; set; }
        public List<SuitEnum> Card2AvaliableSuits { get; set; }

        public RangeGrid Clone()
        {
            return new RangeGrid()
            {
                HighRank = this.HighRank,
                LowRank = this.LowRank,
                Category = this.Category,
                Description = this.Description,
                Card1AvaliableSuits = new List<SuitEnum>(Card1AvaliableSuits),
                Card2AvaliableSuits = new List<SuitEnum>(Card2AvaliableSuits)
            };
        }

        public void EliminateConflicts(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
            {
                if (HighRank == card.Rank)
                {
                    Card1AvaliableSuits.RemoveElementEqualsTo(card.Suit);
                }
                if (LowRank == card.Rank)
                {
                    Card2AvaliableSuits.RemoveElementEqualsTo(card.Suit);
                }
            }
        }


        public int PossibleCount => Category == GridCategoryEnum.Paired
            ? (Card1AvaliableSuits.Count * (Card1AvaliableSuits.Count - 1) / 2)
            : (Category == GridCategoryEnum.Suited ? Math.Min(Card1AvaliableSuits.Count, Card2AvaliableSuits.Count) : (Card1AvaliableSuits.Count * Card2AvaliableSuits.Count));
        
        public int AvailableRankCombCount
        {
            get
            {
                switch (Category)
                {
                    case GridCategoryEnum.Suited:
                        return Math.Min(Card1AvaliableSuits.Count, Card2AvaliableSuits.Count);
                    case GridCategoryEnum.Paired:
                        return Card1AvaliableSuits.Count * (Card2AvaliableSuits.Count - 1) / 2;
                    case GridCategoryEnum.Offsuit:
                        return Card1AvaliableSuits.Sum(card1AvaliableSuit => Card2AvaliableSuits.Count(card2AvaliableSuit => card1AvaliableSuit != card2AvaliableSuit));
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public int TotalCount
        {
            get
            {
                switch (Category)
                {
                        case GridCategoryEnum.Suited:
                        return 4;
                        case GridCategoryEnum.Paired:
                        return 6;
                        case GridCategoryEnum.Offsuit:
                        return 12;
                    default:
                        throw new InvalidCastException();
                }
            }
        }

        public GridCategoryEnum Category { get; set; }
        
        public RangeGrid(RankEnum rank1, RankEnum rank2, bool suited)
        {
            HighRank = rank1 > rank2 ? rank1 : rank2;
            LowRank = rank1 > rank2 ? rank2 : rank1;
            Category = suited ? GridCategoryEnum.Suited : rank1 == rank2 ? GridCategoryEnum.Paired : GridCategoryEnum.Offsuit;
            Card1AvaliableSuits = new List<SuitEnum>() { SuitEnum.Heart, SuitEnum.Spade, SuitEnum.Diamond, SuitEnum.Club };
            Card2AvaliableSuits = new List<SuitEnum>() { SuitEnum.Heart, SuitEnum.Spade, SuitEnum.Diamond, SuitEnum.Club };
        }

        public RangeGrid(HoldingHoles holes): this(holes.Hole1.Rank, holes.Hole2.Rank, holes.Hole1.Suit == holes.Hole2.Suit)
        { }

        private RangeGrid()
        {
        }

        public override bool Equals(object obj)
        {
            var grid = obj as RangeGrid;
            if (grid == null)
            {
                throw new InvalidOperationException();
            }

            return HighRank == grid.HighRank && LowRank == grid.LowRank && Category == grid.Category;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Utils.GetStringForRank(HighRank) + Utils.GetStringForRank(LowRank) +
                   (Category == GridCategoryEnum.Paired ? "" : Category == GridCategoryEnum.Suited ? "s" : "o"));

            return sb.ToString();
        }

        public string ToStringFull()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Utils.GetStringForRank(HighRank) + Utils.GetStringForRank(LowRank) +
                   (Category == GridCategoryEnum.Paired ? "" : Category == GridCategoryEnum.Suited ? "s" : "o"));
            sb.Append($"|{string.Join("", Card1AvaliableSuits.Select(Utils.GetStringForSuit).ToArray())}")
                .Append($"|{string.Join("", Card2AvaliableSuits.Select(Utils.GetStringForSuit).ToArray())}");

            return sb.ToString();
        }
    }
}
