using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using TexasBot.Models;
using Card = Models.Card;

namespace Coaching
{
    public static class FindingHandsUtils
    {
        public static HandEnum FindBestHand(List<Card> sortedCards)
        {
            throw new NotImplementedException();
        }

        public static HandEnum FindStraight(List<Card> sortedCards)
        {
            throw new NotImplementedException();
        }

        public static List<Tuple<SuitEnum, List<RankEnum>>> SortBySuit(List<Card> cards)
        {
            var dict = new Dictionary<SuitEnum, List<RankEnum>>();
            foreach (var card in cards)
            {
                if (!dict.ContainsKey(card.Suit))
                {
                    dict.Add(card.Suit, new List<RankEnum>());
                }

                dict[card.Suit].Add(card.Rank);
            }

            var list = new List<Tuple<SuitEnum, List<RankEnum>>>();

            foreach (var entry in dict)
            {
                entry.Value.Sort();
                list.Add(new Tuple<SuitEnum, List<RankEnum>>(entry.Key, entry.Value));
            }

            list.Sort((t1, t2) => -(t1.Item2.Count().CompareTo(t2.Item2.Count())));

            return list;
        }

        public static List<Tuple<RankEnum, List<SuitEnum>>> SortByRank(List<Card> cards)
        {

            var dict = new Dictionary<RankEnum, List<SuitEnum>>();
            foreach (var card in cards)
            {
                if (!dict.ContainsKey(card.Rank))
                {
                    dict.Add(card.Rank, new List<SuitEnum>());
                }

                dict[card.Rank].Add(card.Suit);
            }

            var list = new List<Tuple<RankEnum, List<SuitEnum>>>();

            foreach (var entry in dict)
            {
                list.Add(new Tuple<RankEnum, List<SuitEnum>>(entry.Key, entry.Value));
            }

            list.Sort((t1, t2) =>
            {
                if (t1.Item2.Count != t2.Item2.Count)
                {
                    return -t1.Item2.Count.CompareTo(t2.Item2.Count);
                }
                else
                {
                    return -t1.Item1.CompareTo(t2.Item1);
                }
            });

            list.Sort((t1, t2) => -(t1.Item2.Count().CompareTo(t2.Item2.Count())));

            return list;
        }
    }
}
