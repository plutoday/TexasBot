using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TexasBot.Models.Hands;

namespace TexasBot.Models
{
    public class HandValue : IComparable<HandValue>
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public HandEnum HandEnum { get; set; }
        public IHand Hand { get; set; }

        public HandValue(HandEnum handEnum, IHand hand)
        {
            HandEnum = handEnum;
            Hand = hand;

            Validate();
        }

        public int CompareTo(HandValue other)
        {
            if (HandEnum == other.HandEnum)
            {
                return Hand.CompareTo(other.Hand);
            }

            return HandEnum.CompareTo(other.HandEnum);
        }

        public override string ToString()
        {
            return Hand.ToString();
        }

        private void Validate()
        {

            switch (HandEnum)
            {
                case HandEnum.RoyalFlush:
                    if (!(Hand is RoyalFlush))
                    {
                        throw new InvalidOperationException();
                    }
                    break;

                case HandEnum.StraightFlush:
                    if (!(Hand is StraightFlush))
                    {
                        throw new InvalidOperationException();
                    }
                    break;

                case HandEnum.FourOfAKind:
                    if (!(Hand is FourOfAKind))
                    {
                        throw new InvalidOperationException();
                    }
                    break;
                case HandEnum.FullHouse:
                    if (!(Hand is FullHouse))
                    {
                        throw new InvalidOperationException();
                    }
                    break;
                case HandEnum.Flush:
                    if (!(Hand is Flush))
                    {
                        throw new InvalidOperationException();
                    }
                    break;
                case HandEnum.Straight:
                    if (!(Hand is Straight))
                    {
                        throw new InvalidOperationException();
                    }
                    break;
                case HandEnum.ThreeOfAKind:
                    if (!(Hand is ThreeOfAKind))
                    {
                        throw new InvalidOperationException();
                    }
                    break;
                case HandEnum.TwoPairs:
                    if (!(Hand is TwoPairs))
                    {
                        throw new InvalidOperationException();
                    }
                    break;
                case HandEnum.OnePair:
                    if (!(Hand is OnePair))
                    {
                        throw new InvalidOperationException();
                    }
                    break;
                case HandEnum.HighCard:
                    if (!(Hand is HighCard))
                    {
                        throw new InvalidOperationException();
                    }
                    break;
            }
        }
    }
}
