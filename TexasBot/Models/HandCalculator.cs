using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;
using TexasBot.Models.Hands;
using TexasBot.Tools;

namespace TexasBot.Models
{
    public class HandCalculator
    {
        public HandValue CalculateHandValueFor5Cards(HandOf5 handOf5)
        {
            var handValue = FindRoyalFlush(handOf5);
            if (handValue != null)
            {
                return handValue;
            }

            handValue = FindStraightFlush(handOf5);
            if (handValue != null)
            {
                return handValue;
            }

            handValue = FindFourOfAKind(handOf5);
            if (handValue != null)
            {
                return handValue;
            }

            handValue = FindFullHouse(handOf5);
            if (handValue != null)
            {
                return handValue;
            }

            handValue = FindFlush(handOf5);
            if (handValue != null)
            {
                return handValue;
            }

            handValue = FindStraight(handOf5);
            if (handValue != null)
            {
                return handValue;
            }

            handValue = FindThreeOfAKind(handOf5);
            if (handValue != null)
            {
                return handValue;
            }

            handValue = FindTwoPairs(handOf5);
            if (handValue != null)
            {
                return handValue;
            }

            handValue = FindOnePair(handOf5);
            if (handValue != null)
            {
                return handValue;
            }

            handValue = FindHighCard(handOf5);
            if (handValue != null)
            {
                return handValue;
            }

            throw new InvalidOperationException();
        }

        private HandValue FindRoyalFlush(HandOf5 handOf5)
        {
            var handValue = FindStraightFlush(handOf5);
            if (handValue == null)
            {
                return null;
            }

            var highRank = ((StraightFlush) (handValue.Hand)).HighRank;
            if (highRank != RankEnum.Ace)
            {
                return null;
            }

            return new HandValue(HandEnum.RoyalFlush, new RoyalFlush());
        }

        private HandValue FindStraightFlush(HandOf5 handOf5)
        {
            var cards = handOf5.Cards;
            cards.Sort();

            if (FindFlush(handOf5) == null)
            {
                return null;
            }

            if (FindStraight(handOf5) == null)
            {
                return null;
            }

            return new HandValue(HandEnum.StraightFlush, new StraightFlush(cards.Max(c => c.Rank)));
        }

        private HandValue FindFourOfAKind(HandOf5 handOf5)
        {
            var sortResult = Tools.Utils.SortByRank(handOf5.Cards);
            if (sortResult[0].Item2.Count == 4)
            {
                return new HandValue(HandEnum.FourOfAKind, new FourOfAKind(sortResult[0].Item1));
            }

            return null;
        }

        private HandValue FindFullHouse(HandOf5 handOf5)
        {
            var sortResult = Tools.Utils.SortByRank(handOf5.Cards);
            if (sortResult[0].Item2.Count == 3 && sortResult[1].Item2.Count == 2)
            {
                return new HandValue(HandEnum.FullHouse, new FullHouse(sortResult[0].Item1));
            }

            return null;
        }

        private HandValue FindFlush(HandOf5 handOf5)
        {
            var sortResult = Tools.Utils.SortBySuit(handOf5.Cards);
            if (sortResult[0].Item2.Count == 5)
            {
                return new HandValue(HandEnum.Flush, new Flush(sortResult.SelectMany(e => e.Item2).ToList()));
            }

            return null;
        }

        private HandValue FindStraight(HandOf5 handOf5)
        {
            var sortResult = Tools.Utils.SortByRank(handOf5.Cards);
            if (sortResult.Count < 5)
            {
                return null;
            }

            for (int i = 0; i < 4; i++)
            {
                if (sortResult[i].Item1 != sortResult[i+1].Item1 + 1)
                {
                    return null;
                }
            }

            return new HandValue(HandEnum.Straight, new Straight(sortResult[0].Item1));
        }

        private HandValue FindThreeOfAKind(HandOf5 handOf5)
        {

            var sortResult = Tools.Utils.SortByRank(handOf5.Cards);
            if (sortResult[0].Item2.Count == 3)
            {
                return new HandValue(HandEnum.ThreeOfAKind, new ThreeOfAKind(sortResult[0].Item1));
            }

            return null;
        }

        private HandValue FindTwoPairs(HandOf5 handOf5)
        {
            var sortResult = Tools.Utils.SortByRank(handOf5.Cards);
            if (sortResult[0].Item2.Count == 2 && sortResult[1].Item2.Count == 2)
            {
                return new HandValue(HandEnum.TwoPairs, new TwoPairs(sortResult[0].Item1, sortResult[1].Item1, sortResult[2].Item1));
            }

            return null;
        }

        private HandValue FindOnePair(HandOf5 handOf5)
        {
            var sortResult = Tools.Utils.SortByRank(handOf5.Cards);
            if (sortResult[0].Item2.Count == 2)
            {
                return new HandValue(HandEnum.OnePair, new OnePair(sortResult[0].Item1, new List<RankEnum>()
                {
                    sortResult[1].Item1,
                    sortResult[2].Item1,
                    sortResult[3].Item1
                }));
            }

            return null;
        }

        private HandValue FindHighCard(HandOf5 handOf5)
        {
            try
            {
                var sortResult = Tools.Utils.SortByRank(handOf5.Cards);
                if (sortResult[0].Item2.Count == 1)
                {
                    return new HandValue(HandEnum.HighCard, new HighCard(new List<RankEnum>()
                    {
                        sortResult[0].Item1,
                        sortResult[1].Item1,
                        sortResult[2].Item1,
                        sortResult[3].Item1,
                        sortResult[4].Item1,
                    }));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return null;
        }
    }
}
