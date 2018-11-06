using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using TexasBot.Tools;

namespace TexasBot.Models.HandSummaries
{
    public class HandOf7Summary
    {
        public List<RankEnum> Ranks { get; set; }

        //This has value only when there're at least five cards of the same color.
        public List<RankEnum> SameColorRanks { get; set; }

        public HandOf7Summary(HandOf7 handOf7)
        {
            Ranks = handOf7.Cards.Select(c => c.Rank).ToList();
            Ranks.Sort();

            var dict = new Dictionary<SuitEnum, int>();
            foreach (var card in handOf7.Cards)
            {
                if (!dict.ContainsKey(card.Suit))
                {
                    dict.Add(card.Suit, 0);
                }

                dict[card.Suit]++;
            }

            foreach (var entry in dict)
            {
                if (entry.Value >= 5)
                {
                    SameColorRanks = handOf7.Cards.Where(c => c.Suit == entry.Key).Select(c => c.Rank).ToList();
                    SameColorRanks.Sort();
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(string.Join("", Ranks.Select(TexasBot.Tools.Utils.GetStringForRank)));
            if (SameColorRanks != null && SameColorRanks.Count >= 5)
            {
                sb.Append($"|{string.Join("", SameColorRanks.Select(TexasBot.Tools.Utils.GetStringForRank))}");
            }

            return sb.ToString();
        }
    }
}
