using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;
using TexasBot.Models;
using TexasBot.Models.Hands;
using TexasBot.Models.HandSummaries;
using Card = TexasBot.Models.Card;

namespace TexasBot.Tools
{
    public static class Utils
    {
        private static string _scoreFile = @"D:\TexasBot\records.json";
        private static string _handOf7File = @"D:\TexasBot\handOf7Records.json";

        private static Dictionary<string, HandSlimRecord> _records = 
            JsonConvert.DeserializeObject<List<HandSlimRecord>>(File.ReadAllText(_scoreFile)).ToDictionary(r => r.HandString, r => r);

        private static Dictionary<string, HandOf7SummarySlimRecord> _handOf7SummarySlimRecordsDict;

        public static void Init()
        {
            var handOf7SummarySlimRecords =
                JsonConvert.DeserializeObject<List<HandOf7SummarySlimRecord>>(File.ReadAllText(_handOf7File));
            _handOf7SummarySlimRecordsDict = new Dictionary<string, HandOf7SummarySlimRecord>();

            foreach (var handOf7SummarySlimRecord in handOf7SummarySlimRecords)
            {
                _handOf7SummarySlimRecordsDict.Add(handOf7SummarySlimRecord.RecordString, handOf7SummarySlimRecord);
            }
        } 

        public static int GetScoreFor5Cards(HandOf5 handOf5)
        {
            return _records[Utils.GetStringForCards(handOf5.Cards)].Score;
        }

        public static List<Card> SortCards(IEnumerable<Card> cards)
        {
            var tuples = cards.Select(c => new Tuple<string, Card>(GetStringForCard(c), c)).ToList();
            tuples.Sort((c1, c2) => string.Compare(c1.Item1, c2.Item1, StringComparison.InvariantCultureIgnoreCase));
            return tuples.Select(t => t.Item2).ToList();
        }

        public static string GetStringForCards(IEnumerable<Card> cards)
        {
            var sb = new StringBuilder();
            foreach (var card in SortCards(cards))
            {
                sb.Append(GetStringForCard(card));
            }
            return sb.ToString();
        }

        public static string GetStringForCard(Card card)
        {
            return $"{GetStringForSuit(card.Suit)}{GetStringForRank(card.Rank)}";
        }

        public static string GetStringForSuit(SuitEnum suit)
        {
            switch (suit)
            {
                case SuitEnum.Diamond:
                    return "D";
                case SuitEnum.Club:
                    return "C";
                case SuitEnum.Heart:
                    return "H";
                case SuitEnum.Spade:
                    return "S";
            }

            throw new NotSupportedException();
        }

        public static string GetStringForRank(RankEnum rank)
        {
            switch (rank)
            {
                case RankEnum.Two:
                    return "2";
                case RankEnum.Three:
                    return "3";
                case RankEnum.Four:
                    return "4";
                case RankEnum.Five:
                    return "5";
                case RankEnum.Six:
                    return "6";
                case RankEnum.Seven:
                    return "7";
                case RankEnum.Eight:
                    return "8";
                case RankEnum.Nine:
                    return "9";
                case RankEnum.Ten:
                    return "T";
                case RankEnum.Jack:
                    return "J";
                case RankEnum.Queen:
                    return "Q";
                case RankEnum.King:
                    return "K";
                case RankEnum.Ace:
                    return "A";
            }

            throw new NotSupportedException();
        }

        public static List<HandOf5> GenerateAllPossibleHands()
        {
            var cards = GenerateAllCards().ToArray();
            return EnumerateAllCombinations(cards).Select(c => new HandOf5(c.ToList())).ToList();
        }

        public static IEnumerable<Card> GenerateAllCards()
        {
            SuitEnum[] suits = new[] { SuitEnum.Diamond, SuitEnum.Club, SuitEnum.Heart, SuitEnum.Spade };
            RankEnum[] ranks = new[]
            {
                RankEnum.Two,
                RankEnum.Three,
                RankEnum.Four,
                RankEnum.Five,
                RankEnum.Six,
                RankEnum.Seven,
                RankEnum.Eight,
                RankEnum.Nine,
                RankEnum.Ten,
                RankEnum.Jack,
                RankEnum.Queen,
                RankEnum.King,
                RankEnum.Ace,
            };

            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    yield return new Card() { Suit = suit, Rank = rank };
                }
            }
        }

        public static IEnumerable<List<Card>> EnumerateAllCombinations(Card[] cards)
        {
            foreach (var combination in Utils.Enumerate(cards, 0, 5))
            {
                yield return combination.ToList();
            }
        }

        public static IEnumerable<IEnumerable<T>> Enumerate<T>(T[] cards, int index, int count)
        {
            if (count == 0)
            {
                yield return new List<T>();
                yield break;
            }

            if (cards.Length == index + count)
            {
                var combination = new List<T>();
                for (int i = index; i < cards.Length; i++)
                {
                    combination.Add(cards[i]);
                }
                yield return combination;
            }
            else
            {
                foreach (var combination in Enumerate(cards, index + 1, count - 1))
                {
                    var comb = combination.ToList();
                    comb.Insert(0, cards[index]);
                    yield return comb;
                }

                foreach (var combination in Enumerate(cards, index + 1, count))
                {
                    yield return combination;
                }
            }
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

        public static int FindBestHandOf5(HandOf7 handOf7)
        {
            if (_handOf7SummarySlimRecordsDict == null)
            {
                Init();
            }

            var handOf7String = new HandOf7Summary(handOf7).ToString();
            var handOf7SlimRecord = _handOf7SummarySlimRecordsDict[handOf7String];

            return handOf7SlimRecord.Score;
        }
    }
}
