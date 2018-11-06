using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public static class Utils
    {
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
                    yield return new Card(suit, rank);
                }
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

        public static List<Tuple<RankEnum, int>> SortRanks(List<RankEnum> ranks)
        {
            Dictionary<RankEnum, int> dict = new Dictionary<RankEnum, int>();
            foreach (var rank in ranks)
            {
                if (!dict.ContainsKey(rank))
                {
                    dict.Add(rank, 0);
                }
                dict[rank]++;
            }

            List<Tuple<RankEnum, int>> list = dict.Select(entry => new Tuple<RankEnum, int>(entry.Key, entry.Value)).ToList();

            list.Sort((t1, t2) => -t1.Item1.CompareTo(t2.Item1));

            return list;
        }

        public static List<Tuple<RankEnum, int>> SortCounts(List<RankEnum> ranks)
        {
            Dictionary<RankEnum, int> dict = new Dictionary<RankEnum, int>();
            foreach (var rank in ranks)
            {
                if (!dict.ContainsKey(rank))
                {
                    dict.Add(rank, 0);
                }
                dict[rank]++;
            }

            List<Tuple<RankEnum, int>> list = dict.Select(entry => new Tuple<RankEnum, int>(entry.Key, entry.Value)).ToList();

            list.Sort((t1, t2) =>
            {
                if (t1.Item2 != t2.Item2)
                {
                    return -t1.Item2.CompareTo(t2.Item2);
                }
                return -t1.Item1.CompareTo(t2.Item1);
            });

            return list;
        }
        public static string GetStringForCard(this Card card)
        {
            return $"{GetStringForSuit(card.Suit)}{GetStringForRank(card.Rank)}";
        }

        public static string GetStringForMove(this Move move)
        {
            return $"{move.Player.Name}-{move.Decision.DecisionType}-{move.Decision.ChipsAdded}";
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
        public static T MaxEnum<T>(T enum1, T enum2) where T : IComparable<T>
        {
            return enum1.CompareTo(enum2) > 0 ? enum1 : enum2;
        }

        public static void RemoveElementEqualsTo<T>(this List<T> list, T target)
        {
            var toRemove = list.FirstOrDefault(e => e.Equals(target));

            if (toRemove != null)
            {
                list.Remove(toRemove);
            }
        }

        public static HashSet<DecisionType> GetCandidateDecisionTypes(bool isRaised)
        {
            if (isRaised)
            {
                return new HashSet<DecisionType>() { DecisionType.Fold, DecisionType.Call, DecisionType.Reraise, DecisionType.AllIn };
            }
            else
            {
                return new HashSet<DecisionType>() { DecisionType.Fold, DecisionType.Check, DecisionType.Raise, DecisionType.AllIn };
            }
        }

        public static PlayerRangeGridStatusEnum PickWorse(this PlayerRangeGridStatusEnum status1,
            PlayerRangeGridStatusEnum status2)
        {
            return WorseThan(status1, status2) > 0 ? status1 : status2;
        }

        public static int WorseThan(PlayerRangeGridStatusEnum status1, PlayerRangeGridStatusEnum status2)
        {
            return status1.CompareTo(status2);
        }
    }
}
